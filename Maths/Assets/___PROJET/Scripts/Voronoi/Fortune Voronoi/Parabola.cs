using PGSauce.Core.PGDebugging;
using UnityEngine;

namespace ESGI.Common
{
    public class Parabola
    {
        private float focusX;
        private float focusY;
        private float lineY;

        public void ComputeParabolaFromFocusAndHorizontalLine(Vector2 focus, float lineY)
        {
            //https://www.varsitytutors.com/hotmath/hotmath_help/topics/finding-the-equation-of-a-parabola-given-focus-and-directrix
            focusX = focus.x;
            focusY = focus.y;
            this.lineY = lineY;
        }

        public float Compute(float x)
        {
            //https://www.varsitytutors.com/hotmath/hotmath_help/topics/finding-the-equation-of-a-parabola-given-focus-and-directrix
            var A = (x - focusX) * (x - focusX);
            var B = focusY * focusY - lineY * lineY;
            var C = 2 * (focusY - lineY);
            if (C == 0.0f)
            {
                PGDebug.Message("Undefined Parabola").LogTodo();
                return 1000f;
            }

            return (A + B) / C;
        }

        public (float a, float b, float c) GetCoeffs(){
            var A = (x - focusX) * (x - focusX);
            var B = focusY * focusY - lineY * lineY;
            var C = 2 * (focusY - lineY);
            if (C == 0.0f)
            {
                PGDebug.Message("Undefined Parabola").LogTodo();
                return (0,0,0);
            }


            var a = 1 / C;
            var b = (- 2 * focusX) / C;
            //var c = (focusX * focusX + B) / C;
            var c = lineY + C / 4 + focusX * focusX / C; 

            return (a,b,c);
        }
    }
}