using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAS_SDK.MVC.Model.Trees.Base_Tree.Node;

namespace DAS_SDK.MVC.Model.Trees.Base_Tree.Node
{
    class Leaf<T> : Node<T> where T : IComparable
    {
        public Node<T> Parent;

        /// <summary>
        /// Leaf as Node without breanches, ... 
        /// </summary>
        /// <param name="content">Whatewer you want in this node.</param>
        /// <param name="parent">Parent node of leaf. Could be root or node.</param>
        /// <param name="nodeBranches">Will be always null, req by parent, but leaves dont have children.</param>
        public Leaf(T content, Node<T> parent , List<Node<T>> nodeBranches = null)
            : base(content, nodeBranches = null)
        {
            if (parent == null) throw new Exception("Leaf has to have parent node!");
            this.Parent = parent;
            Parent.ConnectedNodes.Add(this);

        }
    }
}
