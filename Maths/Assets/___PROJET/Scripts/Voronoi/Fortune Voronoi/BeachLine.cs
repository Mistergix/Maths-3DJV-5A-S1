﻿using ESGI.Structures;
using ESGI.Voronoi;
using ESGI.Voronoi.Fortune;
using PGSauce.Core.PGDebugging;
using UnityEngine;

namespace ESGI.Voronoi.Fortune
{
    public class BeachLine
    {
        private BinarySearchTree _bst;
        private VoronoiFortune _voronoi;

        public BeachLine(VoronoiFortune voronoiFortune)
        {
            _bst = new BinarySearchTree();
            _voronoi = voronoiFortune;
        }

        public VoronoiNodeData GetArcAboveSite(Vector2 site)
        {
            if (_bst.Root == null)
            {
                return null;
            }

            var node = _bst.Root;
            while(!node.IsLeaf){
                var x = GetX(node.Data.Node, _voronoi.lineY);
                node = x > site.x ? node.LeftNode : node.RightNode;
            }

            return node.Data;
        }

        private float GetX(VoronoiNode node, float lineY){
            var left = GetLeftChild(node);
            var right = GetRightChild(node);

            return ComputeBreakPointX(left.Data.Arc, right.Data.Arc);
        }

        public VoronoiNode GetLeftChild(VoronoiNode node)
        {
            if (node == null)
            {
                return null;
            }
            var left = node.LeftNode;

            // The arc to the left of the break point is the right most node in the left tree
            while(!left.IsLeaf){
                left = left.RightNode;
            }

            return left;
        }

        public VoronoiNode GetRightChild(VoronoiNode node){
            if (node == null)
            {
                return null;
            }
            var right = node.RightNode;

            // The arc to the right of the break point is the left most node in the right tree
            while(!right.IsLeaf){
                right = right.LeftNode;
            }

            return right;
        }

        public VoronoiNode GetLeftParent(VoronoiNode node){
            var parent = node.Parent;
            var last = node;

            while(parent.LeftNode.Equals(last)){
                if(parent.Parent == null) {return null;}
                last = parent;
                parent = parent.Parent;
            }

            return parent;
        }

        public VoronoiNode GetRightParent(VoronoiNode node){
            var parent = node.Parent;
            var last = node;

            while(parent.RightNode.Equals(last)){
                if(parent.Parent == null) {return null;}
                last = parent;
                parent = parent.Parent;
            }

            return parent;
        }

        private float ComputeBreakPointX(Arc leftArc, Arc rightArc)
        {
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
                        return 0;
                    }

                    PGDebug.Message("parabolas only differ vertically, should never happen, no intersections").LogError();
                    return 0;
                }

                var x = -c / b;

                return x;
            }

            var delta = b * b - 4 * a * c;

            if(delta < 0) {
                PGDebug.Message("one parabola is inside the other, should never happen, no intersections").LogError();
                return 0;
            }

            if(delta == 0.0f){
                var x = -b / (2 * a);
                return x;
            }

            var sqrDelta = Mathf.Sqrt(delta);

            var x1 = (-b - sqrDelta) / (2 * a);
            var x2 = (-b + sqrDelta) / (2 * a);

            var ry = 0.0f;

            if(siteA.y < siteB.y) {return x2;}
            return x1;
        }


        public void FinishEdge()
        {
            FinishEdge(_bst.Root);
        }

        private void FinishEdge(VoronoiNode node)
        {
            var border = 0.0f;
            var edgeX = node.Data.Edge.Start.x;
            if (node.Data.Edge.Direction.x > 0)
            {
                border = Mathf.Max(_voronoi.Width, edgeX + _voronoi.Offset);
            }
            else
            {
                border = Mathf.Min(0.0f, edgeX - _voronoi.Offset);
            }

            var y = node.Data.Edge.GetY(border);
            node.Data.Edge.End = new Vertex(new Vector2(border, y));
            if (!node.LeftNode.IsLeaf)
            {
                FinishEdge(node.LeftNode);
            }
            
            if (!node.RightNode.IsLeaf)
            {
                FinishEdge(node.RightNode);
            }
        }

        public void CreateRoot(Vector2 site)
        {
            _bst.Root = new VoronoiNode(site);
        }
    }
}