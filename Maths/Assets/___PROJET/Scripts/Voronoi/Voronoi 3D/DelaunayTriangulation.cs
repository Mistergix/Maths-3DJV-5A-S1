using System;
using System.Collections.Generic;
using System.Linq;
using ESGI.ConvexHull3D;
using PGSauce.Core.PGDebugging;
using UnityEngine;

namespace ESGI.Voronoi.Voronoi3D
{
    public class DelaunayTriangulation
    {
        private float[,] _mMatrixBuffer;

        public DelaunayTriangulation()
        {
            Cells = new List<DelaunayCell>();
            Vertices = new List<Vertex3D>();
            _mMatrixBuffer = new float[4, 4];
        }

        public List<DelaunayCell> Cells { get; }
        public List<Vertex3D> Vertices { get; private set; }
        
        public Vertex3D Center { get; private set; }

        public virtual void Clear()
        {
            Cells.Clear();
            Vertices.Clear();
            Center = new Vertex3D(Vector3.zero);
        }
        
        public void Generate(List<Vertex3D> input)
        {
            Clear();

            var hull = ConvexHull3D.ConvexHull3D.ComputeHull(CreatePoints(input));

            Vertices = new List<Vertex3D>(hull.vertices);

            Center.position = hull.Center.position;

            var count = hull.faces.Count;

            for (var i = 0; i < count; i++)
            {
                var face = hull.faces[i];

                if (FilterFace(face))
                {
                    foreach (var f in face.Adjacent)
                    {
                        if (f != null)
                        {
                            f.Remove(face);
                        }
                    }
                }
                else
                {
                    var cell = CreateCell(face);
                    cell.Center.index = i;
                    Cells.Add(cell);
                }

            }
        }

        private bool FilterFace(Face3D face)
        { 
            PGDebug.Message($"FILTER FACE").LogTodo();
            return false;
        }

        private float Minor(int r0, int r1, int r2, int c0, int c1, int c2)
        {
            return _mMatrixBuffer[r0, c0] * (_mMatrixBuffer[r1, c1] * _mMatrixBuffer[r2, c2] - _mMatrixBuffer[r2, c1] * _mMatrixBuffer[r1, c2]) -
                   _mMatrixBuffer[r0, c1] * (_mMatrixBuffer[r1, c0] * _mMatrixBuffer[r2, c2] - _mMatrixBuffer[r2, c0] * _mMatrixBuffer[r1, c2]) +
                   _mMatrixBuffer[r0, c2] * (_mMatrixBuffer[r1, c0] * _mMatrixBuffer[r2, c1] - _mMatrixBuffer[r2, c0] * _mMatrixBuffer[r1, c1]);
        }
        
        private float Determinant()
        {
            return (_mMatrixBuffer[0, 0] * Minor(1, 2, 3, 1, 2, 3) -
                    _mMatrixBuffer[0, 1] * Minor(1, 2, 3, 0, 2, 3) +
                    _mMatrixBuffer[0, 2] * Minor(1, 2, 3, 0, 1, 3) -
                    _mMatrixBuffer[0, 3] * Minor(1, 2, 3, 0, 1, 2));
        }

        private DelaunayCell CreateCell(Face3D face)
        {
            var verts = new List<Vertex3D>{face.p1, face.p2, face.p3};

            // x, y, z, 1
            for (var i = 0; i < 4; i++)
            {
                _mMatrixBuffer[i, 0] = verts[i].position[0];
                _mMatrixBuffer[i, 1] = verts[i].position[1];
                _mMatrixBuffer[i, 2] = verts[i].position[2];
                _mMatrixBuffer[i, 3] = 1;
            }
            var a = Determinant();

            // size, y, z, 1
            for (var i = 0; i < 4; i++)
            {
                _mMatrixBuffer[i, 0] = verts[i].SqrMagnitude;
            }
            var dx = Determinant();

            // size, x, z, 1
            for (var i = 0; i < 4; i++)
            {
                _mMatrixBuffer[i, 1] = verts[i].position[0];
            }
            var dy = -Determinant();

            // size, x, y, 1
            for (var i = 0; i < 4; i++)
            {
                _mMatrixBuffer[i, 2] = verts[i].position[1];
            }
            var dz = Determinant();

            //size, x, y, z
            for (var i = 0; i < 4; i++)
            {
                _mMatrixBuffer[i, 3] = verts[i].position[2];
            }
            var c = Determinant();

            var s = -1.0f / (2.0f * a);
            var radius = Math.Abs(s) * (float)Math.Sqrt(dx * dx + dy * dy + dz * dz - 4 * a * c);

            var center = new Vector3(dx,dy,dz) * s;

            return new DelaunayCell(face, center);
        }

        private Points3DBase CreatePoints(List<Vertex3D> verts)
        {
            var points = ScriptableObject.CreateInstance<Points3D>();
            points.positions = verts.Select(v => v.position).ToList();
            return points;
        }
    }
}