using System;
using System.Collections.Generic;
using ESGI.Common;
using ESGI.Structures;
using PGSauce.Core.PGDebugging;
using Shapes;
using Sirenix.OdinInspector;
using UnityEngine;
using Event = ESGI.Common.Event;

namespace ESGI.Voronoi.Fortune
{
    public class VoronoiFortune : Drawer2D
    {
        public float lineY;
        public float Width = 10;
        public float Offset = 10;
        
        [SerializeField] private Color lineColor = Color.grey;
        [SerializeField] private float lineThickness = 0.2f;
        
        private PriorityQueue<Event> Queue { get; set; }

        private BeachLine _beachLine;

        private DCEL _dcel;

        private void Awake()
        {
            Init();
        }

        protected override void CustomUpdate()
        {
            
        }

        public override void DrawShapes(Camera cam)
        {
            base.DrawShapes(cam);
            using (Draw.Command(cam))
            {
                foreach (var point in Positions)
                {
                    Draw.Disc(point, Data.pointSize, Data.pointColor);
                }

                var edges = _dcel.Edges;

                foreach (var edge in edges)
                {
                    Draw.Line(edge.Start.position, edge.End.position, lineThickness,lineColor);
                }
            }
        }

        
        

        private void Init()
        {
            Queue = new PriorityQueue<Event>();
            _dcel = new DCEL();
            _beachLine = new BeachLine(this);
        }

        [Button]
        private void ComputeVoronoi()
        {
            Init();
            var sites = CleanSites(Positions);
            foreach (var point in sites)
            {
                var siteEvent = new SiteEvent(point);
                Queue.Enqueue(siteEvent);
                _dcel.AddNewCell(new VoronoiCell());
            }

            while (!Queue.Empty())
            {
                var e = Queue.Dequeue();
                e.HandleEvent(this);
            }

            FinishEdge();
            _dcel.UpdateTwins();
        }

        private void FinishEdge()
        {
            _beachLine.FinishEdge();
        }

        private List<Vector2> CleanSites(List<Vector2> points)
        {
            PGDebug.Message("Remove Points To Close").LogTodo();
            return points;
        }
        
        public void InsertInBeachLine(Vector2 site)
        {
            var nodeData = _beachLine.GetArcAboveSite(site);
            
            if (nodeData == null)
            {
                _beachLine.CreateRoot(site);
                return;
            }

            if (nodeData.Node.IsLeaf && nodeData.Site.y - site.y < 0.01f)
            {
                PGDebug.Message("Cas dégénéré").LogTodo();
            }

            nodeData.CleanQueue(Queue);

            var start = new Vector2(site.x, nodeData.Arc.Compute(site.x, lineY));
            var edgeLeft = new VoronoiEdge(start, nodeData.Site, site);
            var edgeRight = new VoronoiEdge(start, site, nodeData.Site);

            edgeLeft.Twin = edgeRight;

            _dcel.AddEdge(edgeLeft);

            nodeData.Edge = edgeRight;

            
            var leftNode = new VoronoiNode(nodeData.Site);
            var middleNode = new VoronoiNode(site);
            var rightNode = new VoronoiNode(nodeData.Site);

            var connectionNode = new VoronoiNode(Vector2.zero);

            nodeData.Node.RightNode = rightNode;
            nodeData.Node.LeftNode = connectionNode;
            nodeData.Node.LeftNode.Data.Edge = edgeLeft;
            
            connectionNode.LeftNode = leftNode;
            connectionNode.RightNode = middleNode;

            CheckCircle(leftNode);
            CheckCircle(rightNode);
        }

        private void CheckCircle(VoronoiNode node){
            var leftParent = _beachLine.GetLeftParent(node);
            var rightParent = _beachLine.GetRightParent(node);

            var a = _beachLine.GetLeftChild(leftParent);
            var c = _beachLine.GetRightChild(rightParent);
            
            if(a == null || c == null || a.Data.Site.Equals(c.Data.Site)){return;}

            var intersection = GetIntersection(leftParent.Data.Edge, rightParent.Data.Edge);
            if(intersection == null){return;}

            var dist = Vector2.Distance(a.Data.Site, intersection.position);
            var distanceToIntersection = intersection.y - dist;
            if(distanceToIntersection >= lineY) {return;}

            var pos = new Vector2(intersection.x, distanceToIntersection);
            var e = new CircleEvent(new Vertex(pos));
            node.Data.Arc.circleEvent = e;
            e.Arch = node;
            Queue.Enqueue(e);
        }

        private Vertex GetIntersection(VoronoiEdge a, VoronoiEdge b)
        {
            var inter = GetLineIntersection(a.Start, a.Right, b.Start, b.Right);
            if (inter == null)
            {
                return null;
            }
            var wrongDirection = IsWrongDirection(a, b, inter);
            if (wrongDirection)
            {
                return null;
            }

            return inter;
        }

        private bool IsWrongDirection(VoronoiEdge a, VoronoiEdge b, Vertex I)
        {
            var A = (I.x - a.Start.x) * a.Direction.x < 0;
            var B = (I.y - a.Start.y) * a.Direction.y < 0;
            var C = (I.x - b.Start.x) * b.Direction.x < 0;
            var D = (I.y - b.Start.y) * b.Direction.y < 0;

            return A || B || C || D;
        }

        private Vertex GetLineIntersection(Vertex a1, Vertex a2, Vertex b1, Vertex b2)
        {
            var dax = (a1.x-a2.x);
            var dbx = (b1.x-b2.x);
            var day = (a1.y-a2.y);
            var dby = (b1.y-b2.y);

            var den = dax*dby - day*dbx;
            if (den == 0) return null;	// parallel

            var a = (a1.x * a2.y - a1.y * a2.x);
            var b = (b1.x * b2.y - b1.y * b2.x);
		
            var I = new Vector2(( a*dbx - dax*b ) / den,( a*dby - day*b ) / den);
            return new Vertex(I);
        }
    }
}