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
    }
}