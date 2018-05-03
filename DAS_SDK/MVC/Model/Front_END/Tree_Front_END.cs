using DAS_SDK.MVC.Enums;
using DAS_SDK.MVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DAS_SDK;
using System.Threading;
using DAS_SDK.MVC.Model.Debug;
using DAS_SDK.MVC.Model.Trees.Base_Tree.Node;
using System.Windows.Markup;
using DAS_SDK.MVC.Model.Trees;

namespace DAS_SDK.MVC.Model.Front_END
{
    class Config_Tree_Front_END
    {
        public Window RenderWindow;
        public Grid MainGrid = new Grid() {
            Language = XmlLanguage.GetLanguage("en"),
            Visibility = Visibility.Visible,
            MinWidth = 640,
            MinHeight = 480,
            Background = Brushes.Red        
        };
        public Grid ScrollGrid = new Grid()
        {
            Language = XmlLanguage.GetLanguage("en"),
            Visibility = Visibility.Visible,
            MinWidth = 640,
            MinHeight = 480,
            Background = Brushes.WhiteSmoke
        };
        public List<UIElement> UiElements_Tree = new List<UIElement>();
        public ScrollViewer ScrollViewer = new ScrollViewer() {
            
        };

        public Config_Tree_Front_END(Window renderWin)
        {
            this.RenderWindow = renderWin;
            SetContent();
        }

        private void SetContent()
        {
            RenderWindow.Content = MainGrid;
            ScrollViewer.Content = ScrollGrid;
            MainGrid.Children.Add(ScrollViewer);
        }

        public void AddContent(UIElement uIElement)
        {
            ScrollGrid.Children.Add(uIElement);
        }
    }

    class Tree_Front_END<T> : Base_Front_END where T : IComparable
    {
        Config_Tree_Front_END config;
        TreeService<T> TreeService;

        Window RenderWindow = new Window() {
            Width = 800,
            Height = 600,
            MinWidth = 640,
            MinHeight = 520,
            WindowStartupLocation = WindowStartupLocation.Manual
        };
        Tuple<Node<T>, Vector, Node<T>> Edges;

        public Tree_Front_END(List<Window> windowRefList, Thread _UI_Thread, Base_Debug debug, TreeService<T> treeService)
            : base(windowRefList, _UI_Thread, debug)
        {
            RenderWindow.Left = WindowRefList[0].Left + WindowRefList[0].Width;
            RenderWindow.Top = Math.Abs(WindowRefList[0].Top - 200);
            // Just for controller visual edits maybe;
            this.WindowRefList.Add(RenderWindow);
            config = new Config_Tree_Front_END(RenderWindow);
            this.TreeService = treeService;
            Init();
            config.RenderWindow.Show();
        }


        private void Init()
        {
            if (TreeService.Trees.Count == 0) throw new Exception("No Trees");
            // config.AddContent(btn);
            UpdateTrees();
        }

        public void UpdateTrees()
        {
            int lastNodeX = 0;
            int lastNodeY = 0;
            int temp = 0;
            Matrix matrix = new Matrix();
            foreach (var tree in TreeService.Trees)
            {
                AddNodeToDraw(tree.Root, 40, Brushes.Crimson, lastNodeX, lastNodeY);
                //Debug.AddMessage<object>(new Message<object>("Graphical Node Added"));
                foreach (var branch in tree.Branches) {
                    AddNodeToDraw(branch.BranchMasterNode, 35, Brushes.Yellow, lastNodeX, lastNodeY = 40);
                    temp = lastNodeY;
                    foreach (var node in branch.BranchNodes)
                    {
                        //lastNodeX *= xOffset++;
                        if (node as Leaf<T> != null)
                        {
                            AddNodeToDraw(node, 20, Brushes.LightGreen, lastNodeX, lastNodeY += 25);
                            lastNodeY = temp;
                            lastNodeX += 30;
                        } 
                        else
                        {
                            AddNodeToDraw(node, 25, Brushes.AliceBlue, lastNodeX, lastNodeY += 35);
                        }                       
                    }
                   
                }
            }
        }

        private void AddNodeToDraw(Node<T> node, int size, Brush brush, int lastNodeX, int lastNodeY)
        {
            config.AddContent(new Button()
            {
                Width = size,
                Height = size,
                Content = node.Content.ToString(),
                Visibility = Visibility.Visible,
                Background = brush,
                Margin = new Thickness(1, 1, 1, 1),
                RenderTransform = new TranslateTransform(lastNodeX, lastNodeY)
            });
            lastNodeX += size;
        }
    }
}
