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

            p1.AddEdge(this);
            p2.AddEdge(this);
        }
    }
}