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
        public List<Node<T>> BranchNodes = new List<Node<T>>();
        public List<Leaf<T>> Leaves = new List<Leaf<T>>();

        public Branch(List<Node<T>> branchNodes, BranchMasterNode<T> branchMasterNode)
        {
            this.BranchMasterNode = branchMasterNode;
            foreach (var item in branchNodes)
            {
                this.BranchNodes.Add(item);
            }
            UpdateBranch();
        }

        private void FindLeaves()
        {
            for (int i = 0; i < BranchNodes.Count; i++)
            {
                if (//BranchNodes[i].ParentNode.ConnectedNodes.Count != 0 &&
                    BranchNodes[i].GetType() != typeof(Root<T>) && BranchNodes[i].GetType() != typeof(BranchMasterNode<T>) 
                    // BranchNodes[i].ConnectedNodes.Count == 1
                    )
                {
                    BranchNodes[i] = new Leaf<T>(BranchNodes[i].Content, BranchNodes[i].ParentNode);
                    Leaves.Add(BranchNodes[i] as Leaf<T>);
                }
            }
        }

        public void UpdateBranch()
        {
            bool contains = false;
            foreach (var branchNode in BranchNodes)
            {
                // If connected nodes count is smaller than 1, its not even a Leaf!
                if (branchNode.ConnectedNodes.Count > 1)
                {
                    if (branchNode.ConnectedNodes.Contains(this.BranchMasterNode))
                    {
                        contains = true;
                        break;
                    }
                }
            }
            if (!contains) throw new Exception("Invalid Branch" +
                                 "Branch is supposed to have one MasterNode, and has to be continuous");
            FindLeaves();
        }
    }
}
