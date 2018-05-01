using DAS_SDK.MVC.Model.Sorts.Base_Sort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAS_SDK.MVC.Model.Trees.Base_Tree
{
    class Node<T> where T : IComparable
    {
        public T Content = default(T);
        public List<Node<T>> ConnectedNodes;
        public Node<T> ParentNode;
        public int Level;

        public Node(T content, List<Node<T>> connectedNodes)
        {
            this.Content = content;
            this.ConnectedNodes = connectedNodes;
        }


        public Node(T content, Node<T> parentNode, List<Node<T>> connectedNodes)
        {
            this.Content = content;
            this.ParentNode = parentNode;
            this.ConnectedNodes = connectedNodes;
        }

    }
}
