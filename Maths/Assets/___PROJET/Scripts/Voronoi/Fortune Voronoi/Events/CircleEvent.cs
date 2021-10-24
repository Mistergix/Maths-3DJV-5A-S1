using ESGI.Structures;
using ESGI.Voronoi;
using ESGI.Voronoi.Fortune;

namespace ESGI.Common
{
    public class CircleEvent : Event
    {
        private Vertex _vertex;
        public CircleEvent(Vertex vertex)
        {
            _vertex = vertex;
        }

        public override float Priority => _vertex.y;
        public VoronoiNode Arch { get; set; }

        protected override void CustomHandleEvent(VoronoiFortune voronoiFortune)
        {
            throw new System.NotImplementedException();
        }

        public override float GetLineY => _vertex.y;
    }
}