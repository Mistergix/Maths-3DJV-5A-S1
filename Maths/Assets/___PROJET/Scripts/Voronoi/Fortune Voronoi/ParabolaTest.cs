using System;
using PGSauce.Core;
using PGSauce.Core.Utilities;
using Shapes;
using UnityEngine;

namespace ESGI.Common
{
    public class ParabolaTest : ImmediateModeShapeDrawer
    {
        public Vector2 focus;
        public float lineY = -1;
        public MinMax<float> xRange;
        [Min(2)] public int iterations = 50;

        private Parabola _parabola;

        private void Awake()
        {
            _parabola = new Parabola();
        }

        public override void DrawShapes(Camera cam)
        {
            base.DrawShapes(cam);
            using (Draw.Command(cam))
            {
                _parabola.ComputeParabolaFromFocusAndHorizontalLine(focus, lineY);
                var step = (xRange.max - xRange.min) / (iterations - 1);
                for (int i = 0; i < iterations; i++)
                { 
                    var x = xRange.min + step * i; 
                    var y = _parabola.Compute(x); 
                    Draw.Disc(new Vector2(x, y), 0.2f);
                }

                Draw.Color = PGColors.Greenish;
                Draw.Disc(focus, 0.2f);
                
                Draw.Line(new Vector3(xRange.min, lineY), new Vector3(xRange.max, lineY));
            }
        }
    }
}