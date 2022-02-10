using System;
using PGSauce.Core;
using PGSauce.Core.PGDebugging;
using UnityEngine;

namespace ESGI.ConvexHull3D
{
    public abstract class Node
    {
        public enum NodeColor
        {
            None,
            Blue,
            Purple,
            Red
        }

        public NodeColor color = NodeColor.None;

        public virtual void SetColor(NodeColor c)
        {
            color = c;
        }

        public Color GetColorFromNodeColor()
        {
            switch (color)
            {
                case NodeColor.None : return PGColors.Black;
                case NodeColor.Blue: return PGColors.Blueish;
                case NodeColor.Purple: return PGColors.Purple;
                case NodeColor.Red: return PGColors.Redish;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected bool NodeEquals(Vertex3D myNode1, Vertex3D myNode2, Vertex3D otherNode1, Vertex3D otherNode2)
        {
            return otherNode1.Equals(myNode1) && otherNode2.Equals(myNode2) || otherNode1.Equals(myNode2) && otherNode2.Equals(myNode1);
        }
    }
}