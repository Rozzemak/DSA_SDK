using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DAS_SDK.MVC.Model.Debug;

namespace DAS_SDK.MVC.Model.Front_END
{
    class Sort_Front_END : Base_Front_END
    {
        public Button SortButton;
        public ProgressBar progressBar;
        public int ProgressValIncrement = 0;

        public Sort_Front_END(List<Window> windowRefList, Thread _UI_Thread, Base_Debug debug)
            : base(windowRefList, _UI_Thread, debug)
        {
            Init();
        }

        private void Init() {
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
                    Name = "Button_GEN" + i,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Content = "Create&Fill__File",
                };
                UiElements.Add(btn);
            }

            foreach (var item in this.UiElements)
            {
                Grid.SetColumn(item, 0);
                StackPanel.Children.Add(item);
                item.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                item.Arrange(new Rect());
                item.TranslatePoint(new Point(0, 0), item);
            }
        }
    }
}
