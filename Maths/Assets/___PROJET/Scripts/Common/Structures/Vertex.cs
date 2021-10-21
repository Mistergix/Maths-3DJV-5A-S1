using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ESGI.Structures
{
    public class Vertex
    {
        public Vector2 position;
        /// <summary>
        /// The half edge coming out of this vertex
        /// </summary>
        public HalfEdge halfEdge;
        /// <summary>
        /// The triangle this vertex belongs to
        /// </summary>
        public Triangle triangle;

        public Vertex previousVertex;
        public Vertex nextVertex;

        public Vertex(Vector2 position)
        {
            this.position = position;
        }
    }
}
