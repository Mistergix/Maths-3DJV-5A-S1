using System.Collections.Generic;
using System.Linq;
using ESGI.Common;
using PGSauce.Core;
using PGSauce.Core.PGDebugging;
using UnityEngine;

namespace ESGI.ConvexHull3D
{
    public class Face3D : Node
    {
        public Vertex3D p1, p2, p3;
        private IncidenceGraph _convexHull;
        public int index;

        public Face3D[] Adjacent => GetAdjacentFaces();

        /// <summary>
        /// Sets the order manually
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        public Face3D(Vertex3D p1, Vertex3D p2, Vertex3D p3)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }

        /// <summary>
        /// Sets the order automatically
        /// </summary>
        public Face3D(Vertex3D a, Vertex3D b, Vertex3D c, IncidenceGraph convexHull)
        {
            _convexHull = convexHull;
            var up = GetNormal(a, b, c);
            p1 = a;
            var center = (a.position + b.position + c.position) / 3f;
            var p1ToCenter = (center - p1.position).normalized;

            var perp = Vector3.Cross(p1ToCenter, (b.position - p1.position).normalized);
            var dir = Vector3.Dot(perp, up);
            if (dir > 0f)
            {
                p2 = c;
                p3 = b;
            }
            else if (dir < 0f)
            {
                p2 = b;
                p3 = c;
            }
            else
            {
               PGDebug.Message($"{a}, {b}, {c} sont alignés").LogWarning();
            }
        }
        
        public bool Remove(Face3D face)
        {
            if(face == null) return false;
            var n = Adjacent.Length;
			
            for(var i = 0; i < n; i++)
            {
                if(Adjacent[i] == null) continue;

                if (!ReferenceEquals(Adjacent[i], face)){ continue;}
                Adjacent[i] = null;
                return true;
            }
			
            return false;
        }

        public bool IsNormalFlipped { get; set; }

        public Vector3 GetNormal(Vertex3D a, Vertex3D b, Vertex3D c)
        {
            var plane = new Plane3D(a, b, c, _convexHull.GetOtherPoint(a,b,c));
            return plane.Normal;
        }
        
        public Vector3 GetNormal()
        {
            return GetNormal(p1, p2, p3);
        }

        public void DrawHyperPlan(IncidenceGraph incidenceGraph, DisplayData data)
        {
            var centroid = (p1.position + p2.position + p3.position)/3f;
            var plane = new Plane3D(p1, p2, p3,
                incidenceGraph.GetOtherPoint(p1,p2,p3));
            DrawPlaneAtPoint(plane, centroid, data.planeSize, GetColorFromNodeColor());
        }

        private void DrawPlaneAtPoint(Plane3D plane, Vector3 center, float size, Color c)
        {
            var basis = Quaternion.LookRotation(plane.Normal);
            Debug.DrawLine(center, center + plane.Normal * 1, c);
            var scale = Vector3.one * size / 10f;

            var right = Vector3.Scale(basis * Vector3.right, scale);
            var up = Vector3.Scale(basis * Vector3.up, scale);

            for(int i = -5; i <= 5; i++) {
                Debug.DrawLine(center + right * i - up * 5, center + right * i + up * 5, c);
                Debug.DrawLine(center + up * i - right * 5, center + up * i + right * 5, c);
            }
        }

        public void SetColorFromPoint(IncidenceGraph incidenceGraph, Vector3 point)
        {
            var plane = new Plane3D(p1, p2, p3,
                incidenceGraph.GetOtherPoint(p1,p2,p3));
            color = plane.GetSide(point) ? NodeColor.Blue : NodeColor.Red;
        }

        public void DrawOrder(DisplayData data)
        {
            DrawEdge(p1, p2, data);
            DrawEdge(p2, p3, data);
            DrawEdge(p3, p1, data);
        }

        private void DrawEdge(Vertex3D a, Vertex3D b, DisplayData data)
        {
            FromTo(a,b, out var from, out var to, data);
            Drawing.Draw.Arrow(from, to, -Vector3.forward, data.arrowSize, PGColors.Redish);
        }

        private void FromTo(Vertex3D a, Vertex3D b, out Vector3 to, out Vector3 @from, DisplayData Data)
        {
            from = a.position;
            to = b.position;
            var dir = (to - @from).normalized;
            var right = Vector3.Cross(dir, -Vector3.forward);
            @from += dir * (Data.pointSize * Data.arrowStretchFactor);
            to -= dir * (Data.pointSize * Data.arrowStretchFactor);
            @from += right * Data.arrowOffset;
            to += right * Data.arrowOffset;
        }

        public bool HasEdge(Vertex3D v1, Vertex3D v2)
        {
            return NodeEquals(p1, p2, v1, v2)
                   || NodeEquals(p2, p3, v1, v2)
                   || NodeEquals(p3, p1, v1, v2);
        }
        
        private Face3D[] GetAdjacentFaces()
        {
            var edges = GetEdges();
            var faces = new Face3D[3];
            for (int i = 0; i < 3; i++)
            {
                faces[i] = GetOtherFace(edges[i]);
            }
            return faces;
        }

        private Face3D GetOtherFace(Edge3D edge)
        {
            return edge.face1.Equals(this) ? edge.face2 : edge.face1;
        }

        private List<Edge3D> GetEdges()
        {
            var allEdges = _convexHull.edges;
            var edges = allEdges.Where(edge => HasEdge(edge.p1, edge.p2)).ToList();

            return edges;
        }

        
    }
}