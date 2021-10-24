
using ESGI.Voronoi.Fortune;
using UnityEngine;

namespace ESGI.Voronoi
{
    public class VoronoiNode
    {
        private VoronoiNode _left, _right;
        
        public VoronoiNodeData Data { get; }

        public VoronoiNode LeftNode
        {
            get => _left;
            set
            {
                _left = value;
                _left.Parent = this;
            }
        }

        public VoronoiNode RightNode
        {
            get => _right;
            set
            {
                _right = value;
                _right.Parent = this;
            }
        }
        
        
        
        public VoronoiNode Parent { get; set; }
        public bool IsLeaf => LeftNode == null && RightNode == null;

        public VoronoiNode(Vector2 nodeSite)
        {
            Data = new VoronoiNodeData(nodeSite) {Node = this};
        }
    }
}