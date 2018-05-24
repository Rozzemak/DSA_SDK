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
using DAS_SDK.MVC.Model.Trees.Drawable;
using System.Windows.Threading;

namespace DAS_SDK.MVC.Model.Front_END
{
    class Config_Tree_Front_END
    {
        public Window RenderWindow;
        public Grid MainGrid = new Grid()
        {
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
        public Canvas BackgroundCanvas = new Canvas()
        {
            MinWidth = 640,
            MinHeight = 480,
            Background = Brushes.WhiteSmoke
        };
        public List<UIElement> UiElements_Tree = new List<UIElement>();
        public Button TreeInitButton = new Button() {
            Name = "TreeInitButton",
            Content = "Init&Show_Tree",
            Width = 85,
            Height = 35,
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(10, 0, 0, 0)
        };
        public ScrollViewer ScrollViewer = new ScrollViewer()
        {

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
            ScrollGrid.Children.Add(BackgroundCanvas);
            MainGrid.Children.Add(ScrollViewer);
        }
        public void AddContent(UIElement uIElement)
        {
            ScrollGrid.Children.Add(uIElement);
        }
    }

    static class Ext
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
            => self.Select((item, index) => (item, index));
    }


    class Tree_Front_END<T> : Base_Front_END where T : IComparable
    {
        /// <summary>
        /// This class would be enourmous.
        /// I had to keep config.UIelement separated.
        /// </summary>
        Config_Tree_Front_END config;
        TreeService<T> TreeService;
        DrawableService<T> DrawableService = new DrawableService<T>();
        List<UIElement> _UIElements = new List<UIElement>();

        Window RenderWindow = new Window()
        {
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
            this.WindowRefList = windowRefList;
            this.WindowRefList.Add(RenderWindow);
            config = new Config_Tree_Front_END(RenderWindow);
            this.TreeService = treeService;
            if (TreeService == null) throw new Exception("Tree service hase to be instantisied.");
            
            //Button
            ((Grid)windowRefList[0].Content).Children.Add(config.TreeInitButton);
            Grid.SetColumn(config.TreeInitButton, 1);
            config.TreeInitButton.Click += TreeInitButton_Click;

        }

        private void Init()
        {
            if (TreeService.GetCollection().Count == 0) throw new Exception("No Trees");
            // config.AddContent(btn);
            UpdateTreesToDraw();
            DrawTrees();
        }


        /// <summary>
        /// For each drawableNode, create new UIelement(button).
        /// </summary>
        private void DrawTrees()
        {
            foreach (var _drawableNode in DrawableService.GetCollection())
            {
                _drawableNode._UIElement = new Button()
                {
                    Width = _drawableNode.Width,
                    Height = _drawableNode.Height,
                    Content = _drawableNode.Node.Content.ToString(),
                    Visibility = Visibility.Visible,
                    Background = _drawableNode.color,
                    Margin = new Thickness(1, 1, 1, 1),
                    ToolTip = ("Parent: [" + WriteIfParentNotNull(_drawableNode) + "] \n"+
                    "Level: [" + _drawableNode.Node.Level + "]\n" +
                    "Children: [" + WriteAllChilrden(_drawableNode) + "]\n" +
                    "Content: [" + _drawableNode.Node.Content + "]\n"
                    )
                    // RenderTransform = new TranslateTransform(TreeDrawLevel(_drawableNode, index), _drawableNode.Height*3 * _drawableNode.Node.Level),
                };
                config.AddContent(_drawableNode._UIElement);
                //_drawableNode._UIElement.MouseLeftButtonDown += Btn_MouseLeftButtonDown;
                //if (_drawableNode._UIElement == null) throw new Exception("DNode has no UIElement");
                _drawableNode._UIElement.MouseDown += _UIElement_MouseDown; 
                
            }
            UpdateRenderPositions();
        }

        /// <summary>
        /// OnMouseBtn down quick impl, 
        /// Left_Click does not work,
        /// Quite few stuff you can do here, .. 
        /// (Maybe drag&drop to move is more sensible, 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _UIElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.AddMessage<object>(new Message<object>(sender.GetHashCode()));
            (sender as Button).Visibility = Visibility.Hidden;
            foreach (var item in DrawableService.GetCollection())
            {            
                if (item._UIElement == sender as UIElement)
                {
                    foreach (var cNode in item.Node.ConnectedNodes)
                    {
                        foreach (var dNode2 in DrawableService.GetCollection())
                        {
                            if (dNode2.Node.Level > cNode.Level && (item._UIElement as Button) == sender as Button && dNode2.Node.Content.CompareTo(item.Node.Content) > 0)
                            {
                                dNode2._UIElement.Visibility = Visibility.Hidden;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Quick impl of Uielement tooltip init.
        /// Should be in drawable node, but time is of the essence.
        /// </summary>
        /// <param name="dNode"></param>
        /// <returns></returns>
        private string WriteIfParentNotNull(DrawableNode<T> dNode)
        {
            if ((dNode.Node as Root<T> != null || dNode.Node.ParentNode == null) && dNode.Node as Leaf<T> == null) return "";
            else
                if (dNode.Node as Leaf<T> == null) return dNode.Node.ParentNode.Content.ToString();
            else return (dNode.Node as Leaf<T>).Parent.Content.ToString();
        }
        
        /// <summary>
        /// Method for UIelement ToolTip init. 
        /// Quick impl, propably should be in drawable node class.
        /// like (Set tooltip or whatever...)
        /// </summary>
        /// <param name="dNode"></param>
        /// <returns></returns>
        private string WriteAllChilrden(DrawableNode<T> dNode)
        {
            string str = "";
            foreach (var item in dNode.Node.ConnectedNodes)
            {
                if((item != dNode.Node.ParentNode || item.ParentNode == null) && item as Leaf<T> == null)
                str += "["+item.Content+ "]\n";
            }
            return str;
        }

        /// <summary>
        /// Just subscribing to the Onload event, because, you can´t really change
        /// positions of UIelements until they have been fully loaded.
        /// For actual UpdatePos impl, go to the ScrollGrid_Loaded method;
        /// </summary>
        private void UpdateRenderPositions()
        {
            config.ScrollGrid.Loaded += ScrollGrid_Loaded;
        }

        /// <summary>
        /// You cant check for UI element(visual parent control) positions,
        /// until it has been fully loaded and drawed. Had to put it in this event, 
        /// to eliminate "no bound parent" exeption.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollGrid_Loaded(object sender, RoutedEventArgs e)
        {
            DrawableNode<T> tempLineNode = null;
            foreach (var (dNode, index) in DrawableService.GetCollection().WithIndex())
            {
                foreach (var trees in TreeService.GetCollection())
                {
                    if (trees.Root == dNode.Node)
                    {
                        dNode._UIElement.RenderTransform = new TranslateTransform(
                        (dNode.Width) * -Direction(dNode.Node.Level),
                        ((dNode.Height)) * (trees.Root.Level + 1));
                        tempLineNode = dNode;
                    }
                    foreach (var branch in trees.Branches)
                    {
                        if (branch.BranchMasterNode == dNode.Node)
                        {
                            dNode._UIElement.RenderTransform = new TranslateTransform(
                            (dNode.Width) * -Direction(dNode.Node.Level),
                            ((dNode.Height)) * (branch.BranchMasterNode.Level + 1));
                        }
                        foreach (var ListsofNodes in branch.LeveledListsOfNodes)
                        {
                            foreach (var (node, index2) in ListsofNodes.WithIndex())
                            {
                                if (node == dNode.Node)
                                {
                                    //dNode._UIElement.RenderTransform = new TranslateTransform(
                                    //((dNode.Width) * (index2)) - dNode.Width*2,
                                    //((dNode.Height)) * (node.Level+1));
                                    dNode._UIElement.RenderTransform = new TranslateTransform(
                                        (dNode.Width * (index)), dNode.Height * (node.Level + 1));
                                    //if (tempLineNode == null) 
                                    //DrawLine(tempLineNode, dNode);
                                    tempLineNode = dNode;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DrawLine(DrawableNode<T> from, DrawableNode<T> to)
        {
            Line line = new Line()
            {
                StrokeThickness = 2,
                Stroke = Brushes.Black,
                X1 = GetPosition(from._UIElement).X,
                Y1 = GetPosition(from._UIElement).X,
                X2 = GetPosition(to._UIElement).X,
                Y2 = GetPosition(to._UIElement).Y
            };
            config.BackgroundCanvas.Children.Add(line);
        }

        /// <summary>
        /// Bugged. Propably one of the uiElements containers has to be set up differently.
        /// </summary>
        /// <param name="uIElement"></param>
        /// <returns></returns>
        Point GetPosition(UIElement uIElement)
        {
            Point pnt = uIElement.TransformToAncestor(config.ScrollGrid).Transform(new Point(0, 0));
            return pnt;
        }

        /// <summary>
        /// Primitive % 2 usage
        /// </summary>
        /// <param name="level">Level of node</param>
        /// <returns>1 or -1</returns>
        private int Direction(int level)
        {
            if (level % 2 == 1) return -1;
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Basically filling the draw service here.
        /// (With DrawableNode here.)... not much thought put here.
        /// </summary>
        private void UpdateTreesToDraw()
        {
            DrawableService.GetCollection().Clear();
            int a = 25;
            foreach (var (tree, tIndex) in TreeService.GetCollection().WithIndex())
            {
                //a = 45;
                AddNodeToDraw(new DrawableNode<T>(tree.Root, 0, 0, 0, a, a));
                foreach (var (branch, bIndex) in tree.Branches.WithIndex())
                {
                    // a = 25;
                    AddNodeToDraw(new DrawableNode<T>(branch.BranchMasterNode, 0, 0, 0, a, a));

                    foreach (var (node, nIndex) in branch.BranchNodes.WithIndex())
                    {
                        if (node as Leaf<T> != null)
                        {
                            //a = 15;
                            AddNodeToDraw(new DrawableNode<T>(node, 0, 0, 0, a, a));
                        }
                        else
                        {
                            //a = 20;
                            AddNodeToDraw(new DrawableNode<T>(node, 0, 0, 0, a, a));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Depreceated
        /// </summary>
        /// <param name="dNode"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private int TreeDrawLevel(DrawableNode<T> dNode, int index)
        {
            //
            //if (dNode.Node.GetType() != typeof(BranchMasterNode<T>))
            //{
            //    if (dNode.Node.Level % 2 == 1)
            //    {
            //        if (dNode.Node.ParentNode != null && (dNode.Node.ParentNode.GetType() == typeof(Node<T>) || dNode.Node.ParentNode.GetType() == typeof(BranchMasterNode<T>)))
            //        { return ((int)(dNode.X * dNode.Z)); }
            //        else
            //            if (dNode.Node.GetType() == typeof(Leaf<T>))
            //            return (-(int)dNode.X);
            //        else
            //        {
            //            if (dNode.Node.GetType() == typeof(Root<T>))
            //                return (-(int)dNode.X);
            //            throw new Exception("Undefined render positon");
            //        }
            //    }
            //    else
            //    {
            //        if (dNode.Node.ParentNode != null && (dNode.Node.ParentNode.GetType() == typeof(Node<T>) || dNode.Node.ParentNode.GetType() == typeof(BranchMasterNode<T>)))
            //        { return (-(int)(dNode.X * dNode.Z)); }
            //        else
            //            if (dNode.Node.GetType() == typeof(Leaf<T>))
            //            return ((int)dNode.X);
            //        else
            //        {
            //            if (dNode.Node.GetType() == typeof(Root<T>))
            //                return ((int)dNode.X);
            //            throw new Exception("Undefined render positon");

            //        }
            //    }
            //}
            //else
            //{
            //    // Multiply this by branch id maybe modulo check and some correction.
            //    return ((int)dNode.X);
            //}
            return 0;
        }

        /// <summary>
        /// Fills DrawableServiceCollection with another node.
        /// </summary>
        /// <param name="drawableNode"></param>
        public void AddNodeToDraw(DrawableNode<T> drawableNode)
        {
            DrawableService.AddToCollection(drawableNode);
        }

        /// <summary>
        /// Subscribe t TreeInitButton Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeInitButton_Click(object sender, RoutedEventArgs e)
        {
            Init();
            config.RenderWindow.Show();
        }
    }
}
