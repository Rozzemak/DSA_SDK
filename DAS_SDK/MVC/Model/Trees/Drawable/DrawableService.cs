using DAS_SDK.MVC.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAS_SDK.MVC.Model.Trees.Drawable
{
    class DrawableService<T> : IServise<DrawableNode<T>> where T : IComparable
    {
        List<DrawableNode<T>> DrawableNodes = new List<DrawableNode<T>>();
        List<List<DrawableNode<T>>> OrderedDrawableNodes = new List<List<DrawableNode<T>>>(64);

        public DrawableService()
        {
            for (int i = 0; i < 64; i++)
            {
                OrderedDrawableNodes.Add(new List<DrawableNode<T>>());
            }
        }

        public void AddToCollection(DrawableNode<T> type)
        {
            DrawableNodes.Add(type);
        }

        public void RemoveFromCollection(DrawableNode<T> type)
        {
            DrawableNodes.Remove(type);
        }

        public List<DrawableNode<T>> GetCollection()
        {
            return this.DrawableNodes;
        }

        public List<List<DrawableNode<T>>> GetDrawablesOrderedByConnectionAndLevel()
        {
            foreach (var Dnode in GetCollection())
            {
                if (!OrderedDrawableNodes[Dnode.Node.Level].Contains(Dnode))
                {
                    OrderedDrawableNodes[Dnode.Node.Level].Add(Dnode);
                }
            }
            return this.OrderedDrawableNodes;
        }
    }
}
