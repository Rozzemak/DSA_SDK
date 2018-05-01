using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAS_SDK.MVC.Model.Trees.Base_Tree
{
    class Branch<T> where T : IComparable
    {
        public BranchMasterNode<T> BranchMasterNode;
        public List<Node<T>> BranchNodes;
        public List<Leaf<T>> Leaves = new List<Leaf<T>>();

        public Branch(List<Node<T>> branchNodes, BranchMasterNode<T> branchMasterNode)
        {
            this.BranchNodes = branchNodes;
            this.BranchMasterNode = branchMasterNode;
            UpdateBranch();
        }

        private void FindLeaves()
        {
            for (int i = 0; i > BranchNodes.Count -1; i++)
            {
                if (BranchNodes[i].ParentNode != null &&
                    BranchNodes[i].GetType() != typeof(Root<T>) && BranchNodes[i].GetType() != typeof(BranchMasterNode<T>) &&
                    BranchNodes[i].ConnectedNodes.Count == 1)
                {
                    BranchNodes[i] = new Leaf<T>(BranchNodes[i].Content, BranchNodes[i].ParentNode);
                    Leaves.Add(BranchNodes[i] as Leaf<T>);
                }
            }
        }

        public void UpdateBranch()
        {
            if (!this.BranchNodes.Contains(this.BranchMasterNode))
            {
                throw new Exception("Invalid Branch" +
                    "Branch is supposed to have one MasterNode");
            }
            FindLeaves();
        }
    }
}
