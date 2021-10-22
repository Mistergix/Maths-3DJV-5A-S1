using System.Collections.Generic;
using Drawing;
using ESGI.Common;
using ESGI.Structures;
using PGSauce.Core;
using PGSauce.Core.PGDebugging;
using UnityEngine;

namespace ESGI.Triangulation
{
    public class Delaunay2DFromPoints : TriangulationBase
    {
        [SerializeField, Min(1)] private int maxIteration = 10000;
        [SerializeField] private bool drawEdges = true;
        [SerializeField] private bool drawCircles = true;
        private List<HalfEdge> _halfEdges;

        public override Color LineColor => Color.black;

        protected override List<Triangle> Triangulate(List<Vertex> vectors)
        {
            var triangles = Geom.IncrementalTriangulate2D(vectors);
            _halfEdges = triangles.ToHalfEdges();
            var iteration = 1;
            var hasFlipped = true;
            //PGDebug.Message("Refactoriser le delaunay").LogWarning();
            while (iteration <= maxIteration && _halfEdges.Count > 0 && hasFlipped)
            {
                hasFlipped = false;
                for (var i = 0; i < _halfEdges.Count; i++)
                {
                    var thisEdge = _halfEdges[i];
                    var twinEdge = thisEdge.twinEdge;
                    if (twinEdge == null)
                    {
                        continue;
                    }

                    //PGDebug.Message("Refactor a bit").LogTodo();
                    var a = thisEdge.targetVertex;
                    var b = thisEdge.nextEdge.targetVertex;
                    var c = thisEdge.previousEdge.targetVertex;
                    var d = twinEdge.nextEdge.targetVertex;

                    if (NeedAFlip(a, b, c, d, drawCircles))
                    {
                        if (Geom.IsQuadConvex(a.position, b.position, c.position, d.position))
                        {
                            if (NeedAFlip(b, c, d, a, drawCircles))
                            {
                                continue;
                            }

                            hasFlipped = true;
                            thisEdge.Flip();
                        }
                    }
                }

                iteration++;
            }

            DrawHalfEdges();

            return triangles;
        }

        private void DrawHalfEdges()
        {
            if (!drawEdges)
            {
                return;
            }
            var drawnEdges = new HashSet<HalfEdge>();

            var max = 100;

            foreach (var halfEdge in _halfEdges)
            {
                var i = 0;
                var next = halfEdge.nextEdge;
                DrawHalfEdge(halfEdge, drawnEdges);
                while (!next.Equals(halfEdge) && i < max)
                {
                    DrawHalfEdge(next, drawnEdges);
                    DrawNextEdgeConnection(halfEdge, next);
                    i++;
                }
            }
        }

        private void DrawNextEdgeConnection(HalfEdge halfEdge, HalfEdge next)
        {
            FromTo(halfEdge, out _, out var to);
            FromTo(next, out var from, out _);
            Draw.Line(from, to, PGColors.Blueish);
        }
        
        private void DrawTwinEdgeConnection(HalfEdge halfEdge)
        {
            var twin = halfEdge.twinEdge;
            if (twin == null)
            {
                return;
            }
            
            FromTo(halfEdge, out var from, out var to);
            FromTo(twin, out var fromT, out var toT);

            var mid1 = (from + to) / 2;
            var mid2 = (fromT + toT) / 2;
            
            Draw.Line(mid1, mid2, PGColors.Greenish);
        }

        private void DrawHalfEdge(HalfEdge halfEdge, HashSet<HalfEdge> drawnEdges)
        {
            if(drawnEdges.Contains(halfEdge)) {return;}
            FromTo(halfEdge, out var from, out var to);
            Draw.Arrow(from, to, -Vector3.forward, Data.arrowSize, PGColors.Redish);
            drawnEdges.Add(halfEdge);
            DrawTwinEdgeConnection(halfEdge);
        }

        private void FromTo(HalfEdge halfEdge, out Vector3 from, out Vector3 to)
        {
            from = (Vector3) halfEdge.FromVertex.position;
            to = (Vector3) halfEdge.targetVertex.position;
            var dir = (to - @from).normalized;
            var right = Vector3.Cross(dir, -Vector3.forward);
            @from += dir * (Data.pointSize * Data.arrowStretchFactor);
            to -= dir * (Data.pointSize * Data.arrowStretchFactor);
            @from += right * Data.arrowOffset;
            to += right * Data.arrowOffset;
        }

        private static bool NeedAFlip(Vertex a, Vertex b, Vertex c, Vertex d, bool drawCircle)
        {
            var circle = new CircleFromPoints(a.position, b.position, c.position);
            DrawCircle(drawCircle, circle);
            return Geom.PointRelativeToCircle(circle, d.position) > 0f;
        }

        private static void DrawCircle(bool drawCircle, CircleFromPoints circle)
        {
            if (drawCircle)
            {
                var p1 = circle.p1;
                var p2 = circle.p2;
                var p3 = circle.p3;

                // triangle "edges"
                var t = p2 - p1;
                var u = p3 - p1;
                var v = p3 - p2;

                // triangle normal
                var w = Vector3.Cross(t, u);
                var wsl = Vector3.Dot(w, w);
                // TODO: if (wsl<10e-14) return false; // area of the triangle is too small (you may additionally check the points for colinearity if you are paranoid)

                // helpers
                var iwsl2 = 1f / (2f * wsl);
                var tt = Vector3.Dot(t, t);
                var uu = Vector3.Dot(u, u);

                // result circle
                Vector3 circCenter = p1 + (u * (tt * (Vector3.Dot(u, v))) - t * (uu * (Vector3.Dot(t, v)))) * iwsl2;
                var circRadius = Mathf.Sqrt(tt * uu * (Vector3.Dot(v, v)) * iwsl2 * 0.5f);
                Vector3 circAxis = w / Mathf.Sqrt(wsl);
                
                Draw.CircleXY(circCenter, circRadius, PGColors.Purple);
            }
        }
    }
}