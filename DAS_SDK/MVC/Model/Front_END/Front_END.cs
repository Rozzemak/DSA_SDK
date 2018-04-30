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

namespace DAS_SDK.MVC.Model.Front_END
{
    class Front_END
    {
        List<UIElement> UiElements = new List<UIElement>();
        public List<Window> WindowRefList;
        public Grid Grid_Root_ControlRef;
        public StackPanel StackPanel;
        public ProgressBar progressBar;
        public Button SortButton;
        public Thread UI_Thread;
        public int ProgressValIncrement = 0;
        private Base_Debug Debug;

        public Front_END(List<Window> windowRefList, Thread _UI_Thread, Base_Debug debug)
        {
            UI_Thread = _UI_Thread;
            this.Debug = debug;
            this.WindowRefList = windowRefList;
            foreach (var item in WindowRefList)
            {
                if ((Grid)(item.Content) != null)
                {
                    if (Grid_Root_ControlRef == null)
                    {
                        Grid_Root_ControlRef = (item.Content as Grid);
                        foreach (var item2 in Grid_Root_ControlRef.Children)
                        {
                            if (StackPanel == null && item2 as StackPanel != null)
                            {
                                StackPanel = (item2 as StackPanel);
                            }
                            if ((item2 as ProgressBar) != null)
                            {
                                this.progressBar = (item2 as ProgressBar);
                            }
                            if ((item2 as Button) != null && (item2 as Button).Name.ToLower().Contains("cont_init"))
                            {
                                this.SortButton = item2 as Button;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < 5; i++)
            {
                Button btn = new Button
                {
                    Name = "Button_GEN"+i,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Content = "Create&Fill__File",
                };
                UiElements.Add(btn);
            }

            foreach (var item in UiElements)
            {
                Grid.SetColumn(item, 0);
                StackPanel.Children.Add(item);
                item.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                item.Arrange(new Rect());
                item.TranslatePoint(new Point(0, 0), item);
            }

            //progressBar = Grid_Root_ControlRef.FindName("Progress_Bar") as ProgressBar;
            //debug.AddMessage<object>(new Message<object>("Progress bar name: [" + progressBar.Name + "]", MessageType_ENUM.Indifferent));
            //debug.AddMessage<object>(new Message<object>("Grid name: [" + Grid_Root_ControlRef.Name +"]", MessageType_ENUM.Indifferent));
            //debug.AddMessage<object>(new Message<object>("InitController_Button name: [" + SortButton.Name + "]", MessageType_ENUM.Indifferent));
        }






    }
}
