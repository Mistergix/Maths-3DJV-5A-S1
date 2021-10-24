using ESGI.Structures;
using ESGI.Voronoi;
using ESGI.Voronoi.Fortune;

namespace ESGI.Common
{
    public class CircleEvent : Event
    {
        private readonly Vertex _vertex;
        private VoronoiNode _node;
        public CircleEvent(Vertex vertex, VoronoiNode node)
        {
            _vertex = vertex;
            _node = node;
        }

        public override float Priority => Vertex.y;
        public VoronoiNode Arch { get; set; }

        protected override void CustomHandleEvent(VoronoiFortune voronoiFortune)
        {
            voronoiFortune.RemoveFromBeachLine(_node);
        }

        public override float GetLineY => Vertex.y;

        public Vertex Vertex => _vertex;
    }
}