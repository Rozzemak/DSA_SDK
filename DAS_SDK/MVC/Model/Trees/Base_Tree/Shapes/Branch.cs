using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAS_SDK.MVC.Model.Trees.Base_Tree.Node;

namespace DAS_SDK.MVC.Model.Trees.Base_Tree.Shapes
{
    class Branch<T> where T : IComparable
    {
        public BranchMasterNode<T> BranchMasterNode;
        public List<Node<T>> BranchNodes = new List<Node<T>>();
        public List<Leaf<T>> Leaves = new List<Leaf<T>>();
        public List<List<Node<T>>> LeveledListsOfNodes = new List<List<Node<T>>>(20);

        public Branch(List<Node<T>> branchNodes, BranchMasterNode<T> branchMasterNode)
        {
            this.BranchMasterNode = branchMasterNode;
            foreach (var item in branchNodes)
            {
                this.BranchNodes.Add(item);
            }
            for (int i = 0; i < 64; i++)
            {
                LeveledListsOfNodes.Add(new List<Node<T>>());
            }
            UpdateBranch();
        }

        private void FindLeaves()
        {
            for (int i = 0; i < BranchNodes.Count; i++)
            {
                if (//BranchNodes[i].ParentNode.ConnectedNodes.Count != 0 &&
                    BranchNodes[i].GetType() != typeof(Root<T>) && BranchNodes[i].GetType() != typeof(BranchMasterNode<T>) &&
                    BranchNodes[i].ConnectedNodes.Count == 1
                    )
                {
                    BranchNodes[i] = new Leaf<T>(BranchNodes[i].Content, BranchNodes[i].ConnectedNodes[0]);
                    Leaves.Add(BranchNodes[i] as Leaf<T>);
                }
            }
        }

        public void CreateLeveledList()
        {
            foreach (var item in BranchNodes)
            {
                LeveledListsOfNodes.ElementAt(item.Level).Add(item);
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
            Leaves.Clear();
            FindLeaves();
            CreateLeveledList();
        }
    }
}
