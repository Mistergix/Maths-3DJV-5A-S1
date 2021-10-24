using ESGI.Structures;
using ESGI.Voronoi.Fortune;
using UnityEngine;

namespace ESGI.Common
{
    public class SiteEvent : Event
    {
        private Vertex _site;
        public SiteEvent(Vertex point)
        {
            _site = point;
        }

        public override float Priority => _site.y;

        protected override void CustomHandleEvent(VoronoiFortune voronoiFortune)
        {
            voronoiFortune.InsertInBeachLine(_site);
        }

        public override float GetLineY => _site.y;
    }
}