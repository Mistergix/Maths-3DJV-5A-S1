using System.Collections.Generic;
using ESGI.Voronoi.Fortune;

namespace ESGI.Voronoi.Voronoi3D
{
    public class VoronoiRegion
    {
        public int Id { get; set; }

        public List<DelaunayCell> Cells { get; }

        public List<VoronoiEdge3D> Edges { get; }

        public VoronoiRegion()
        {

            Cells = new List<DelaunayCell>();

            Edges = new List<VoronoiEdge3D>();

        }

        public override string ToString()
        {
            return $"[VoronoiRegion: Id={Id}, Cells={Cells.Count}, Edges={Edges.Count}]";
        }
    }
}