
using UnityEngine;
using Event = ESGI.Common.Event;

namespace ESGI.Voronoi.Fortune
{
    public class VoronoiNodeData
    {
        public VoronoiNode Node { get; set; }
        private Vector2 _site;
        private Arc _arc;

        public VoronoiEdge Edge {get;set;}
        public VoronoiNodeData(Vector2 site)
        {
            _site = site;
            _arc = new Arc(site);
        }

        public Vector2 Site => _site;
        public Arc Arc => _arc;
        public void CleanQueue(PriorityQueue<Event> queue)
        {
           _arc.CleanQueue(queue);
        }
    }
}