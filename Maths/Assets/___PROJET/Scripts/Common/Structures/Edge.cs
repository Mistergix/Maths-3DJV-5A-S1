

using UnityEngine;

namespace ESGI.Structures
{
    public class Edge
    {
        public Vertex p1;
        public Vertex p2;

        public Edge(Vertex p1, Vertex p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }
        
        public Edge(Vector2 v1, Vector2 v2)
        {
            p1 = new Vertex(v1);
            p2 = new Vertex(v2);
        }

        public void FlipEdge()
        {
            var tmp = p1;
            p1 = p2;
            p2 = tmp;
        }
    }
}