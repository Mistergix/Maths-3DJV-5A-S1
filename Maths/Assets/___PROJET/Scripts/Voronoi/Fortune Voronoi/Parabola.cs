using PGSauce.Core.PGDebugging;
using UnityEngine;

namespace ESGI.Common
{
    public class Parabola
    {
        private float _a;
        private float _b;
        private float _c;

        public void ComputeParabolaFromFocusAndHorizontalLine(Vector2 focus, float lineY)
        {
            //https://www.varsitytutors.com/hotmath/hotmath_help/topics/finding-the-equation-of-a-parabola-given-focus-and-directrix
            _a = focus.x;
            _b = focus.y;
            _c = lineY;
            /*
            var B = focus.y - lineY;
            var A = focus.y * focus.y - lineY * lineY;

            if (B == 0.0f)
            {
                _a = _b = _c = 0;
                PGDebug.Message("Undefined Parabola").LogTodo();
            }

            _a = 1 / (2 * B);
            _b = -focus.x / B;
            _c = (1 + A) / (2 * B);*/
        }

        public float Compute(float x)
        {
            var A = (x - _a) * (x - _a);
            var B = _b * _b - _c * _c;
            var C = 2 * (_b - _c);
            if (C == 0.0f)
            {
                PGDebug.Message("Undefined Parabola").LogTodo();
                return 0f;
            }

            return (A + B) / C;
        }
    }
}