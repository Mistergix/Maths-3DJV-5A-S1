namespace ESGI.Voronoi.Fortune
{
    public class DCEL
    {
        private List<VoronoiCell> cells;
        private List<VoronoiEdges> edges;

        new DCEL(){
            cells = new List<VoronoiCell>();
            edges = new List<VoronoiEdges>();
        }
    }
}