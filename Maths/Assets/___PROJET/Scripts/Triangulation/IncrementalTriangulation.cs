using System;
using System.Collections.Generic;
using System.Linq;
using ESGI.Common;
using ESGI.Structures;
using PGSauce.Core.PGDebugging;
using UnityEngine;

namespace ESGI.Triangulation
{
    public class IncrementalTriangulation : TriangulationBase
    {
        protected override List<Triangle> Triangulate(List<Vertex> sites)
        {
            var triangles = new List<Triangle>();

            if (sites.Count < 3)
            {
                return triangles;
            }

            var points = sites.ToList();
                
            SortPointsByXThenY(points);

            var x = points[0].position.x;
            var index = 0;
            while (index < points.Count && AlmostTheSame(x, points[index].position.x))
            {
                index++;
            }

            if (index == 1)
            {
                index++;
            }

            if (index > points.Count - 1)
            {
                PGDebug.Message("Most of the points are aligned vertically").LogWarning();
                return triangles;
            }

            var triangle = new Triangle(points[0], points[index-1], points[index]);
            
            triangles.Add(triangle);

            var edges = new List<Edge>();
            
            edges.AddTriangle(triangle);

            for (var i = index + 1; i < points.Count; i++)
            {
                var currentPoint = points[i].position;
                var newEdges = new List<Edge>();

                CheckForVisibleEdges(edges, currentPoint, newEdges, triangles);

                edges.AddRange(newEdges);
            }
            
            return triangles;
        }

        private static void CheckForVisibleEdges(IReadOnlyCollection<Edge> edges, Vector2 currentPoint, ICollection<Edge> newEdges, ICollection<Triangle> triangles)
        {
            foreach (var currentEdge in edges)
            {
                var mid = currentEdge.MidPoint;
                var edgeToMid = new Edge(currentPoint, mid);

                var canSee = edges
                    .Where(e => !e.Equals(currentEdge))
                    .All(e => !Geom.Intersecting(edgeToMid, e));

                if (!canSee) continue;

                var edge1 = new Edge(currentEdge.p1, new Vertex(currentPoint));
                var edge2 = new Edge(currentEdge.p2, new Vertex(currentPoint));

                newEdges.Add(edge1);
                newEdges.Add(edge2);

                var tri = new Triangle(edge1.p1, edge1.p2, edge2.p1);
                triangles.Add(tri);
            }
        }

        private static bool AlmostTheSame(float a, float b)
        {
           return Mathf.Approximately(a, b);
        }

        private static void SortPointsByXThenY(List<Vertex> sites)
        {
            sites.Sort((v1, v2) =>
            {
                var x1 = v1.position.x;
                var x2 = v2.position.x;

                if (x1 < x2)
                {
                    return -1;
                }

                if (x1 > x2)
                {
                    return 1;
                }

                var y1 = v1.position.y;
                var y2 = v2.position.y;

                if (y1 < y2)
                {
                    return -1;
                }

                if (y1 > y2)
                {
                    return 1;
                }

                return 0;
            });
        }
    }
}