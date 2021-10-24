using System.Collections.Generic;
using ESGI.Structures;

namespace ESGI.Voronoi.Fortune
{
    public class VoronoiCell
    {
        private List<Vertex> _vertices;
        public Vertex Last => _vertices.Count == 0 ? null : _vertices[_vertices.Count - 1];
        public Vertex First => _vertices.Count == 0 ? null : _vertices[0];

        public VoronoiCell()
        {
            _vertices = new List<Vertex>();
        }

        public void AddRight(Vertex p)
        {
            _vertices.Add(p);
        }

        public void AddLeft(Vertex p)
        {
            _vertices.Insert(0, p);
        }
    }
}