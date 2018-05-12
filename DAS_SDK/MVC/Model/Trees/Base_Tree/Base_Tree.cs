using DAS_SDK.MVC.Model.Sorts.Base_Sort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAS_SDK.MVC.Model.Trees.Base_Tree.Node;
using DAS_SDK.MVC.Model.Trees.Base_Tree.Shapes;

namespace DAS_SDK.MVC.Model.Trees.Base_Tree
{
    class Base_Tree<T> where T : IComparable
    {

        public Root<T> Root;
        public List<BranchMasterNode<T>> BranchMasterNodes;
        public List<Branch<T>> Branches;
        public List<List<Leaf<T>>> Leaves = new List<List<Leaf<T>>>();

        public Base_Tree(List<Branch<T>> branches, Root<T> root = null)
        {
            this.Root = root;
            this.Branches = branches;
            UpdateTree();
        }

        /// <summary>
        /// Updates tree to init leaves, check and update root if its null.
        /// </summary>
        public void UpdateTree()
        {
            if (Root == null)
            {
                // Iterate trough every branch then node and find root if there is not one.
                foreach (var branch in Branches)
                {
                    /* Depreceated?
                    foreach (var node in branch.BranchNodes)
                    {
                        if (node.GetType() == typeof(Root<T>))
                        {
                            // If root has already been found, break a tree definition a little and write temporary exception. (Will be fixed someday)
                            if (Root != null) throw new Exception("There cannot more than one Root in Tree.(For now)");
                            Root = node as Root<T>;
                        }
                    }
                    */
                    foreach (var cNode in branch.BranchMasterNode.ConnectedNodes)
                    {
                        if (cNode.GetType() == typeof(Root<T>))
                        {
                            // If root has already been found, break a tree definition a little and write temporary exception. (Will be fixed someday)
                            if (Root != null) throw new Exception("There cannot more than one Root in Tree.(For now)");
                            Root = cNode as Root<T>;
                        }
                    }
                }
            }
            // If there is no root... there is no tree.
            if (Root == null) throw new Exception("Tree has no Root node. One and only one node has to be in branchNodes collection.");
            Leaves.Clear();
            foreach (var branch in Branches)
            {
                branch.UpdateBranch();
                Leaves.Add(branch.Leaves);
            }
            UpdateLevels();
        }

        public void UpdateLevels(int initLevel = 0)
        {
            Root.UpdateLevels(Root, 0);   
            /*
            foreach (var branch in Branches)
            {
                foreach (var branchNode in branch.BranchNodes)
                {
                    switch (branchNode.GetType())
                    {
                        case Type nodeTcheck when branchNode.GetType() == typeof(Root<T>):
                            branchNode.Level = 0;
                            break;
                        case Type nodeTcheck when branchNode.GetType() == typeof(Node<T>):
                            branchNode.Level = 1;
                            bool IsItRoot = false;
                            while (IsItRoot)
                            {
                                foreach (var cNode in branchNode.ConnectedNodes)
                                {

                                }
                            }
                            break;
                        case Type nodeTcheck when branchNode.GetType() == typeof(Leaf<T>):
                            this.color = Brushes.LightGreen;
                            break;
                        case Type nodeTcheck when branchNode.GetType() == typeof(BranchMasterNode<T>):
                            this.color = Brushes.Yellow;
                            break;
                    }
                }
               
            } */
        }


    }


}
