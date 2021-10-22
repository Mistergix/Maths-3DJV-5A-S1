namespace ___PROJET.Scripts.Voronoi.Fortune_Voronoi.BST
{
    public abstract class NodeBase
    {
        public abstract NodeBase Left { get; set; }
        public abstract NodeBase Right { get; set; }
        public abstract bool IsLeaf { get; }
    }
}