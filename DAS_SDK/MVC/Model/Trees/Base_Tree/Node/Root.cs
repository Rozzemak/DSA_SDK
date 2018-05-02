using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAS_SDK.MVC.Model.Trees.Base_Tree
{
    class Root<T> : Node<T> where T : IComparable
    {
        public List<BranchMasterNode<T>> BranchMasterNodes = new List<BranchMasterNode<T>>();

        public Root(T content, List<Node<T>> nodeBranches = null) 
            : base(content, null , null)
        {
            // Root without branches,.. could be.
        }

        /*
         * Since BranchMasterNode is a linker between nodes of type node and root, an actual instance of root has to already exist.
         * Not too transparent to do anything with it here. Maybe rework branchMasterNode ?? 
        public Root(T content, List<BranchMasterNode<T>> branchMasterNodes, List<Node<T>> connectedNodes = null)
           : base(content, null , connectedNodes)
        {
            this.BranchMasterNodes = branchMasterNodes;
        }
        */
    }
}
