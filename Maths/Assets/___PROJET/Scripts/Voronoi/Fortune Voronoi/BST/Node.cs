using System;

namespace ___PROJET.Scripts.Voronoi.Fortune_Voronoi.BST
{
    public class Node<T> : NodeBase where T : NodeData
    {
        public Node<T> LeftNode { get; set; }
        public Node<T> RightNode { get; set; }
        public T Data { get; set; }
        public override NodeBase Left
        {
            get => LeftNode;
            set => LeftNode = (Node<T>) value;
        }
        public override NodeBase Right
        {
            get => RightNode;
            set => RightNode = (Node<T>) value;
        }

        public override bool IsLeaf => LeftNode == null && RightNode == null;
    }
}