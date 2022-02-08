using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ESGI.ConvexHull3D
{
    public class Vertex3D : Node
    {
        public Vector3 position;
        public List<Edge3D> edges;
        public Vertex3D(Vector3 position)
        {
            this.position = position;
            edges = new List<Edge3D>();
        }

        public void AddEdge(Edge3D edge3D)
        {
            edges.Add(edge3D);
        }
    }
}
