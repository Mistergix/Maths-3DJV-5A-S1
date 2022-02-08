using System.Collections.Generic;
using ESGI.Common;
using PGSauce.Core;
using Shapes;
using UnityEngine;
using Draw = Drawing.Draw;

namespace ESGI.ConvexHull3D
{
    public class IncidenceGraph
    {
        public List<Vertex3D> vertices;
        public List<Edge3D> edges;
        public List<Face3D> faces;
        
        public Color LineColor => Color.black;

        public IncidenceGraph()
        {
            vertices = new List<Vertex3D>();
            edges = new List<Edge3D>();
            faces = new List<Face3D>();
        }

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
                Shapes.Draw.Disc(point.position, data.pointSize, data.pointColor);
            }

            foreach (var triangle in faces)
            {
                Shapes.Draw.UseDashes = true;
                Shapes.Draw.DashStyle = DashStyle.defaultDashStyle;
                Shapes.Draw.Line(triangle.edge1.p1.position, triangle.edge1.p2.position, LineColor);
                Shapes.Draw.Line(triangle.edge2.p1.position, triangle.edge2.p2.position, LineColor);
                Shapes.Draw.Line(triangle.edge3.p1.position, triangle.edge3.p2.position, LineColor);
            }
            
            var drawnEdges = new HashSet<Edge3D>();
            foreach (var edge in edges)
            {
                //DrawEdge(edge, drawnEdges, data);
            }
        }

        private void DrawEdge(Edge3D edge, HashSet<Edge3D> drawnEdges, DisplayData data)
        {
            if(drawnEdges.Contains(edge)) {return;}
            FromTo(edge, out var from, out var to, data);
            Draw.Arrow(from, to, -Vector3.forward, data.arrowSize, PGColors.Redish);
            drawnEdges.Add(edge);
        }

        private void FromTo(Edge3D halfEdge, out Vector3 @from, out Vector3 to, DisplayData Data)
        {
            from = (Vector3) halfEdge.p1.position;
            to = (Vector3) halfEdge.p2.position;
            var dir = (to - @from).normalized;
            var right = Vector3.Cross(dir, -Vector3.forward);
            @from += dir * (Data.pointSize * Data.arrowStretchFactor);
            to -= dir * (Data.pointSize * Data.arrowStretchFactor);
            @from += right * Data.arrowOffset;
            to += right * Data.arrowOffset;
        }
    }
}