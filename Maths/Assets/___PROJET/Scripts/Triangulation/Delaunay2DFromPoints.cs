using System.Collections.Generic;
using ESGI.Common;
using ESGI.Structures;
using PGSauce.Core.PGDebugging;
using UnityEngine;

namespace ESGI.Triangulation
{
    public class Delaunay2DFromPoints : TriangulationBase
    {
        [SerializeField, Min(1)] private int maxIteration = 10000;
        public override Color LineColor => Color.black;

        protected override List<Triangle> Triangulate(List<Vertex> vectors)
        {
            var triangles = Geom.IncrementalTriangulate2D(vectors);
            var halfEdges = triangles.ToHalfEdges();
            var iteration = 1;
            var hasFlipped = true;
            //PGDebug.Message("Refactoriser le delaunay").LogWarning();
            while (iteration <= maxIteration && halfEdges.Count > 0 && hasFlipped)
            {
                hasFlipped = false;
                for (var i = 0; i < halfEdges.Count; i++)
                {
                    var thisEdge = halfEdges[i];
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

                    if (NeedAFlip(a, b, c, d))
                    {
                        if (Geom.IsQuadConvex(a.position, b.position, c.position, d.position))
                        {
                            if (NeedAFlip(b, c, d, a))
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

            return triangles;
        }

        private static bool NeedAFlip(Vertex a, Vertex b, Vertex c, Vertex d)
        {
            return Geom.PointRelativeToCircle(new CircleFromPoints(a.position, b.position, c.position),
                d.position) < 0f;
        }
    }
}