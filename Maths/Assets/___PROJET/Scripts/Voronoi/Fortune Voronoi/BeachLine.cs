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
        private VoronoiFortune _voronoi;

        public BeachLine(VoronoiFortune voronoiFortune)
        {
            _bst = new BinarySearchTree<VoronoiNodeData>();
            _voronoi = voronoiFortune;
        }

        public VoronoiNodeData GetArcAboveSite(Vector2 site)
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

        private Vector2 ComputeBreakPoint(Arc leftArc, Arc rightArc)
        {
            // https://www.emathzone.com/tutorials/geometry/equation-of-a-circle-given-two-points-and-tangent-line.html
            var lineY = _voronoi.lineY;
            var siteA = leftArc.site;
            var siteB = rightArc.site;

            leftArc.UpdateDirectrix(lineY);
            rightArc.UpdateDirectrix(lineY);

            var (a1,b1,c1) = leftArc.parabola.GetCoeffs();
            var (a2,b2,c2) = rightArc.parabola.GetCoeffs();

            var a = a1 - a2;
            var b = b1 - b2;
            var c = c1 - c2;

            if(a == 0.0f){
                if(b == 0.0f){
                    if(c==0.0f){
                        PGDebug.Message("parabolas are the same, so the sites are the same, infinite intersections").LogError();
                        return Vector2.zero;
                    }

                    PGDebug.Message("parabolas only differ vertically, should never happen, no intersections").LogError();
                    return Vector2.zero;
                }

                var x = -c / b;

                return leftArc.Compute(x);
            }

            var delta = b * b - 4 * a * c;

            if(delta < 0) {
                PGDebug.Message("one parabola is inside the other, should never happen, no intersections").LogError();
                return Vector2.zero;
            }

            if(delta == 0.0f){
                var x = -b / (2 * a);
                return leftArc.Compute(x);
            }

            var sqrDelta = Mathf.Sqrt(delta);

            var x1 = (-b - sqrDelta) / (2 * a);
            var x2 = (-b + sqrDelta) / (2 * a);

        }

        public void AddRootNode(Vector2 site)
        {
            _bst.Root = new VoronoiNode(site);
        }
    }
}