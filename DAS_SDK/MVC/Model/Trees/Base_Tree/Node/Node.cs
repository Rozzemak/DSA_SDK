using DAS_SDK.MVC.Model.Sorts.Base_Sort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAS_SDK.MVC.Model.Trees.Base_Tree.Node
{
    class Node<T> where T : IComparable
    {
        public T Content = default(T);
        public List<Node<T>> ConnectedNodes = new List<Node<T>>();
        public Node<T> ParentNode;
        public int Level;

        public Node(T content, List<Node<T>> connectedNodes)
        {
            this.Content = content;
            if (connectedNodes != null)
                this.ConnectedNodes = connectedNodes;
            foreach (var item in this.ConnectedNodes)
            {
                item.ConnectedNodes.Add(this);
            }
        }


        public Node(T content, Node<T> parentNode, List<Node<T>> connectedNodes)
        {
            this.Content = content;
            this.ParentNode = parentNode;
            if (connectedNodes != null)
                this.ConnectedNodes = connectedNodes;
            foreach (var item in this.ConnectedNodes)
            {
                item.ConnectedNodes.Add(this);
            }
        }

        /// <summary>
        /// Updates level recursively to each node from specified note ignoring parentNode direction.
        /// Recursive update affects all tree hierarchic children nodes.
        /// </summary>
        /// <param name="node">Node from which you want to update levels.</param>
        /// <param name="level">Recursive pass param, use 0 if root, else use current node level</param>
        /// <param name="rootFound">If root has been found by recursion pass.</param>
        public void UpdateLevels(Node<T> node, int level, bool rootFound = false)
        {
            // This alg was created merely by think/try/observe method. May be very laggy! ...
            //node.Level = level++; => stupid to update level without any check, but leave it here as comment!
            //So I would stop thiking about this.
            foreach (Node<T> cNode in node.ConnectedNodes)
            {
                if (node as Root<T> != null) { rootFound = true; UpdateLevels(cNode, level++, true); }
                else if (rootFound)
                {
                    if (node.Level == 0) node.Level = level++;
                    //  && node.ParentNode.Level + 1 == node.Level /-> rethink this?? or is it done?? ..
                    // Testing required!!
                    if (cNode != node.ParentNode && cNode.Level == 0)
                    {
                        UpdateLevels(cNode, level++,true);
                    }
                    if ((cNode as Leaf<T>) != null && (cNode as Leaf<T>).Level == 0)
                    {
                        (cNode as Leaf<T>).Level = (cNode as Leaf<T>).Parent.Level + 1;
                    }
                } else
                {
                    // If not initiated from root, impl needed here.
                }
            }
        }
    }
}
