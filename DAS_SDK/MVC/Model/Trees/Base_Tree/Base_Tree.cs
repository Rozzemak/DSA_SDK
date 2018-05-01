using DAS_SDK.MVC.Model.Sorts.Base_Sort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAS_SDK.MVC.Model.Trees.Base_Tree
{
    class Base_Tree<T> where T : IComparable
    {
       
        public Root<T> Root;
        public List<Node<T>> SlaveNodes;
        /*
       public Base_Tree(Root<T> root){
           this.Root = root;
           Root.UpdateLevel(Root);
       }

       public Base_Tree(Root<T> root, List<List<Node<T>>> SlaveNodes)
       {
           this.Root = root;
           root.BranchMasterNodes = SlaveNodes;
       }

       public void UpdateLevel(Node<T> node)
       {
           foreach (var _node in NodeBranches)
           {
               while (node.ConnectedNodes != null && node.NodeBranches.Count != 0 && node.Level >= _node.Level && node.Level == NodeBranches.Count)
               {
                   _node.Level++;
                   UpdateLevel(_node);
               }
           }
       }




       public bool IsInOrder(Node<T> node, Order_Enum order_Enum = Order_Enum.ASCENDING)
       {
           switch (order_Enum)
           {
               case Order_Enum.ASCENDING:
                   foreach (var _node in NodeBranches)
                   {
                       if (node.ConnectedNodes != null && node.NodeBranches.Count != 0)
                       {
                           if (node.Content.CompareTo(node.NodeBranches.Content) <= 0)
                           {
                               IsInOrder(node.ConnectedNodes);
                           }
                           else return false;
                       }
                   }
                   break;
               case Order_Enum.DESCENDING:
                   if (node.ConnectedNodes != null && node.NodeBranches.Count != 0)
                   {
                       if (node.Content.CompareTo(node.NodeBranches.Content) >= 0)
                       {
                           IsInOrder(node.ConnectedNodes);
                       }
                       else return false;
                   }
                   break;
               case Order_Enum.CANNOT_BE_SPECIFIED:
                   throw new Exception("Could not be Impl.");
               case Order_Enum.ASC_LENGTH:
                   throw new Exception("Could not be Impl(maybe).");
               case Order_Enum.DESC_LENGTH:
                   throw new Exception("Could not be Impl(maybe).");
           }
           return true;
       }
       */
    }
}
