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


        private float _theta;

        private VoronoiMesh3D _voronoi;

        private List<Mesh> _meshes;

        private void Start()
        {

            var vertices = new List<Vertex3D>();
            for (var i = 0; i < NumberOfVertices; i++)
            {
                vertices.Add(new Vertex3D(points3D.positions[i], i));
            }

            _voronoi = new VoronoiMesh3D();
            _voronoi.Generate(vertices);

            //RegionsToMeshes();
        }

        private int NumberOfVertices => points3D.positions.Count;

        private void RegionsToMeshes()
        {
            _meshes = new List<Mesh>();

            foreach (var region in _voronoi.Regions)
            {
                var draw = true;

                var verts = new List<Vertex3D>();

                foreach (var cell in region.Cells)
                {
                    if (!InBound(cell.Center))
                    {
                        draw = false;
                        break;
                    }
                    verts.Add(cell.Center);
                }

                if (!draw) {continue;}

                //If you find the convex hull of the voronoi region it
                //can be used to make a triangle mesh.

                var positions = CreateConvexHull(verts, out var normals, out var indices);
                GenerateMesh(positions, normals, indices);
            }

        }

        private void GenerateMesh(List<Vector3> positions, List<Vector3> normals, List<int> indices)
        {
            var mesh = new Mesh();
            mesh.SetVertices(positions);
            mesh.SetNormals(normals);
            mesh.SetTriangles(indices, 0);

            mesh.RecalculateBounds();

            _meshes.Add(mesh);
        }

        private List<Vector3> CreateConvexHull(List<Vertex3D> verts, out List<Vector3> normals, out List<int> indices)
        {
            var hull = ConvexHull3D.ConvexHull3D.ComputeHull(CreatePoints(verts));

            var positions = new List<Vector3>();
            normals = new List<Vector3>();
            indices = new List<int>();

            for (var i = 0; i < hull.faces.Count; i++)
            {
                var hullFace = hull.faces[i];

                positions.Add(hullFace.p1.position);
                positions.Add(hullFace.p2.position);
                positions.Add(hullFace.p3.position);

                var n = hullFace.GetNormal();

                if (hullFace.IsNormalFlipped)
                {
                    indices.Add(i * 3 + 2);
                    indices.Add(i * 3 + 1);
                    indices.Add(i * 3 + 0);
                }
                else
                {
                    indices.Add(i * 3 + 0);
                    indices.Add(i * 3 + 1);
                    indices.Add(i * 3 + 2);
                }

                normals.Add(n);
                normals.Add(n);
                normals.Add(n);
            }

            return positions;
        }

        private Points3DBase CreatePoints(List<Vertex3D> verts)
        {
            var points = ScriptableObject.CreateInstance<Points3D>();
            points.positions = verts.Select(v => v.position).ToList();
            return points;
        }

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
