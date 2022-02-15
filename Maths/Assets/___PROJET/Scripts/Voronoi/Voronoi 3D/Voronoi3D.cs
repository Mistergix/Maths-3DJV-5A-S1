using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ESGI.ConvexHull3D;

namespace ESGI.Voronoi.Voronoi3D
{
    public class Voronoi3D : MonoBehaviour
    {
        public Points3D points3D;

        public float size = 5;


        public bool drawLines;


        private VoronoiMesh3D _voronoi;

        private void Start()
        {

            var vertices = new List<Vertex3D>();
            for (var i = 0; i < NumberOfVertices; i++)
            {
                vertices.Add(new Vertex3D(points3D.positions[i], i));
            }

            _voronoi = new VoronoiMesh3D();
            _voronoi.Generate(vertices);
        }

        private int NumberOfVertices => points3D.positions.Count;

        private void OnDrawGizmos()
        {
            if (!drawLines) return;

            if (_voronoi == null || _voronoi.Regions.Count == 0) return;
            
            foreach (var region in _voronoi.Regions)
            {
                var draw = true;

                foreach (var cell in region.Cells)
                {
                    if (!InBound(cell.Center))
                    {
                        draw = false;
                        break;
                    }
                }

                if (!draw) continue;

                foreach (var edge in region.Edges)
                {
                    var v0 = edge.From.Center;
                    var v1 = edge.To.Center;

                    DrawLine(v0, v1);
                }
            }
        }

        private void DrawLine(Vertex3D v0, Vertex3D v1)
        {
            Debug.DrawLine(v0.position, v1.position, Color.red);
        }

        private bool InBound(Vertex3D v)
        {
            if (v.X < -size || v.X > size) return false;
            if (v.Y < -size || v.Y > size) return false;
            if (v.Z < -size || v.Z > size) return false;

            return true;
        }
    }
}
