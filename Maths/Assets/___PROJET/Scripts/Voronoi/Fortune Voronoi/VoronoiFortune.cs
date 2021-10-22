using System;
using System.Collections.Generic;
using ___PROJET.Scripts.Voronoi.Fortune_Voronoi;
using ESGI.Common;
using ESGI.Voronoi.Fortune_Voronoi;
using Sirenix.OdinInspector;
using UnityEngine;
using Event = ESGI.Common.Event;

namespace ESGI.Voronoi.Fortune
{
    public class VoronoiFortune : MonoBehaviour
    {
        [SerializeField] private Points points;
        protected List<Vector2> Positions => points.positions;

        public DisplayData Data => displayData;

        public PriorityQueue<Event> Queue => _queue;
        private BeachLine _beachLine;

        public DCEL Dcel => _dcel;

        private DCEL _dcel;

        [SerializeField] private DisplayData displayData;
        
        private PriorityQueue<Event> _queue;

        [Button]
        private void ComputeVoronoi()
        {
            _queue = new PriorityQueue<Event>();
            _dcel = new DCEL();
            _beachLine = new BeachLine();
            foreach (var point in Positions)
            {
                var siteEvent = new SiteEvent(point);
                Queue.Enqueue(siteEvent);
            }

            while (!Queue.Empty())
            {
                var e = Queue.Dequeue();
                e.HandleEvent(this);
            }
        }
        
        public void InsertInBeachLine(Vector2 site)
        {
            var node = _beachLine.GetArcAboveSite(this, site);
            if (node == null)
            {
                _beachLine.AddRootNode(site);
                return;
            }
            node.CleanQueue(_queue);
            var leftNode = new VoronoiNode(node.Site);
            var middleNode = new VoronoiNode(site);
            var rightNode = new VoronoiNode(node.Site);
            var connectionNode = new VoronoiNode(Vector2.zero);

            node.Node.Left = leftNode;
            node.Node.Right = connectionNode;
            connectionNode.LeftNode = middleNode;
            connectionNode.RightNode = rightNode;
        }
    }
}