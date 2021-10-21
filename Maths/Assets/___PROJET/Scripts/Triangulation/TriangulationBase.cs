using System;
using System.Collections.Generic;
using ESGI.Common;
using ESGI.Structures;
using Shapes;
using UnityEngine;
using Triangle = ESGI.Structures.Triangle;

namespace ESGI.Triangulation
{
    public abstract class TriangulationBase : ImmediateModeShapeDrawer
    {
        [SerializeField] private Points points;
        [SerializeField] private DisplayData displayData;
        private List<Vector2> positions => points.positions;

        private List<Triangle> _triangulation;

        private void Start()
        {
            _triangulation = new List<Triangle>();
        }

        private void Update()
        {
            _triangulation = Triangulate(positions.ToVertices());
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

                foreach (var triangle in _triangulation)
                {
                    Draw.UseDashes = true;
                    Draw.DashStyle = DashStyle.defaultDashStyle;
                    Draw.Line(triangle.p1.position, triangle.p2.position);
                    Draw.Line(triangle.p2.position, triangle.p3.position);
                    Draw.Line(triangle.p3.position, triangle.p1.position);
                }
            }
        }

        protected abstract List<Triangle> Triangulate(List<Vertex> vectors);
    }
}