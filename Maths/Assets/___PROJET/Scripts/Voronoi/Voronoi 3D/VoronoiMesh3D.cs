using System.Collections.Generic;
using System.Linq;
using ESGI.ConvexHull3D;
using ESGI.Voronoi.Fortune;
using UnityEngine;

namespace ESGI.Voronoi.Voronoi3D
{
    public class VoronoiMesh3D
    {
        public int Dimension => 3;
        public List<DelaunayCell> Cells { get; }
        public List<VoronoiRegion> Regions { get; }

        public VoronoiMesh3D()
        {
            Cells = new List<DelaunayCell>();
            Regions = new List<VoronoiRegion>();
        }

        public void Clear()
        {
            Cells.Clear();
            Regions.Clear();
        }

        public void Generate(List<Vertex3D> input)
        {
            var delaunay = new DelaunayTriangulation();
            Generate(input, delaunay);
        }

        private void Generate(List<Vertex3D> input, DelaunayTriangulation delaunay)
        {

            Clear();

            delaunay.Generate(input);

            for (var i = 0; i < delaunay.Vertices.Count; i++)
            {
                delaunay.Vertices[i].index = i;
            }

            for (var i = 0; i < delaunay.Cells.Count; i++)
            {
                delaunay.Cells[i].Center.index = i;
                delaunay.Cells[i].Face.index = i;
                Cells.Add(delaunay.Cells[i]);
            }

            var cells = new List<DelaunayCell>();
            var neighbourCell = new Dictionary<int, DelaunayCell>();

            foreach (var vertex in delaunay.Vertices)
            {
                cells.Clear();

                foreach (var cell in delaunay.Cells)
                {
                    var face = cell.Face;
                    var verts = new List<Vertex3D>() {face.p1, face.p2, face.p3};
                    if (verts.Any(v => v.index == vertex.index))
                    {
                        cells.Add(cell);
                    }
                }

                if (cells.Count > 0)
                {
                    var region = new VoronoiRegion();

                    foreach (var cell in cells)
                    {
                        region.Cells.Add(cell);
                    }

                    neighbourCell.Clear();

                    foreach (var cell in cells)
                    {
                        neighbourCell.Add(cell.Center.index, cell);
                    }

                    foreach (var cell in cells)
                    {
                        var face = cell.Face;

                        foreach (var f in face.Adjacent)
                        {
                            if (f == null) continue;

                            var index = f.index;

                            if (!neighbourCell.ContainsKey(index)) {continue;}
                            var edge = new VoronoiEdge3D(cell, neighbourCell[index]);
                            region.Edges.Add(edge);
                        }
                    }

                    region.Id = Regions.Count;
                    Regions.Add(region);
                }
            }

        }

    }
}