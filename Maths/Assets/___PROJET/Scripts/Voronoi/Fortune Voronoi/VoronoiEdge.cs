using ESGI.Structures;
using UnityEngine;

namespace ESGI.Voronoi.Fortune
{
    public class VoronoiEdge{
        private Vertex _start,_left, _right, _end;
        private float _coeffDir, _originOrdonnee;
        private Vector2 _direction, _secondPoint;
        private VoronoiEdge _twin;
        private bool intersected, counted;
        public VoronoiEdge(Vector2 start, Vector2 left, Vector2 right){
            _start = new Vertex(start);
            _left = new Vertex(left);
            _right = new Vertex(right);

            _coeffDir = (right.x - left.x) / (right.y - left.y);
            _originOrdonnee = start.y - CoeffDir * start.x;
            _direction = new Vector2(right.y - left.y, -(right.x - left.x));
            _secondPoint = new Vector2(start.x + Direction.x, start.y + Direction.y);
        }

        public VoronoiEdge Twin
        {
            get => _twin;
            set { _twin = value; }
        }

        public Vertex Start
        {
            get => _start;
            set => _start = value;
        }

        public Vertex Left => _left;

        public Vertex Right => _right;

        public Vector2 Direction => _direction;

        public Vertex End
        {
            get => _end;
            set => _end = value;
        }

        private float CoeffDir => _coeffDir;

        private float OriginOrdonnee => _originOrdonnee;

        public float GetY(float border)
        {
            return CoeffDir * border + OriginOrdonnee;
        }
    }
}