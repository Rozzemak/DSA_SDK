using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAS_SDK.MVC.Model.Trees.Base_Tree
{
    class Leaf<T> : Node<T> where T : IComparable
    {
        Node<T> Parent;

        /// <summary>
        /// Leaf as Node without breanches, ... 
        /// </summary>
        /// <param name="content">Whatewer you want in this node.</param>
        /// <param name="parent">Parent node of leaf. Could be root or node.</param>
        /// <param name="nodeBranches">Will be always null, req by parent, but leaves dont have children.</param>
        public Leaf(T content, Node<T> parent , List<Node<T>> nodeBranches = null)
            : base(content, nodeBranches = null)
        {
            this.Parent = parent;

        }
    }
}
