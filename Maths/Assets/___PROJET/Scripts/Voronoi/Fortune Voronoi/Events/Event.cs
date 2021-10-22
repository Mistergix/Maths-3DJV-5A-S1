using System;
using ESGI.Voronoi.Fortune;

namespace ESGI.Common
{
    public abstract class Event : IComparable<Event>
    {
        public abstract float Priority { get; }
        public int CompareTo(Event other)
        {
            if (other.Priority > Priority)
            {
                return 1;
            }

            if (other.Priority < Priority)
            {
                return -1;
            }

            return 0;
        }

        public abstract void HandleEvent(VoronoiFortune voronoiFortune);
    }
}