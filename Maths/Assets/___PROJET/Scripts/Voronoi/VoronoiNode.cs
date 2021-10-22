using ___PROJET.Scripts.Voronoi.Fortune_Voronoi;
using ___PROJET.Scripts.Voronoi.Fortune_Voronoi.BST;
using UnityEngine;

namespace ESGI.Voronoi
{
    public class VoronoiNode : Node<VoronoiNodeData>
    {
        public VoronoiNode(Vector2 nodeSite)
        {
            Data = new VoronoiNodeData(nodeSite) {Node = this};
        }
    }
}