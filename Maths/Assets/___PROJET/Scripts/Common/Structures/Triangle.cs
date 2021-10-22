using PGSauce.Core.Utilities;
using UnityEngine;

namespace ESGI.Structures
{
    public class Triangle
    {
        public Vertex p1, p2, p3;
        /// <summary>
        /// One of the three edges
        /// </summary>
        public HalfEdge halfEdge;

        public Triangle(Vertex p1, Vertex p2, Vertex p3)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }
        
        public Triangle(Vector2 v1, Vector2 v2, Vector2 v3)
        {
            p1 = new Vertex(v1);
            p2 = new Vertex(v2);
            p3 = new Vertex(v3);
        }
        
        public Triangle(HalfEdge halfEdge)
        {
            this.halfEdge = halfEdge;
        }

        /// <summary>
        /// From cw to ccw and vice versa
        /// </summary>
        public void SwitchOrientation()
        {
            var tmp = p1;
            p1 = p2;
            p2 = tmp;
        }

        public void SetLocal(Transform transform)
        {
            p1.position = transform.GetWorldPosition(p1.position);
            p2.position = transform.GetWorldPosition(p2.position);
            p3.position = transform.GetWorldPosition(p3.position);
        }
    }
}