using System;
using System.Collections.Generic;
using System.Linq;
using ESGI.Common;
using ESGI.Structures;
using PGSauce.Core;
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Utilities;
using Shapes;
using Sirenix.OdinInspector;
using UnityEngine;
using Event = ESGI.Common.Event;

namespace ESGI.Voronoi.Fortune
{
    public class VoronoiFortune : Drawer2D
    {
        public float lineY { get; set; }
        public float Width = 10;
        public float Offset = 10;

        public float debugLineY;
        
        [SerializeField] private Color lineColor = Color.grey;
        [SerializeField] private float lineThickness = 0.2f;
        [SerializeField] private float sweepLineThickness = 0.3f;
        [SerializeField] private Color sweepLineColor;
        [SerializeField, Min(2)] private int parabolaDrawQuality = 20;
        [SerializeField] private Color parabolaColorStart = PGColors.Redish;
        [SerializeField] private Color parabolaColorEnd = PGColors.Blueish;
        [SerializeField] private float parabolaWidth = 0.3f;
        [SerializeField] private bool pointAtMouse;

        [SerializeField] private bool drawParabolas;
        
        private PriorityQueue<Event> Queue { get; set; }

        private BeachLine _beachLine;

        private DCEL _dcel;

        private bool _animating;

        private Camera cam;
        private Vector2 mousePoint;

        private void Awake()
        {
            cam = Camera.main;
            Init();
        }

        protected override void CustomUpdate()
        {
            var mousePos = Input.mousePosition;
            mousePos.z = 0;
            mousePoint = cam.ScreenToWorldPoint(mousePos);
            Compute();
        }

        private void Compute()
        {
            Init();
            var sites = CleanSites(Positions).ToList();
            if (pointAtMouse)
            {
                sites.Add(new Vertex(mousePoint));
            }
            foreach (var point in sites)
            {
                point.cell = new VoronoiCell();
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

        public override void DrawShapes(Camera cam)
        {
            base.DrawShapes(cam);
            using (Draw.Command(cam))
            {
                var points = new List<Vector2>(Positions);
                if (pointAtMouse)
                {
                    points.Add(mousePoint);
                }

                for (var index = 0; index < points.Count; index++)
                {
                    var point = points[index];
                    var t = index.Remap(0, points.Count - 1, 0, 1);
                    var color = Color.Lerp(parabolaColorStart, parabolaColorEnd, t);
                    Draw.Disc(point, Data.pointSize, color);


                    if (drawParabolas && point.y > debugLineY)
                    {
                        DrawParabola(point, color);
                    }
                }

                var edges = _dcel.Edges;

                foreach (var edge in edges)
                {
                    Draw.Line(edge.Start.position, edge.End.position, lineThickness,lineColor);
                }
                
               //Draw.Line(new Vector3(-Offset, debugLineY), new Vector3(Offset, debugLineY), sweepLineThickness,sweepLineColor);
            }
        }

        private void DrawParabola(Vector2 point, Color color)
        {
            var parabola = new Parabola();
            parabola.ComputeParabolaFromFocusAndHorizontalLine(new Vertex(point), debugLineY);
            var (a,b,c) = parabola.GetCoeffs();
            var step = (Offset - - Offset) / (parabolaDrawQuality - 1);
            for (var i = 0; i < parabolaDrawQuality - 1; i++)
            { 
                var x1 = -Offset + step * i; 
                var y1 = a * x1 * x1 + b * x1 + c;

                var x2 = x1 + step;
                var y2 = a * x2 * x2+ b * x2 + c;
                Draw.Line(new Vector3(x1, y1), new Vector3(x2, y2), parabolaWidth, color);
            }
        }

        private void Init()
        {
            Queue = new PriorityQueue<Event>();
            _dcel = new DCEL();
            _beachLine = new BeachLine(this);
        }

        private void FinishEdge()
        {
            _beachLine.FinishEdge();
        }

        private IEnumerable<Vertex> CleanSites(IEnumerable<Vector2> points)
        {
            PGDebug.Message("Remove Points Too Close").LogTodo();
            return points
                .Select(vector2 => new Vertex(vector2)).ToList();
        }
        
        public void InsertInBeachLine(Vertex site)
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
            var edgeLeft = new VoronoiEdge(new Vertex(start), nodeData.Site, site);
            var edgeRight = new VoronoiEdge(new Vertex(start), site, nodeData.Site);

            edgeLeft.Twin = edgeRight;

            _dcel.AddEdge(edgeLeft);

            nodeData.Edge = edgeRight;

            
            var leftNode = new VoronoiNode(nodeData.Site);
            var middleNode = new VoronoiNode(site);
            var rightNode = new VoronoiNode(nodeData.Site);

            var connectionNode = new VoronoiNode(new Vertex(Vector2.zero));

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

            var dist = Vector2.Distance(a.Data.Site.position, intersection.position);
            var distanceToIntersection = intersection.y - dist;
            if(distanceToIntersection >= lineY) {return;}

            var pos = new Vector2(intersection.x, distanceToIntersection);
            var e = new CircleEvent(new Vertex(pos),node);
            node.Data.Arc.circleEvent = e;
            e.Arch = node;
            Queue.Enqueue(e);
        }

        private Vertex GetIntersection(VoronoiEdge a, VoronoiEdge b)
        {
            var inter = GetLineIntersection(a.Start, a.SecondPoint, b.Start, b.SecondPoint);
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

        public void RemoveFromBeachLine(VoronoiNode node)
        {
            var e = node.Data.Arc.circleEvent;

            var p1 = e.Arch;

            var xl = _beachLine.GetLeftParent(p1);
            var xr = _beachLine.GetRightParent(p1);

            var p0 = _beachLine.GetLeftChild(xl);
            var p2 = _beachLine.GetRightChild(xr);

            p0.Data.Arc.CleanQueue(Queue);
            p2.Data.Arc.CleanQueue(Queue);

            var pos = new Vector2(e.Vertex.x, p1.Data.Arc.Compute(e.Vertex.x, lineY));
            var p = new Vertex(pos);

            if (p0.Data.Site.cell.Equals(p1.Data.Site.cell))
            {
                p1.Data.Site.cell.AddLeft(p);
            }
            else
            {
                p1.Data.Site.cell.AddRight(p);
            }
            
            p0.Data.Site.cell.AddRight(p);
            p2.Data.Site.cell.AddLeft(p);

            xl.Data.Edge.End = p;
            xr.Data.Edge.End = p;

            var higher = new VoronoiNode(new Vertex(Vector2.zero));
            var par = p1;
            while (par != _beachLine.Root)
            {
                par = par.Parent;
                if (par.Equals(xl))
                {
                    higher = xl;
                }

                if (par.Equals(xr))
                {
                    higher = xr;
                }
            }

            higher.Data.Edge = new VoronoiEdge(p, p0.Data.Site, p2.Data.Site);
            _dcel.AddEdge(higher.Data.Edge);

            var gParent = p1.Parent.Parent;

            if (p1.Parent.LeftNode.Equals(p1))
            {
                if(gParent.LeftNode.Equals(p1.Parent))
                {
                    gParent.LeftNode = p1.Parent.RightNode;
                }
                else
                {
                    p1.Parent.Parent.RightNode = p1.Parent.RightNode;
                }
            }
            else
            {
                if(gParent.LeftNode.Equals(p1.Parent))
                {
                    gParent.LeftNode = p1.Parent.LeftNode;
                }
                else
                {
                    gParent.RightNode = p1.Parent.LeftNode;
                }
            }
            
            CheckCircle(p0);
            CheckCircle(p1);
        }
    }
}