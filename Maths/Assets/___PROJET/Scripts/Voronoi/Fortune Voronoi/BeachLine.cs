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

            if(bstRoot.LeftNode.IsLeaf && bstRoot.RightNode.IsLeaf){
                return BothLeaves(bstRoot, site);
            }

            if(bstRoot.RighNode.IsLeaf){
                PGDegub.Message("Should not happen, with the way we insert nodes in beach line").LogError();
            }

            if(bstRoot.LeftNode.IsLeaf)
            {
                return OneLeafOneNode(bstRoot, site);
            }

            return BothNodes(bstRoot, site);
        }

        private VoronoiNodeData BothNodes(Node<VoronoiNodeData> bstRoot, Vector2 site){
            Arc leftArc;
            Arc rightArc;

            var left = bstRoot.LeftNode;
            var right = bstRoot.RightNode;

            // The arc to the left of the break point is the right most node in the left tree
            while(left.RighNode != null){
                left = left.RightNode;
            }

            leftArc = left.Data.Arc;

            // The arc to the right of the break point is the left most node in the right tree
            while(right.LeftNode != null){
                right = right.LeftNode;
            }

            rightArc = right.Data.Arc;

            Vector2 breakPoint = ComputeBreakPoint(leftArc, rightArc);

            if(breakPoint.x > site.x){
            // break point to the right, so we return left arc
                return FindArc(site, bstRoot.LeftNode);
            }

            if(breakPoint.x < site.x){
                return FindArc(site, bstRoot.RightNode);
            }

            PGDebug.Message("The site is directly under a breakpoint, what to do ?").LogTodo();
            return bstRoot.LeftNode.Data;
        }

        private VoronoiNodeData OneLeafOneNode(Node<VoronoiNodeData> bstRoot, Vector2 site){
            Arc leftArc = bstRoot.LeftNode.Data.Arc;
            Arc rightArc;

            var right = bstRoot.RightNode;

            // The arc to the right of the break point is the left most node in the right tree
            while(right.LeftNode != null){
                right = right.LeftNode;
            }

            rightArc = right.Data.Arc;

            Vector2 breakPoint = ComputeBreakPoint(leftArc, rightArc);

            if(breakPoint.x > site.x){
            // break point to the right, so we return left arc
                return bstRoot.LeftNode.Data;
            }

            if(breakPoint.x < site.x){
                return FindArc(site, bstRoot.RightNode);
            }

            PGDebug.Message("The site is directly under a breakpoint, what to do ?").LogTodo();
            return bstRoot.LeftNode.Data;
        }

        private VoronoiNodeData BothLeaves(Node<VoronoiNodeData> bstRoot, Vector2 site){
            Arc leftArc = bstRoot.LeftNode.Data.Arc;
            Arc rightArc = bstRoot.RightNode.LeftNode.Data.Arc;

            Vector2 breakPoint = ComputeBreakPoint(leftArc, rightArc);

            if(breakPoint.x > site.x){
                // break point to the right, so we return left arc
                return bstRoot.LeftNode.Data;
            }

            if(breakPoint.x < site.x){
                return bstRoot.RightNode.Data;
            }

            PGDebug.Message("The site is directly under a breakpoint, what to do ?").LogTodo();
            return bstRoot.RightNode.Data;
        }

        public void AddRootNode(Vector2 site)
        {
            _bst.Root = new VoronoiNode(site);
        }
    }
}