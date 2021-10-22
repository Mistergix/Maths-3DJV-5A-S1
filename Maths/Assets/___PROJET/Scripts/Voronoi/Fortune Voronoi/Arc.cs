using ESGI.Common;
using ESGI.Voronoi.Fortune_Voronoi;
using UnityEngine;
using Event = ESGI.Common.Event;

namespace ESGI.Voronoi.Fortune
{
    public class Arc
    {
        public Vector2 site;
        public CircleEvent circleEvent;

        public void CleanQueue(PriorityQueue<Event> queue)
        {
            queue.Data.Remove(circleEvent);
        }
    }
}