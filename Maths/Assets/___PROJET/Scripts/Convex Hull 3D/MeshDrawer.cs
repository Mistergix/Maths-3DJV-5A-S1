using PGSauce.Core.PGDebugging;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ESGI.ConvexHull3D
{
    public class MeshDrawer : MonoBehaviour
    {
        private MeshFilter myMesh;
        public List<int> index;
        private void Start()
        {
            myMesh = transform.GetComponent<MeshFilter>();
        }
        public void DrawMesh(IncidenceGraph convexHull)
        {
            Vector3[] vertex = convexHull.vertices.Select(pos => pos.position).ToArray();
            Mesh mesh = new Mesh();
            var meshRenderer = GetComponent<MeshRenderer>();
            mesh.vertices = vertex;
            mesh.triangles = calculateTriangles(convexHull.faces).ToArray();
            mesh.RecalculateNormals();
            myMesh.mesh = mesh;
        }


        private List<int> calculateTriangles(List<Face3D> faces)
        {
            index = new List<int>();
            for(int i = 0; i < faces.Count; i++)
            {
                index.Add(faces[i].p1.index);
                index.Add(faces[i].p2.index);
                index.Add(faces[i].p3.index);

                index.Add(faces[i].p2.index);
                index.Add(faces[i].p1.index);
                index.Add(faces[i].p3.index);
            }
            return index;
            
        }
    }
}