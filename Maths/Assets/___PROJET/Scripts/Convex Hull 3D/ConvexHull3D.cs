using System;
using System.Collections.Generic;
using System.Linq;
using ESGI.Common;
using Kit;
using PGSauce.Core;
using PGSauce.Core.PGDebugging;
using Shapes;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ESGI.ConvexHull3D
{
    public class ConvexHull3D : ImmediateModeShapeDrawer
    {
        [SerializeField] private Points3D points;
        [SerializeField] private MeshDrawer meshDrawer;

        public DisplayData Data => displayData;

        [SerializeField] private DisplayData displayData;
        
        private IncidenceGraph _convexHull;

        private void Update()
        {
            ComputeHull();
            meshDrawer.DrawMesh(_convexHull);
        }

        private void OnDrawGizmos()
        {
            if(_convexHull == null){return;}
            foreach (var vertex in _convexHull.vertices)
            {
                Handles.color = Color.blue;
                Handles.Label(vertex.position, vertex.index.ToString());
            }
        }

        public override void DrawShapes(Camera cam)
        {
            base.DrawShapes(cam);
            using (Draw.Command(cam))
            {
                if(_convexHull == null){return;}
                _convexHull.DrawGraph(Data);
            }
        }

        [Button]
        public void ComputeHull()
        {
            _convexHull = ComputeTetrahedre();
            for (int q = 4; q <= points.MaxQ; q++)
            {
                var point = points.positions[q];
                ComputeBlueAndRedFaces(point);
                var pointIsInsideConvexHull = _convexHull.faces.All(face3D => face3D.color != Node.NodeColor.Blue);
                if (pointIsInsideConvexHull)
                {
                    // 2A
                    foreach (var edge in _convexHull.edges)
                    {
                        edge.SetColor(Node.NodeColor.None);
                    }
                }
                else
                {
                    //2B
                    ColorEdges();
                    ColorVertices();
                    if (points.stopAtColoring && q == points.MaxQ)
                    {
                        continue;
                    }
                    RemoveBlueElements();
                    CombineWithPurpleGraph(point, q);
                }
            }
            Drawing.Draw.WireSphere(points.MaxQPoint, 0.5f, PGColors.Redish);
        }

        private void CombineWithPurpleGraph(Vector3 point, int index)
        {
            var purpleIGraph = _convexHull.GetPurpleGraph();
            var pointVertex = new Vertex3D(point, index);
            var newEdges = CreateEdgesFromPurpleGraph(purpleIGraph, pointVertex);
            var newFaces = CreateNewFaces(purpleIGraph, pointVertex);
            ConnectFacesAndEdges(newEdges, newFaces, pointVertex);
            _convexHull.vertices.Add(pointVertex);
        }

        private void ConnectFacesAndEdges(List<Edge3D> newEdges, List<Face3D> newFaces, Vertex3D pointVertex)
        {
            foreach (var edge in newEdges)
            {
                foreach (var face in newFaces)
                {
                    TryConnectFaceToEdge(edge, face, pointVertex);

                    if (edge.face1 != null && edge.face2 != null)
                    {
                        break;
                    }
                }
            }
        }

        private List<Face3D> CreateNewFaces(IncidenceGraph purpleIGraph, Vertex3D pointVertex)
        {
            var newFaces = new List<Face3D>();

            foreach (var edge in purpleIGraph.edges)
            {
                var face = GetRedFace(edge);
                var (begin, end) = GetOrderedVertices(face, edge);
                var newFace = CreateNewFace(begin, end, pointVertex, edge);
                newFaces.Add(newFace);
            }

            return newFaces;
        }

        private void TryConnectFaceToEdge(Edge3D edge, Face3D face, Vertex3D pointVertex)
        {
            // WE KNOW pointVertex belongs to edge AND face
            // We look at other vertex in edge, and see if it belongs to face
            // if not, then None
            // else, edge belongs to face
            // if pointVertex is at the end of edge on face, then Face1
            // else, it is at the beginning of edge on face, then Face2

            var otherVertex = edge.p1.Equals(pointVertex) ? edge.p2 : edge.p1;
            if (!face.HasEdge(pointVertex, otherVertex))
            {
                return;
            }

            if (IsBeginVertexOnFace(edge, pointVertex, face))
            {
                edge.face2 = face;
            }
            else
            {
                edge.face1 = face;
            }
        }

        private bool IsBeginVertexOnFace(Edge3D edge, Vertex3D testedVertex, Face3D face)
        {
            var (begin, end) = GetOrderedVertices(face, edge);
            return testedVertex.Equals(begin);
        }

        private List<Edge3D> CreateEdgesFromPurpleGraph(IncidenceGraph purpleIGraph, Vertex3D pointVertex)
        {
            var newEdges = new List<Edge3D>();

            foreach (var vertex in purpleIGraph.vertices)
            {
                var edge = new Edge3D(pointVertex, vertex);
                _convexHull.edges.Add(edge);
                newEdges.Add(edge);
            }

            return newEdges;
        }

        private Face3D CreateNewFace(Vertex3D begin, Vertex3D end, Vertex3D pointVertex, Edge3D edge)
        {
            var newFace = new Face3D(begin, end, pointVertex, _convexHull);
            _convexHull.faces.Add(newFace);
            if (edge.face1.color == Node.NodeColor.Blue)
            {
                edge.face1 = newFace;
            }
            else
            {
                edge.face2 = newFace;
            }

            return newFace;
        }

        private static Edge3D FindEdge(IEnumerable<Edge3D> newEdges, Vertex3D begin, Vertex3D end)
        {
            foreach (var edge in newEdges.Where(edge => EdgeEqualsTwoVertices(edge, begin, end)))
            {
                return edge;
            }

            throw new Exception(
                $"Edge not found, should not happen, if it does, it may be because the edges were not ALL created from purple graph");
        }

        private static (Vertex3D begin, Vertex3D end) GetOrderedVertices(Face3D face, Edge3D edge)
        {
            var begin = face.p1;
            var end = face.p2;
            
            if (EdgeEqualsTwoVertices(edge, begin, end))
            {
                return (begin, end);
            }
            
            begin = face.p2;
            end = face.p3;

            if (EdgeEqualsTwoVertices(edge, begin, end))
            {
                return (begin, end);
            }

            return (face.p3, face.p1);
        }

        private static bool EdgeEqualsTwoVertices(Edge3D edge, Vertex3D begin, Vertex3D end)
        {
            return edge.EqualsTwoVertices(begin, end);
        }

        private static Face3D GetRedFace(Edge3D edge)
        {
            return edge.face1.color == Node.NodeColor.Red ? edge.face1 : edge.face2;
        }

        private void RemoveBlueElements()
        {
            _convexHull.RemoveBlueElements();
        }

        private void ColorVertices()
        {
            var blueEdges = _convexHull.edges.Where(edge3D => edge3D.color == Node.NodeColor.Blue).ToList();
            var redEdge3Ds = _convexHull.edges.Where(edge3D => edge3D.color == Node.NodeColor.Red).ToList();
            var purpleEdges = _convexHull.edges.Where(edge3D => edge3D.color == Node.NodeColor.Purple).ToList();

            foreach (var edge3D in redEdge3Ds)
            {
                edge3D.p1.SetColor(Node.NodeColor.Red);
                edge3D.p2.SetColor(Node.NodeColor.Red);
            }

            foreach (var edge3D in blueEdges)
            {
                edge3D.p1.SetColor(Node.NodeColor.Blue);
                edge3D.p2.SetColor(Node.NodeColor.Blue);
            }

            foreach (var edge3D in purpleEdges)
            {
                edge3D.p1.SetColor(Node.NodeColor.Purple);
                edge3D.p2.SetColor(Node.NodeColor.Purple);
            }
        }

        private void ColorEdges()
        {
            foreach (var edge in _convexHull.edges)
            {
                if (edge.face1.color == Node.NodeColor.Red && edge.face2.color == Node.NodeColor.Red)
                {
                    edge.SetColor(Node.NodeColor.Red);
                }
                else if (edge.face1.color == Node.NodeColor.Blue && edge.face2.color == Node.NodeColor.Blue)
                {
                    edge.SetColor(Node.NodeColor.Blue);
                }
                else
                {
                    edge.SetColor(Node.NodeColor.Purple);
                }
            }
        }

        private void ComputeBlueAndRedFaces(Vector3 point)
        {
            foreach (var face in _convexHull.faces)
            {
                face.SetColorFromPoint(_convexHull, point);
            }
        }

        private IncidenceGraph ComputeTetrahedre()
        {
            if (points.positions.Count < 4)
            {
                PGDebug.Message($"Pas assez de points dans {points.name} (au moins 4)").LogError();
            }

            var iGraph = new IncidenceGraph();

            var four = points.positions.Take(4).ToList();
            
            var vertices = four.Select((point, i) => new Vertex3D(point, i)).ToList();

            var edge01 = new Edge3D(vertices[0], vertices[1]);
            var edge20 = new Edge3D(vertices[2], vertices[0]);
            var edge12 = new Edge3D(vertices[1], vertices[2]);
            var edge30 = new Edge3D(vertices[3], vertices[0]);
            var edge23 = new Edge3D(vertices[2], vertices[3]);
            var edge31 = new Edge3D(vertices[3], vertices[1]);

            var face012 = new Face3D(vertices[2], vertices[1], vertices[0]);
            var face013 = new Face3D(vertices[0], vertices[1], vertices[3]);
            var face123 = new Face3D(vertices[1], vertices[2], vertices[3]);
            var face023 = new Face3D(vertices[3], vertices[2], vertices[0]);

            edge01.face1 = face012;
            edge01.face2 = face013;

            edge20.face1 = face023;
            edge20.face2 = face012;

            edge30.face1 = face013;
            edge30.face2 = face023;

            edge12.face1 = face012;
            edge12.face2 = face123;

            edge23.face1 = face023;
            edge23.face2 = face123;

            edge31.face1 = face123;
            edge31.face2 = face013;

            iGraph.AddVertices(vertices);
            iGraph.AddEdges(new List<Edge3D>() {edge01, edge20, edge30, edge12, edge31, edge23});
            iGraph.AddFaces(new List<Face3D>() {face012, face013, face023, face123});

            return iGraph;
        }
    }
}