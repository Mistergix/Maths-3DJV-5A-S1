using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ESGI.Common;
using PGSauce.Core.Utilities;
using Shapes;
using Sirenix.OdinInspector;

namespace ESGI.ConvexHull2D
{
    public class GrahamScan : ImmediateModeShapeDrawer
    {
        public List<Vector2> positions;
        public DisplayData displayData;
        private static Vector2 _barycentre;
        
        private void Update()
        {
            ComputeGrahamScan(positions);
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
                
                Draw.Disc(_barycentre, displayData.pointSize, displayData.barycentreColor);
            }
        }

        public static void ComputeGrahamScan(List<Vector2> vectors)
        {
            var points = new List<Vector2>(vectors);

            _barycentre = GetBaryCentre(points);
        }

        private static Vector2 GetBaryCentre(List<Vector2> points)
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
            for (int i = 0; i < count; i++)
            {
                positions.Add(PGUtils.RandomVector3(range));
            }
        }
    }
}
