using System;
using System.Collections.Generic;
using ESGI.Common;
using ESGI.Structures;
using Shapes;
using UnityEngine;
using Triangle = ESGI.Structures.Triangle;

namespace ESGI.Triangulation
{
    public abstract class TriangulationBase : Drawer2D
    {
        private List<Triangle> _triangulation;

        private void Start()
        {
            _triangulation = new List<Triangle>();
        }

        protected override void CustomUpdate()
        {
            _triangulation = Triangulate(Positions.ToVertices());
        }

        public override void DrawShapes(Camera cam)
        {
            base.DrawShapes(cam);
            using (Draw.Command(cam))
            {
                foreach (var point in Positions)
                {
                    Draw.Disc(point, Data.pointSize, Data.pointColor);
                }

                foreach (var triangle in _triangulation)
                {
                    Draw.UseDashes = true;
                    Draw.DashStyle = DashStyle.defaultDashStyle;
                    Draw.Line(triangle.p1.position, triangle.p2.position, LineColor);
                    Draw.Line(triangle.p2.position, triangle.p3.position, LineColor);
                    Draw.Line(triangle.p3.position, triangle.p1.position, LineColor);
                }
            }
        }

        public abstract Color LineColor { get; }

        protected abstract List<Triangle> Triangulate(List<Vertex> vectors);
    }
}