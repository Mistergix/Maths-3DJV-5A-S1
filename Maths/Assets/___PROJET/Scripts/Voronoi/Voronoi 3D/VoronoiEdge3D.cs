using System;

namespace ESGI.Voronoi.Voronoi3D
{
    public class VoronoiEdge3D : IEquatable<VoronoiEdge3D>
    {
        public VoronoiEdge3D(DelaunayCell from, DelaunayCell to)
        {
            From = from;
            To = to;
        }

        public DelaunayCell From { get; }
        public DelaunayCell To { get; }
        
        public static bool operator ==(VoronoiEdge3D k1, VoronoiEdge3D k2)
        {
            if (ReferenceEquals(k1, k2))
            {
                return true;
            }
			
            if ((object)k1 == null || (object)k2 == null)
            {
                return false;
            }

            return ReferenceEquals(k1.From, k2.To);
        }
        
        public static bool operator !=(VoronoiEdge3D k1, VoronoiEdge3D k2)
        {
            return !(k1 == k2);
        }
		
        public override bool Equals(object o)
        {
            var k = o as VoronoiEdge3D;
            return k != null && k == this;
        }
        
        public bool Equals(VoronoiEdge3D k)
        {
            return k == this;
        }

        public override int GetHashCode()
        {
            var hashcode = 23;

            hashcode = hashcode * 37 + From.GetHashCode();
            hashcode = hashcode * 37 + To.GetHashCode();
			
            return hashcode;
        }
    }
}