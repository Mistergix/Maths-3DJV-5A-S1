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

        public HalfEdge(Vertex target)
        {
            targetVertex = target;
        }
    }
}