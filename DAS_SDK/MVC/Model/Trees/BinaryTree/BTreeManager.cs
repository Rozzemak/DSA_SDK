using DAS_SDK.MVC.Model.Debug;
using DAS_SDK.MVC.Model.Front_END;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DAS_SDK.MVC.Model.Trees.BinaryTree
{
    class BTreeManager<T> where T : IComparable
    {
        Button AddToTreeBtn;
        Button RemoveFromTreeBtn;
        protected Base_Debug _Debug;
        Random random = new Random();
        Sort_Front_END sort_Front_END;
        public BinaryTree<int> BinaryTree = new BinaryTree<int>() { TraversalOrder = BinaryTree<int>.TraversalMode.InOrder };

        public BTreeManager(Base_Debug debug, Sort_Front_END sort_Front_END)
        {
            this._Debug = debug;
            this.sort_Front_END = sort_Front_END;
            InitTreeFrontEndOperations();
            Init();
        }

        private void InitTreeFrontEndOperations()
        {
            Dispatcher.FromThread(sort_Front_END.UI_Thread).Invoke(() =>
            {
                AddToTreeBtn = new Button
                {
                    Name = "AddToTreeBtn",
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Content = "AddToTreeBtn"
                };
                RemoveFromTreeBtn = new Button
                {
                    Name = "RemoveFromTreeBtn",
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Content = "RemoveFromTreeBtn"
                };
                sort_Front_END.StackPanel.Children.Add(AddToTreeBtn);
                sort_Front_END.StackPanel.Children.Add(RemoveFromTreeBtn);
                AddToTreeBtn.Click += AddToTreeBtn_Click;
                RemoveFromTreeBtn.Click += RemoveFromTreeBtn_Click;
            });     
        }

        private void RemoveFromTreeBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(BinaryTree.Count != 0)
            RemoveFromTree(BinaryTree.ElementAt(BinaryTree.Count/2));
            WriteTree();
        }

        private void AddToTreeBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AddToTree(random.Next(10));
            WriteTree();
        }

        public void Init(int i = 10000)
        {
            _Init(i);  
        }

        private void _Init(int nodeCount = 10000)
        {
            BinaryTree = new BinaryTree<int>();
            BinaryTree.Add(nodeCount/2);
            for (int i = 0; i < nodeCount; i++)
            {
                BinaryTree.Add(random.Next(nodeCount));
            }
        }

        public void AddToTree(int content)
        {
            BinaryTree.Add(content);
            _Debug.AddMessage<object>(new Message<object>("Node:[" + content + "] Was added!"));
        }

        public void RemoveFromTree(int content)
        {
            BinaryTree.Remove(content);
            _Debug.AddMessage<object>(new Message<object>("Node:[" + content + "] Was removed!"));
        }

        public BinaryTreeNode<T> FindNode(int i)
        {
            var temp = BinaryTree.Find(i);
            if(temp != null)
            _Debug.AddMessage<object>(new Message<object>("Node:[" + temp.Value + "] Was found! Iterations[" + BinaryTree.Iterations + "]"));
            else
            {
                _Debug.AddMessage<object>(new Message<object>("Node:[" + i + "] Was Not found! Iterations[" + BinaryTree.Iterations + "]"));
            }
            return temp as BinaryTreeNode<T>;
        }

        public void WriteTree(BinaryTree<int>.TraversalMode traversalMode = BinaryTree<int>.TraversalMode.InOrder)
        {
            BinaryTree.TraversalOrder = traversalMode;
            if (BinaryTree.Count < 20)
            {
                _Debug.AddMessage<object>(new Message<object>("[" + "TREE" + "]," + Enum.GetName(typeof(BinaryTree<int>.TraversalMode), traversalMode), MessageType_ENUM.Event));
                foreach (int item in BinaryTree)
                {
                    _Debug.AddMessage<object>(new Message<object>("[" + item + "]"));
                }
                _Debug.AddMessage<object>(new Message<object>("[" + "TREE" + "]," + Enum.GetName(typeof(BinaryTree<int>.TraversalMode), traversalMode), MessageType_ENUM.Event));
            }

        }
    }
}
