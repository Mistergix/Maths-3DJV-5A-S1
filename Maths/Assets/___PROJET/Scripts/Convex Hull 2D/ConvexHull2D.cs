﻿using System.Collections.Generic;
using System.Linq;
using ESGI.Common;
using PGSauce.Core.Utilities;
using Shapes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ESGI.ConvexHull2D
{
    public abstract class ConvexHull2D : ImmediateModeShapeDrawer
    {
        [SerializeField] private Points points;
        private List<Vector2> positions => points.positions;
        [SerializeField] private DisplayData displayData;
        
        private List<Vector2> _hull;

        private void Start()
        {
            _hull = new List<Vector2>();
        }

        private void Update()
        {
            _hull = ComputeHull(positions);
        }

        protected abstract List<Vector2> ComputeHull(List<Vector2> vectors);

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
        
        protected static int GetPivotPoint(IReadOnlyList<Vector2> vectors)
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
        
        protected static bool IsToTheRight(Vector2 point, OrientatedLine orientatedLine)
        {
            return Geom.IsToTheRight(point, orientatedLine);
        }
        
        
    }
}