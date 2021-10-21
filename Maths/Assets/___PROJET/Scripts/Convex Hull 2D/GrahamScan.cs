using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ESGI.Common;
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Utilities;
using Shapes;
using Sirenix.OdinInspector;

namespace ESGI.ConvexHull2D
{
    public class GrahamScan : ImmediateModeShapeDrawer
    {
        public List<Vector2> positions;
        public DisplayData displayData;
        private List<Vector2> _hull;

        private void Start()
        {
            _hull = new List<Vector2>();
        }

        private void Update()
        {
            _hull = ComputeGrahamScan(positions);
        }

        public override void DrawShapes(Camera cam)
        {
            base.DrawShapes(cam);
            using (Draw.Command(cam))
            {
                foreach (var point in positions)
                {
                    Draw.Disc(point, displayData.pointSize, displayData.pointColor);
                }

                for (var i = 0; i < _hull.Count; i++)
                {
                    Draw.UseDashes = true;
                    Draw.DashStyle = DashStyle.defaultDashStyle;
                    Draw.Line(_hull[i], i == _hull.Count - 1 ? _hull[0] : _hull[i + 1]);
                }
            }
        }

        public static List<Vector2> ComputeGrahamScan(List<Vector2> vectors)
        {
            if (vectors.Count <= 2)
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

        private static bool IsToTheRight(Vector2 point, OrientatedLine orientatedLine)
        {
            var det = Determinant(orientatedLine, point);
            if (det != 0.0f) {return det > 0;}
            return HandleAlignedPoints(point, orientatedLine);
        }

        private static bool HandleAlignedPoints(Vector2 point, OrientatedLine orientatedLine)
        {
            return false;
        }

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
        
        private static int GetPivotPoint(IReadOnlyList<Vector2> vectors)
        {
            return Enumerable.Range(0, vectors.Count)
                .Aggregate((indexMin, indexCurrent) =>
                {
                    if (vectors[indexCurrent].y < vectors[indexMin].y)
                    {
                        return indexCurrent;
                    }

                    if (vectors[indexCurrent].y > vectors[indexMin].y)
                    {
                        return indexMin;
                    }

                    if (vectors[indexCurrent].x < vectors[indexMin].x)
                    {
                        return indexCurrent;
                    }

                    return indexMin;
                });
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

        private static Vector2 GetBaryCentre(IReadOnlyCollection<Vector2> points)
        {
            if (points.Count <= 0)
            {
                return Vector2.zero;
            }
            var b = points.Aggregate(Vector2.zero, (current, point) => current + point);
            return b / points.Count;
        }

        [Button]
        private void GenerateRandomPoints(MinMax<Vector3> range, int count = 20)
        {
            positions.Clear();
            range.max.z = 0;
            range.min.z = 0;
            for (var i = 0; i < count; i++)
            {
                positions.Add(PGUtils.RandomVector3(range));
            }
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
