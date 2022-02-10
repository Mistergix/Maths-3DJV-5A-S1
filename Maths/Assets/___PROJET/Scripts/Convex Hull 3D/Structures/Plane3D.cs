using UnityEngine;

namespace ESGI.ConvexHull3D
{
    public class Plane3D
    {
        private float _a, _b, _c, _d;
        private readonly Vector3 _normal;
        private readonly Vertex3D _pointOnPlane;

        public Plane3D(Vertex3D a, Vertex3D b, Vertex3D c, Vertex3D directorPoint)
        {
            var ab = b.position - a.position;
            var ac = c.position - a.position;
            var n1 = Vector3.Cross(ab, ac).normalized;
            var n2 = Vector3.Cross(ac, ab).normalized;
            var dirToDirectorPoint = directorPoint.position - a.position;
            Vector3 normal;
            if (Vector3.Dot(n1, dirToDirectorPoint) >= 0)
            {
                normal = n2;
            }
            else
            {
                normal = n1;
            }

            _a = normal.x;
            _b = normal.y;
            _c = normal.z;

            _d = -_a * a.position.x - _b * a.position.y - _c * a.position.z;

            _normal = normal;
            _pointOnPlane = a;
        }

        public Vector3 Normal => _normal;

        public bool GetSide(Vector3 point)
        {
            var dirToPoint = point - _pointOnPlane.position;
            dirToPoint.Normalize();
            return Vector3.Dot(dirToPoint, _normal) >= 0;
        }
    }
}