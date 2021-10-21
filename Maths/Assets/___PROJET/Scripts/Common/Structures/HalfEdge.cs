namespace ESGI.Structures
{
    public class HalfEdge
    {
        public Vertex targetVertex;
        /// <summary>
        /// The triangle this edge belongs to
        /// </summary>
        public Triangle triangle;

        public HalfEdge previousEdge;
        public HalfEdge nextEdge;
        public HalfEdge twinEdge;
        public Vertex FromVertex => previousEdge.targetVertex;

        public HalfEdge(Vertex target)
        {
            targetVertex = target;
        }

        public void SetEdges(HalfEdge prev, HalfEdge next, Triangle t)
        {
            previousEdge = prev;
            nextEdge = next;
            targetVertex.halfEdge = next;
            triangle = t;
        }
    }
}