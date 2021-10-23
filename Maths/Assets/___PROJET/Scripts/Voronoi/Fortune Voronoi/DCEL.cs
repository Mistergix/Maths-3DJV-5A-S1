namespace ESGI.Voronoi.Fortune
{
    public class DCEL
    {
        private List<VoronoiCell> cells;
        private List<VoronoiEdge> edges;

        new DCEL(){
            cells = new List<VoronoiCell>();
            edges = new List<VoronoiEdge>();
        }
    }
}