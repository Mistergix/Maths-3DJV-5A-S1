using ESGI.Structures;
using UnityEngine;

namespace ESGI.Voronoi.Fortune
{
    public class VoronoiEdge{
        private Vertex _start,_left, _right, _end,_secondPoint;
        private float _coeffDir, _originOrdonnee;
        private Vector2 _direction;
        private VoronoiEdge _twin;
        private bool intersected, counted;
        public VoronoiEdge(Vertex start, Vertex left, Vertex right){
            _start = start;
            _left = left;
            _right = right;

            _coeffDir = (right.x - left.x) / (left.y - right.y);
            _originOrdonnee = start.y - CoeffDir * start.x;
            _direction = new Vector2(right.y - left.y, -(right.x - left.x));
            _secondPoint = new Vertex(new Vector2(start.x + Direction.x, start.y + Direction.y));
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

        public Vertex SecondPoint => _secondPoint;

        public float GetY(float border)
        {
            return CoeffDir * border + OriginOrdonnee;
        }
    }
}