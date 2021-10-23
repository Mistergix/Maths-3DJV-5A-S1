using ___PROJET.Scripts.Voronoi.Fortune_Voronoi.BST;
using ESGI.Voronoi.Fortune_Voronoi;
using PGSauce.Core.PGDebugging;
using UnityEngine;
using Event = ESGI.Common.Event;

namespace ___PROJET.Scripts.Voronoi.Fortune_Voronoi
{
    public class VoronoiNodeData : NodeData
    {
        private Vector2 _site;
        private Arc _arc;
        public VoronoiNodeData(Vector2 site)
        {
            _site = site;
            _arc = new Arc(site);
        }

        public Vector2 Site => _site;
        public Arc Arc => _arc;
        public override bool Smaller(NodeData other)
        {
            if (Node.IsLeaf)
            {
                return ((VoronoiNodeData) other).Site.x > Site.x;
            }
        }

        public override bool Greater(NodeData other)
        {
            return ((VoronoiNodeData) other).Site.x < Site.x;
        }

        public void CleanQueue(PriorityQueue<Event> queue)
        {
           _arc.CleanQueue(queue);
        }
    }
}