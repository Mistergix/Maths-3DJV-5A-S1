using ESGI.Common;
using PGSauce.Core;
using PGSauce.Core.PGDebugging;
using UnityEngine;

namespace ESGI.ConvexHull3D
{
    public class Face3D : Node
    {
        public Vertex3D p1, p2, p3;
        private readonly Plane _plane;

        public Face3D(Vertex3D p1, Vertex3D p2, Vertex3D p3)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            PGDebug.Message("Create a class for plane").LogTodo();
            _plane = new Plane(p1.position, p2.position, p3.position);
        }

        public void DrawHyperPlan(DisplayData data)
        {
            var centroid = (p1.position + p2.position + p3.position)/3f;
            DrawPlaneAtPoint(_plane, centroid, data.planeSize, GetColorFromNodeColor());
        }

        private void DrawPlaneAtPoint(Plane plane, Vector3 center, float size, Color c)
        {
            var basis = Quaternion.LookRotation(plane.normal);
            var scale = Vector3.one * size / 10f;

            var right = Vector3.Scale(basis * Vector3.right, scale);
            var up = Vector3.Scale(basis * Vector3.up, scale);

            for(int i = -5; i <= 5; i++) {
                Debug.DrawLine(center + right * i - up * 5, center + right * i + up * 5, c);
                Debug.DrawLine(center + up * i - right * 5, center + up * i + right * 5, c);
            }
        }

        public void SetColorFromPoint(Vector3 point)
        {
            color = _plane.GetSide(point) ? NodeColor.Blue : NodeColor.Red;
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
    }
}