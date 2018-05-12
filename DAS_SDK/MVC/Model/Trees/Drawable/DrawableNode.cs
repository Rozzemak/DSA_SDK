using DAS_SDK.MVC.Model.Front_END;
using DAS_SDK.MVC.Model.Interfaces;
using DAS_SDK.MVC.Model.Trees.Base_Tree.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DAS_SDK.MVC.Model.Trees.Drawable
{
    enum NodeTypes {
        Root
    }


    class DrawableNode<T> where T : IComparable
    {
        public float X;
        public float Y;
        public float Z;
        public int Width;
        public int Height;
        public Brush color = Brushes.AliceBlue;
        public Node<T> Node { get; private set; }
        Tree_Front_END<T> tree_Front_END;
        public UIElement _UIElement;
        public DrawableNode<T> drawableParentNode;

        public DrawableNode(Node<T> node)
        {
            this.Node = node;
            ColorCheck(node);
        }

        public DrawableNode(Node<T> node, float x, float y, float z, int width, int height, DrawableNode<T> drawableParentNode = null)
        {
            this.Node = node;
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Width = width;
            this.Height = height;
            ColorCheck(node);
            this.drawableParentNode = drawableParentNode;
        }

        public void ColorCheck(Node<T> node)
        {
            switch (node.GetType())
            {
                case Type nodeTcheck when node.GetType() == typeof(Root<T>):
                    this.color = Brushes.Red;
                    break;
                case Type nodeTcheck when node.GetType() == typeof(Node<T>):
                    this.color = Brushes.AliceBlue;
                    break;
                case Type nodeTcheck when node.GetType() == typeof(Leaf<T>):
                    this.color = Brushes.LightGreen;
                    break;
                case Type nodeTcheck when node.GetType() == typeof(BranchMasterNode<T>):
                    this.color = Brushes.Yellow;
                    break;
            }
        }

    }
}
