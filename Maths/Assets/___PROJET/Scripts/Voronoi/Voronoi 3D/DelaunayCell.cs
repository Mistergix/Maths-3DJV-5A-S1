using ESGI.ConvexHull3D;
using UnityEngine;

namespace ESGI.Voronoi.Voronoi3D
{
    public class DelaunayCell
    {
        public Face3D Face { get; }
		
        public Vertex3D Center { get; }
		

        public DelaunayCell(Face3D face, Vector3 center)
        {
            Face = face;

            Center = new Vertex3D(center);
        }
    }
}