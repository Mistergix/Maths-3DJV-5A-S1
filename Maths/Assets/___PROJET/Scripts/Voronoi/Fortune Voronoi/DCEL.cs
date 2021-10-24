using System.Collections.Generic;
using System.Linq;

namespace ESGI.Voronoi.Fortune
{
    public class DCEL
    {
        private List<VoronoiCell> _cells;
        private List<VoronoiEdge> _edges;

        public  DCEL(){
            _cells = new List<VoronoiCell>();
            _edges = new List<VoronoiEdge>();
        }

        public List<VoronoiEdge> Edges => _edges;

        public void AddNewCell(VoronoiCell voronoiCell)
        {
            _cells.Add(voronoiCell);
        }

        public void AddEdge(VoronoiEdge edge)
        {
            Edges.Add(edge);
        }

        public void UpdateTwins()
        {
            var twinEdges = Edges.Where(edge => edge.Twin != null);
            foreach (var edge in twinEdges)
            {
                edge.Start = edge.Twin.End;
            }
        }
    }
}