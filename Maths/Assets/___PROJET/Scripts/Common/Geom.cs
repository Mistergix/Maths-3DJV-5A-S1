using System.Collections.Generic;
using System.Linq;
using ESGI.Structures;
using PGSauce.Core.PGDebugging;
using UnityEngine;

namespace ESGI.Common
{
    public static class Geom
    {
        public static void Flip(HalfEdge edge)
        {
            PGDebug.Message("Flip pas implémenté").LogWarning();
        }

        public static void AddTriangle(this List<Edge> edges, Triangle triangle)
        {
            edges.Add(new Edge(triangle.p1, triangle.p2));
            edges.Add(new Edge(triangle.p2, triangle.p3));
            edges.Add(new Edge(triangle.p3, triangle.p1));
        }

        public static List<Vertex> ToVertices(this IEnumerable<Vector2> points)
        {
            return points.Select(point => new Vertex(point)).ToList();
        }
        
        public static bool IsQuadConvex(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            PGDebug.Message($"What happens here if 3 of the points are colinear ? Or the quad is hourglass shaped ?").LogTodo();
            var abc = new Triangle(a, b, c).IsClockwise();
            var abd = new Triangle(a, b, d).IsClockwise();
            var bcd = new Triangle(b, c, d).IsClockwise();
            var cad = new Triangle(c, a, d).IsClockwise();
            
            switch (abc)
            {
                case true when abd && bcd & !cad:
                case true when abd && !bcd & cad:
                case true when !abd && bcd & cad:
                case false when !abd && !bcd & cad:
                case false when !abd && bcd & !cad:
                case false when abd && !bcd & !cad:
                    return true;
                default:
                    return false;
            }
        }
        
        public static List<HalfEdge> ToHalfEdges(this List<Triangle> triangles)
        {
            triangles.OrientClockWise();

            var edges =InitEdges(triangles);

            ComputeTwinEdges(edges);

            return edges;
        }

        /// <summary>
        /// Where is the point in relation to the circle ?
        /// </summary>
        /// <param name="circle">The circle</param>
        /// <param name="point">The point we want to test</param>
        /// <returns>1 if inside, -1 if outside, 0 if on the circle</returns>
        public static float PointRelativeToCircle(CircleFromPoints circle, Vector2 point)
        {
            // https://gamedev.stackexchange.com/questions/71328/how-can-i-add-and-subtract-convex-polygons#answer-71704
            var p1 = circle.p1;
            var p2 = circle.p2;
            var p3 = circle.p3;
            
            var a = p1.x - point.x;
            var d = p2.x - point.x;
            var g = p3.x - point.x;

            var b = p1.y - point.y;
            var e = p2.y - point.y;
            var h = p3.y - point.y;

            var c = a * a + b * b;
            var f = d * d + e * e;
            var i = g * g + h * h;

            var det = (a * e * i) + (b * f * g) + (c * d * h) - (g * e * c) - (h * f * a) - (i * d * b);

            return det;
        }

        private static void ComputeTwinEdges(List<HalfEdge> edges)
        {
            foreach (var he in edges)
            {
                if (he.twinEdge != null)
                {
                    continue;
                }

                var from = he.FromVertex;
                var to = he.targetVertex;

                var oppositeEdge = edges.First(edge => @from.position == edge.targetVertex.position &&
                                                       to.position == edge.FromVertex.position);
                he.twinEdge = oppositeEdge;
                oppositeEdge.twinEdge = he;
            }
        }

        private static List<HalfEdge> InitEdges(IEnumerable<Triangle> triangles)
        {
            var edges = new List<HalfEdge>();

            foreach (var triangle in triangles)
            {
                var heA = new HalfEdge(triangle.p1);
                var heB = new HalfEdge(triangle.p2);
                var heC = new HalfEdge(triangle.p3);

                heA.SetEdges(heC, heB, triangle);
                heB.SetEdges(heA, heC, triangle);
                heC.SetEdges(heB, heA, triangle);

                // Could be any of them
                triangle.halfEdge = heA;

                edges.Add(heA);
                edges.Add(heB);
                edges.Add(heC);
            }

            return edges;
        }

        public static void OrientClockWise(this List<Triangle> triangles)
        {
            foreach (var triangle in triangles)
            {
                triangle.OrientClockWise();
            }
        }

        public static void OrientClockWise(this Triangle triangle)
        {
            if (!triangle.IsClockwise())
            {
                triangle.SwitchOrientation();
            }
        }

        // https://math.stackexchange.com/questions/1324179/how-to-tell-if-3-connected-points-are-connected-clockwise-or-counter-clockwise
        public static bool IsClockwise(this Triangle triangle)
        {
            var p1 = triangle.p1.position;
            var p2 = triangle.p2.position;
            var p3 = triangle.p3.position;
            var det = p1.x * p2.y + p3.x * p1.y + p2.x * p3.y - p1.x * p3.y - p3.x * p2.y - p2.x * p1.y;

            return det > 0;
        }
        
        public static bool IsToTheRight(Vector2 point, OrientatedLine orientatedLine)
        {
            var det = Determinant(orientatedLine, point);
            if (det != 0.0f) {return det > 0;}
            return HandleAlignedPoints(point, orientatedLine);
        }
        
        private static bool HandleAlignedPoints(Vector2 point, OrientatedLine orientatedLine)
        {
            return false;
        }

        // https://gamedev.stackexchange.com/questions/71328/how-can-i-add-and-subtract-convex-polygons#answer-71704
        private static float Determinant(OrientatedLine orientatedLine, Vector2 point)
        {
            var A = point;
            var B = orientatedLine.pointA;
            var C = orientatedLine.pointB;
            var AC = C - A;
            var AB = B - A;
            var det = AC.x * AB.y - AC.y * AB.x;
            return det;
        }

        public static bool Intersecting(Edge edge1, Edge edge2)
        {
            var l1P1 = new Vector2(edge1.p1.position.x, edge1.p1.position.y);
            var l1P2 = new Vector2(edge1.p2.position.x, edge1.p2.position.y);
        
            var l2P1 = new Vector2(edge2.p1.position.x, edge2.p1.position.y);
            var l2P2 = new Vector2(edge2.p2.position.x, edge2.p2.position.y);

            bool isIntersecting = AreLinesIntersecting(l1P1, l1P2, l2P1, l2P2, true);

            return isIntersecting;
        }

        private static bool AreLinesIntersecting(Vector2 l1P1, Vector2 l1P2, Vector2 l2P1, Vector2 l2P2, bool shouldIncludeEndPoints)
        {
            //http://thirdpartyninjas.com/blog/2008/10/07/line-segment-intersection/
            var isIntersecting = false;

            var denominator = (l2P2.y - l2P1.y) * (l1P2.x - l1P1.x) - (l2P2.x - l2P1.x) * (l1P2.y - l1P1.y);

            //Make sure the denominator is > 0, if not the lines are parallel
            if (denominator != 0f)
            {
                var uA = ((l2P2.x - l2P1.x) * (l1P1.y - l2P1.y) - (l2P2.y - l2P1.y) * (l1P1.x - l2P1.x)) / denominator;
                var uB = ((l1P2.x - l1P1.x) * (l1P1.y - l2P1.y) - (l1P2.y - l1P1.y) * (l1P1.x - l2P1.x)) / denominator;

                //Are the line segments intersecting if the end points are the same
                if (shouldIncludeEndPoints)
                {
                    //Is intersecting if u_a and u_b are between 0 and 1 or exactly 0 or 1
                    if (uA >= 0f && uA <= 1f && uB >= 0f && uB <= 1f)
                    {
                        isIntersecting = true;
                    }
                }
                else
                {
                    //Is intersecting if u_a and u_b are between 0 and 1
                    if (uA > 0f && uA < 1f && uB > 0f && uB < 1f)
                    {
                        isIntersecting = true;
                    }
                }
		
            }

            return isIntersecting;
        }
    }
    
    public class OrientatedLine
    {
        public readonly Vector2 pointA;
        public readonly Vector2 pointB;

        public OrientatedLine(Vector2 pointA, Vector2 pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
        }
    }

    public struct CircleFromPoints
    {
        public Vector2 p1, p2, p3;

        public CircleFromPoints(Vector2 a, Vector2 b, Vector2 c)
        {
            this.p1 = a;
            this.p2 = b;
            this.p3 = c;
        }
    }
}