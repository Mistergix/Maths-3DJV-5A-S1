namespace ESGI.ConvexHull3D
{
    public class Edge3D : Node
    {
        public Vertex3D p1, p2;
        public Face3D face1, face2;

        public Edge3D(Vertex3D a, Vertex3D b)
        {
            p1 = a;
            p2 = b;
        }

        public bool EqualsTwoVertices(Vertex3D begin, Vertex3D end)
        {
            return NodeEquals(p1, p2, begin, end);
        }
    }
}