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
            var plane = new Plane3D(a, b, c, convexHull.GetOtherPoint(a, b, c));
            p1 = a;
            var center = (a.position + b.position + c.position) / 3f;
            var p1ToCenter = (center - p1.position).normalized;
            
            var up = plane.Normal;
            var forward = p1ToCenter;

            var perp = Vector3.Cross(forward, (b.position - p1.position).normalized);
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
    }
}