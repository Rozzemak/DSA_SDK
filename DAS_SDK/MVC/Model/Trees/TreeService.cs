using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAS_SDK.MVC.Model.Trees.Base_Tree;
using DAS_SDK.MVC.Model.Trees.Base_Tree.Node;

namespace DAS_SDK.MVC.Model.Trees
{
    class TreeService<T> where T : IComparable
    {
        public List<Base_Tree<T>> Trees = new List<Base_Tree<T>>();

        public TreeService()
        {

        }

        public void AddTree(Base_Tree<T> tree)
        {
            Trees.Add(tree);
        }


    }
}
