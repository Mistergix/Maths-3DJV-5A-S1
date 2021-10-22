namespace ___PROJET.Scripts.Voronoi.Fortune_Voronoi.BST
{
    public abstract class NodeData
    {
        public NodeBase Node { get; set; }
        
        public abstract bool Smaller(NodeData other);
        public abstract bool Greater(NodeData other);
    }
}