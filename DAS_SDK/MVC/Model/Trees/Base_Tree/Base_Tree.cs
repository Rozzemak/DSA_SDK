using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAS_SDK.MVC.Model.Trees.Base_Tree
{
    class Base_Tree<T> where T : IComparable
    {
        Queue<Node<T>> nodes;

    }
}
