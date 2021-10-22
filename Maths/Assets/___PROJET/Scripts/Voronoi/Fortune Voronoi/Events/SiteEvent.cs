using ESGI.Voronoi.Fortune;
using UnityEngine;

namespace ESGI.Common
{
    public class SiteEvent : Event
    {
        private Vector2 _site;
        public SiteEvent(Vector2 point)
        {
            _site = point;
        }

        public override float Priority => _site.y;
        public override void HandleEvent(VoronoiFortune voronoiFortune)
        {
            voronoiFortune.InsertInBeachLine(_site);
        }
    }
}