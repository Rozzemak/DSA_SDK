using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using DAS_SDK.MVC.Enums;
using DAS_SDK.MVC.Model.Debug;

namespace DAS_SDK.MVC.Model.FrontEND
{

    class SortFrontEnd : BaseFrontEnd
    {
        public List<UIElement> UiElements = new List<UIElement>();
        public Button ControllerInitButton;
        public ProgressBar ProgressBar;
        public int ProgressValIncrement = 1;
        public int SizeOfFileInMb = 10;
        public TextBox FileSizeInMbTextBox;
        public TextBox FileNameTextBox;

        public SortFrontEnd(List<Window> windowRefList, Thread uiThread, BaseDebug debug)
            : base(windowRefList, uiThread, debug)
        {
            Init();
        }

        private void Init() {
            foreach (var item in WindowRefList)
            {
                if ((Grid)(item.Content) != null)
                {
                    if (GridRootControlRef == null)
                    {
                        GridRootControlRef = (item.Content as Grid);
                        foreach (var item2 in GridRootControlRef.Children)
                        {
                            if (StackPanel == null && item2 as StackPanel != null)
                            {
                                StackPanel = (item2 as StackPanel);
                            }
                            if ((item2 as ProgressBar) != null)
                            {
                                this.ProgressBar = (item2 as ProgressBar);
                            }
                            if ((item2 as Button) != null && (item2 as Button).Name.ToLower().Contains("cont_init"))
                            {
                                this.ControllerInitButton = item2 as Button;
                                this.ControllerInitButton.Click += Controller_InitButton_Click;
                            }
                        }
                    }
                }
            }
            foreach (var enumName in Enum.GetNames(typeof(DasEnumSortType)))
            {
                var btn = new Button
                {
                    Name = enumName,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Content = enumName
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
            FileSizeInMbTextBox = new TextBox() {
                Height = 20,
                Width = 45,
                ToolTip = "File_Size_In_MB",
                Text = 10.ToString(),
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 70, 0, 0)
        };
            FileNameTextBox = new TextBox()
            {
                Height = 20,
                Width = 90,
                ToolTip = "Name_Of_File_To_Be_Sorted",
                Text = "FileToSort.txt",
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 50, 0, 0)
            };
            FileSizeInMbTextBox.TextChanged += FileSizeInMBTextBox_TextChanged;
            (WindowRefList[0].Content as Grid).Children.Add((FileSizeInMbTextBox));
            (WindowRefList[0].Content as Grid).Children.Add((FileNameTextBox));
            Grid.SetColumn(FileSizeInMbTextBox, 1);
            Grid.SetColumn(FileNameTextBox, 1);
        }

        /// <summary>
        /// Just for convenience, simple event subscription & val change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileSizeInMBTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Int32.TryParse((sender as TextBox).Text, out var o))
                SizeOfFileInMb = o;
            else
            {
                SizeOfFileInMb = 10;
            }
        }

        private void Controller_InitButton_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).Visibility = Visibility.Hidden;
        }
    }
}
