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
        private Node<T> nodeBranch;
        private int Level;

        public Node(T type, Node<T> nodeBranch){
            this.Content = type;
            this.nodeBranch = nodeBranch;
            UpdateLevel(this);
        }

        public void UpdateLevel(Node<T> node)
        {
            while(node.nodeBranch != null && node.Level >= node.nodeBranch.Level)
            {
                node.nodeBranch.Level++;
                UpdateLevel(node.nodeBranch);
            }
        }

        public bool IsInOrder(Node<T> node, Order_Enum order_Enum = Order_Enum.ASCENDING)
        {
            switch (order_Enum)
            {
                case Order_Enum.ASCENDING:
                    if (node.nodeBranch != null)
                    {   
                        if (node.Content.CompareTo(node.nodeBranch.Content) <= 0)
                        {
                            IsInOrder(node.nodeBranch);
                        }
                        else return false;
                    }
                    break;
                case Order_Enum.DESCENDING:
                    if (node.nodeBranch != null)
                    {
                        if (node.Content.CompareTo(node.nodeBranch.Content) >= 0)
                        {
                            IsInOrder(node.nodeBranch);
                        }
                        else return false;
                    }
                    break;
                case Order_Enum.CANNOT_BE_SPECIFIED:
                    break;
                case Order_Enum.ASC_LENGTH:
                    break;
                case Order_Enum.DESC_LENGTH:
                    break;
            }
            return true;
        }
    }
}
