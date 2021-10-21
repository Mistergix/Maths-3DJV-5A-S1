using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ESGI.ConvexHull2D
{
    public class GrahamScan : ConvexHull2D
    {
        public static List<Vector2> ComputeGrahamScan(List<Vector2> vectors)
        {
            if (vectors.Count <= 3)
            {
                return vectors.ToList();
            }

            var indexPivot = GetPivotPoint(vectors);
            
            var sort = SortPointsByAngle(vectors, indexPivot);

            var points = new List<Vector2>();
            points.Add(vectors[indexPivot]);
            points.AddRange(sort);

            var pile = new List<Vector2>();
            pile.Insert(0, points[0]);
            pile.Insert(0, points[1]);

            for (var i = 2; i < points.Count; i++)
            {
                while ((EnoughPointsInPile(pile) && IsPointToTheRightOfSegment(points, i, pile)))
                {
                    pile.RemoveAt(0);
                }
                pile.Insert(0, points[i]);
            }

            pile.Reverse();
            
            return pile;
        }

        private static bool IsPointToTheRightOfSegment(List<Vector2> points, int i, List<Vector2> pile)
        {
            return IsToTheRight(points[i], new OrientatedLine(pile[1], pile[0]));
        }

        private static bool EnoughPointsInPile(List<Vector2> pile)
        {
            return pile.Count >= 2;
        }
        
        

        private static IEnumerable<Vector2> SortPointsByAngle(IReadOnlyList<Vector2> vectors, int indexPivot)
        {
            return Enumerable.Range(0, vectors.Count)
                .Where(i => i != indexPivot) // skip pivot
                .Select(i => new KeyValuePair<float, Vector2>(GetAngleToPoint(vectors, indexPivot, i), vectors[i]))
                .OrderBy(pair => pair.Key)
                .Select(pair => pair.Value);
        }

        private static float GetAngleToPoint(IReadOnlyList<Vector2> vectors, int indexPivot, int i)
        {
            return Mathf.Atan2(vectors[i].y - vectors[indexPivot].y, vectors[i].x - vectors[indexPivot].x);
        }

        protected override List<Vector2> ComputeHull(List<Vector2> vectors)
        {
            return ComputeGrahamScan(vectors);
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
}
