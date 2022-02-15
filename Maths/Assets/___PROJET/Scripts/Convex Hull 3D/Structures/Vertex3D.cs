using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ESGI.ConvexHull3D
{
    public class Vertex3D : Node
    {
        public Vector3 position;
        public int index;
        public Vertex3D(Vector3 position, int index)
        {
            this.index = index;
            this.position = position;
        }

        public Vertex3D(Vector3 vector3)
        {
            position = vector3;
        }

        public float X => position.x;
        public float Y => position.y;
        public float Z => position.z;

        public float SqrMagnitude => position.sqrMagnitude;

        public override string ToString()
        {
            return $"Vertex {index} ({position.ToString("F3")})";
        }
    }
}
