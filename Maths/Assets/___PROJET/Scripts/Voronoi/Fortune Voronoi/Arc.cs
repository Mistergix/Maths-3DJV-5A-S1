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
        public Parabola parabola;

        public Arc (Vector2 site){
            this.site = site;
            parabola = new Parabola();
        }

        public void CleanQueue(PriorityQueue<Event> queue)
        {
            if(circleEvent == null) {return;}
            queue.Data.Remove(circleEvent);
            circleEvent = null;
        }

        public void UpdateDirectrix(float lineY){
            parabola.ComputeParabolaFromFocusAndHorizontalLine(site, lineY);
        }

        public float Compute(float x){
            return parabola.Compute(x);
        }
    }
}