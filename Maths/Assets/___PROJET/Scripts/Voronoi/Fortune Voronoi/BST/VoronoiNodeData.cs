
using ESGI.Structures;
using UnityEngine;
using Event = ESGI.Common.Event;

namespace ESGI.Voronoi.Fortune
{
    public class VoronoiNodeData
    {
        public VoronoiNode Node { get; set; }
        private Vertex _site;
        private Arc _arc;

        public VoronoiEdge Edge {get;set;}
        public VoronoiNodeData(Vertex site)
        {
            _site = site;
            _arc = new Arc(site);
        }

        public Vertex Site => _site;
        public Arc Arc => _arc;
        public void CleanQueue(PriorityQueue<Event> queue)
        {
           _arc.CleanQueue(queue);
        }
    }
}