using System.Collections.Generic;
using System.Linq;
using ___PROJET.Scripts.Voronoi.Fortune_Voronoi.BST;
using ESGI.Voronoi;
using ESGI.Voronoi.Fortune;
using PGSauce.Core.PGDebugging;
using UnityEngine;

namespace ___PROJET.Scripts.Voronoi.Fortune_Voronoi
{
    public class BeachLine
    {
        private BinarySearchTree<VoronoiNodeData> _bst;

        public BeachLine()
        {
            _bst = new BinarySearchTree<VoronoiNodeData>();
        }

        public VoronoiNodeData GetArcAboveSite(VoronoiFortune voronoiFortune, Vector2 site)
        {
            if (_bst.Root == null)
            {
                return null;
            }
            
            return FindArc(site, _bst.Root);
        }

        private VoronoiNodeData FindArc(Vector2 site, Node<VoronoiNodeData> bstRoot)
        {
            if (bstRoot.IsLeaf)
            {
                return bstRoot.Data;
            }
            
            // Must have two children

            if (bstRoot.Left == null || bstRoot.Right == null)
            {
                PGDebug.Message("A child is missing").LogError();
                return null;
            }

            if (bstRoot.RightNode.LeftNode == null)
            {
                PGDebug.Message("A child is missing in the children").LogError();
                return null;
            }

            Arc leftArc = bstRoot.LeftNode.Data.Arc;
            Arc rightArc = bstRoot.RightNode.LeftNode.Data.Arc;

            Vector2 breakPoint = ComputeBreakPoint(leftArc, rightArc);
            
            
        }

        public void AddRootNode(Vector2 site)
        {
            _bst.Root = new VoronoiNode(site);
        }
    }
}