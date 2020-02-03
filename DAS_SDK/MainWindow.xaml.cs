using DAS_SDK.MVC.Controller;
using DAS_SDK.MVC.Enums;
using DAS_SDK.MVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Runtime.InteropServices;
using System.Windows.Threading;
using DAS_SDK.MVC.Model.FrontAndList;

namespace DAS_SDK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SdkController<int> controller;
        Random rnd = new Random();

        public MainWindow()
        {
            Console.WriteLine("Loading, please wait.");
            new Thread(() => CheckWindow()).Start();
            InitializeComponent();
            FrontAddType_CB.Visibility = Visibility.Visible;
            ListAddType_CB.Visibility = Visibility.Hidden;
        }

        private void CheckWindow()
        {
            var loaded = false;
            var stop = false;
            var th = new Thread(() => 
            {
                while (true)
                {
                    Thread.Sleep(25);
                    Console.Write("█");
                    if (stop)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n Window_Loaded! \n ");
                        Console.ResetColor();
                        break;
                    }
                }
            });
            th.Start();
            while (true)
            {
                Thread.Sleep(1); 
                this.Dispatcher.Invoke(() =>
                {
                    if ((Application.Current.MainWindow).IsLoaded) loaded = true;
                });
                if (loaded)
                {
                    stop = true;
                    break;
                }
            }
        }

        private void AddVal(object sender, RoutedEventArgs e)
        {
            if ((DasEnumCustomArray)Enum.Parse(typeof(DasEnumCustomArray), ((ComboBoxItem)OperationType_CB.SelectedItem).Content.ToString()) == DasEnumCustomArray.Front)
            {
                controller.MyObjFront.AddToFront<object>((TextBox_Num.Text) as object, (AddMethodFront)Enum.Parse(typeof(AddMethodFront), ((ComboBoxItem)FrontAddType_CB.SelectedItem).Content.ToString()));
                TextBox_ArrayContent.Text = controller.MyObjFront.GetFrontContent();
                TextBox_Num.Text = rnd.Next(-200, 200).ToString();
            }
            else
            {
                controller.MyObjList.AddToList<object>((TextBox_Num.Text) as object, (AddMethodList)Enum.Parse(typeof(AddMethodList), ((ComboBoxItem)ListAddType_CB.SelectedItem).Content.ToString()));
                TextBox_ArrayContent.Text = controller.MyObjList.GetListContent();
                TextBox_Num.Text = rnd.Next(-200, 200).ToString();
            }
        }

        private void RemoveVal(object sender, RoutedEventArgs e)
        {
            if ((DasEnumCustomArray)Enum.Parse(typeof(DasEnumCustomArray), ((ComboBoxItem)OperationType_CB.SelectedItem).Content.ToString()) == DasEnumCustomArray.Front)
            {
                controller.MyObjFront.RemoveFromFront<object>();
                TextBox_ArrayContent.Text = controller.MyObjFront.GetFrontContent();
            }
            else
            {
                controller.MyObjList.RemoveFromList<object>();
                TextBox_ArrayContent.Text = controller.MyObjList.GetListContent();
            }

        }

        private void OperationType_CB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (controller != null)
            {
                if ((DasEnumCustomArray)Enum.Parse(typeof(DasEnumCustomArray), ((ComboBoxItem)OperationType_CB.SelectedItem).Content.ToString()) == DasEnumCustomArray.Front)
                {
                    controller.MyObjFront = new MyFront<object>();
                    FrontAddType_CB.Visibility = Visibility.Visible;
                    ListAddType_CB.Visibility = Visibility.Hidden;
                }
                else
                {
                    controller.MyObjList = new MyList<object>();
                    ListAddType_CB.Visibility = Visibility.Visible;
                    FrontAddType_CB.Visibility = Visibility.Hidden;
                }
            }
            TextBox_ArrayContent.Text = null;
        }

        private void Cont_InitButton_Click(object sender, RoutedEventArgs e)
        {
            Add_ValButton.Visibility = Visibility.Visible;
            Rem_ValButton.Visibility = Visibility.Visible;
            SetConsolePosition((int)this.Left, (int)(this.Top+20));
            var thread = new Thread(()=>
            {
                this.Dispatcher.BeginInvoke(new Action(()=> {
                    controller = new SdkController<int>(this, App.Current.MainWindow.Dispatcher.Thread);
                }));          
            });
             thread.Start();
        }

        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Searched MC documentation, and felt urge to comment this -_-.
        /// </summary>
        /// <param name="hWnd"> Console window handle</param>
        /// <param name="opt_hWnd">Enum for win position, -1 for topmost, 0 for top, and some more</param>
        /// <param name="x">x from left</param>
        /// <param name="y">y from top</param>
        /// <param name="cx">x from left + width</param>
        /// <param name="cy">y from top + height</param>
        /// <param name="uFlags">No idea</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr optHWnd, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public void SetConsolePosition(int left, int top)
        {
            SetForegroundWindow(GetConsoleWindow());
            //left = left + (int)this.Width + 200;
            SetWindowPos(GetConsoleWindow(), IntPtr.Zero, left, top + (int)this.Height, (int)(this.Width * 1.3), (int)this.Height, 0);
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            //Rest, do this later... It will loose control while moving console win && mouse button down.
            //SetWindowPos(GetConsoleWindow(), IntPtr.Zero, (int)this.Left, (int)this.Top + (int)this.Height, (int)(this.Width * 1.3), (int)this.Height, 0);
        }
    }
}
