using System;

namespace ___PROJET.Scripts.Voronoi.Fortune_Voronoi.BST
{
    public class BinarySearchTree<T> where T : NodeData
    {
        public Node<T> Root { get; set; }
 
    public bool Add(T value)
    {
        Node<T> before = null, after = this.Root;
 
        while (after != null)
        {
            before = after;
            if (value.Smaller(after.Data)) //Is new node in left tree? 
            {
                after = after.LeftNode;
            }
            else //Is new node in right tree?
            {
                after = after.RightNode;
            }
        }

        var newNode = new Node<T> {Data = value};
        value.Node = newNode;

        if (this.Root == null)//Tree ise empty
            this.Root = newNode;
        else
        {
            if (value.Smaller(before.Data))
                before.LeftNode = newNode;
            else
                before.RightNode = newNode;
        }
 
        return true;
    }
 
    public Node<T> Find(T value)
    {
        return this.Find(value, this.Root);            
    }
 
    public void Remove(T value)
    {
        this.Root = Remove(this.Root, value);
    }
 
    private Node<T> Remove(Node<T> parent, T key)
    {
        if (parent == null) return parent;
 
        if (key.Smaller(parent.Data)) parent.LeftNode = Remove(parent.LeftNode, key); else if (key.Greater(parent.Data))
            parent.RightNode = Remove(parent.RightNode, key);
 
        // if value is same as parent's value, then this is the node to be deleted  
        else
        {
            // node with only one child or no child  
            if (parent.LeftNode == null)
                return parent.RightNode;
            else if (parent.RightNode == null)
                return parent.LeftNode;
 
            // node with two children: Get the inorder successor (smallest in the right subtree)  
            parent.Data = MinValue(parent.RightNode);
 
            // Delete the inorder successor  
            parent.RightNode = Remove(parent.RightNode, parent.Data);
        }
 
        return parent;
    }
 
    private T MinValue(Node<T> node)
    {
        T minv = node.Data;
 
        while (node.LeftNode != null)
        {
            minv = node.LeftNode.Data;
            node = node.LeftNode;
        }
 
        return minv;
    }
 
    private Node<T> Find(T value, Node<T> parent)
    {
        if (parent != null)
        {
            if (value == parent.Data) return parent;
            if (value.Smaller(parent.Data))
                return Find(value, parent.LeftNode);
            else
                return Find(value, parent.RightNode);
        }
 
        return null;
    }
 
    public int GetTreeDepth()
    {
        return this.GetTreeDepth(this.Root);
    }
 
    private int GetTreeDepth(Node<T> parent)
    {
        return parent == null ? 0 : Math.Max(GetTreeDepth(parent.LeftNode), GetTreeDepth(parent.RightNode)) + 1;
    }
 
    public void TraversePreOrder(Node<T> parent)
    {
        if (parent != null)
        {
            DoPreOrder(parent);
            TraversePreOrder(parent.LeftNode);
            TraversePreOrder(parent.RightNode);
        }
    }

    protected virtual void DoPreOrder(Node<T> parent)
    {
    }

    public void TraverseInOrder(Node<T> parent)
    {
        if (parent != null)
        {
            TraverseInOrder(parent.LeftNode);
            DoInOrder(parent);
            TraverseInOrder(parent.RightNode);
        }
    }

    protected virtual void DoInOrder(Node<T> parent)
    {
    }

    public void TraversePostOrder(Node<T> parent)
    {
        if (parent != null)
        {
            TraversePostOrder(parent.LeftNode);
            TraversePostOrder(parent.RightNode);
            DoPostOrder(parent);
        }
    }

    protected virtual void DoPostOrder(Node<T> parent)
    {
    }
    }
}