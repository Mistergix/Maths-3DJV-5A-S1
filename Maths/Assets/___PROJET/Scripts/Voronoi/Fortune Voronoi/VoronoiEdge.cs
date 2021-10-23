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
            _originOrdonnee = start.y - _coeffDir * x;
            _direction = new Vector2(right.y - left.y, -(right.x - left.x));
            _secondPoint = new Vector2(start.x + _direction.x, start.y + _direction.y);
        }
    }
}