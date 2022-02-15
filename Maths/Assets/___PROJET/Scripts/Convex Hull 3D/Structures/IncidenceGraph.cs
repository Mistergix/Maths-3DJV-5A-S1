using System.Collections.Generic;
using System.Linq;
using ESGI.Common;
using Shapes;
using UnityEngine;

namespace ESGI.ConvexHull3D
{
    public class IncidenceGraph
    {
        public List<Vertex3D> vertices;
        public List<Edge3D> edges;
        public List<Face3D> faces;

        public IncidenceGraph()
        {
            vertices = new List<Vertex3D>();
            edges = new List<Edge3D>();
            faces = new List<Face3D>();
        }

        public Vertex3D Center { get; private set; }

        public void AddVertices(List<Vertex3D> v)
        {
            vertices.AddRange(v);
        }

        public void AddEdges(List<Edge3D> edge3Ds)
        {
            edges.AddRange(edge3Ds);
        }

        public void AddFaces(List<Face3D> face3Ds)
        {
            faces.AddRange(face3Ds);
        }

        public void DrawGraph(DisplayData data)
        {
            foreach (var point in vertices)
            {
                Draw.Disc(point.position, data.pointSize, point.GetColorFromNodeColor());
            }
            
            foreach (var edge in edges)
            {
                Draw.UseDashes = true;
                Draw.DashStyle = DashStyle.defaultDashStyle;
                Draw.Line(edge.p1.position, edge.p2.position, edge.GetColorFromNodeColor());
            }

            foreach (var face in faces)
            {
                face.DrawHyperPlan(this, data);
                face.DrawOrder(data);
            }
        }
        
        public void RemoveBlueElements()
        {
            vertices = vertices.Where(vertex3D => vertex3D.color != Node.NodeColor.Blue).ToList();
            edges = edges.Where(edge3D => edge3D.color != Node.NodeColor.Blue).ToList();
            faces = faces.Where(face => face.color != Node.NodeColor.Blue).ToList();
        }

        public IncidenceGraph GetPurpleGraph()
        {
            var pGraph = new IncidenceGraph();
            var pV = vertices.Where(v => v.color == Node.NodeColor.Purple).ToList();
            var pE = edges.Where(edge3D => edge3D.color == Node.NodeColor.Purple).ToList();
            pGraph.edges = pE;
            pGraph.vertices = pV;
            return pGraph;
        }

        public Vertex3D GetOtherPoint(Vertex3D p1, Vertex3D p2, Vertex3D p3)
        {
            return vertices.FirstOrDefault(v => !v.Equals(p1) && !v.Equals(p2) && !v.Equals(p3));
        }

        public void ComputeCenter()
        {
            var center = new Vector3();
            center = vertices.Aggregate(center, (current, vertex) => current + vertex.position);
            center /= vertices.Count;
            Center = new Vertex3D(center);
        }
    }
}