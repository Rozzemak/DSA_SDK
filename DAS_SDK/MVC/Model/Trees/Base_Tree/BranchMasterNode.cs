using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAS_SDK.MVC.Model.Trees.Base_Tree
{
    class BranchMasterNode<T> : Node<T> where T : IComparable
    {
        Root<T> root;

        public BranchMasterNode(T content, List<Node<T>> connectedNodes)
            : base(content, connectedNodes)
        {
            // Only if list is empty, we can check for count. (Maybe better negation ?)
            if (connectedNodes != null && connectedNodes.Count != 0)
            {
                foreach (var node in connectedNodes)
                {
                    if (node as Root<T> != null) root = node as Root<T>;
                    break;
                }
                if (root == null) throw new Exception("Root has to be in connected nodes (in Branch)");
            }
            else throw new Exception("Use leaf instead," +
                " BranchMasterNode is linker between root and branches(notLeaves).");
        }
    }
}
