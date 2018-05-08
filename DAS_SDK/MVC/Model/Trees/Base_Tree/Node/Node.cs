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

        public int UpdateLevels(Node<T> node, int level = 0)
        {
            node.Level = level++;
            foreach (var cNode in node.ConnectedNodes)
            {
                if (node.ParentNode == null) { UpdateLevels(cNode, level++); }
                else
                {
                    if (cNode != ParentNode && cNode.Level == 0 && node.ParentNode.Level + 1 == node.Level)
                    {
                        UpdateLevels(cNode, level++);
                    }
                    else if (node as Leaf<T> != null)
                    {
                        //this.Level = level++;
                        return 1;
                    }
                }
            }
            return 0;
        }
    }
}
