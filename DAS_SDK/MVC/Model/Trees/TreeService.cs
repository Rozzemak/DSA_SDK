using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAS_SDK.MVC.Model.Interfaces;
using DAS_SDK.MVC.Model.Trees.Base_Tree;
using DAS_SDK.MVC.Model.Trees.Base_Tree.Node;

namespace DAS_SDK.MVC.Model.Trees
{
    class TreeService<T> : IServise<Base_Tree<T>> where T : IComparable
    {
        List<Base_Tree<T>> Trees = new List<Base_Tree<T>>();

        public TreeService()
        {

        }

        public void AddToCollection(Base_Tree<T> tree)
        {
            Trees.Add(tree);
        }

        public void RemoveFromCollection(Base_Tree<T> tree)
        {
            Trees.Remove(tree);
        }

        public List<Base_Tree<T>> GetCollection()
        {
            return this.Trees;
        }

    }
}
