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

namespace DAS_SDK.MVC.Model.Front_END
{
    class Tree_Front_END<T> : Base_Front_END where T : IComparable
    {
        Window RenderWindow = new Window();
        Tuple<Node<T>, Vector, Node<T>> Edges;

        public Tree_Front_END(List<Window> windowRefList, Thread _UI_Thread, Base_Debug debug)
            : base(windowRefList, _UI_Thread, debug)
        {
            // Just for controller visual edits maybe;
            this.WindowRefList.Add(RenderWindow);
            Init();
            RenderWindow.Show();
        }

        private void Init()
        {
            Button btn = new Button();
            RenderWindow.Content = new Button()
            {
                Content = "wush",
                Height = 20,
                Width = 20,
                Visibility = Visibility.Visible

            };
        }
    }
}
