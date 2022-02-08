namespace ESGI.ConvexHull3D
{
    public class Face3D : Node
    {
        public Edge3D edge1;
        public Edge3D edge2;
        public Edge3D edge3;

        public Face3D(Edge3D edge1, Edge3D edge2, Edge3D edge3)
        {
            this.edge1 = edge1;
            this.edge2 = edge2;
            this.edge3 = edge3;
        }
    }
}