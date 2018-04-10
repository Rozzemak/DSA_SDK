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

namespace DAS_SDK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SDK_Controller _Controller;
        Random rnd = new Random();


        public MainWindow()
        {
            InitializeComponent();
            FrontAddType_CB.Visibility = Visibility.Visible;
            ListAddType_CB.Visibility = Visibility.Hidden;
            // Managed();
        }

        private void AddVal(object sender, RoutedEventArgs e)
        {
            if ((DAS_ENUM_CustomArray)Enum.Parse(typeof(DAS_ENUM_CustomArray), ((ComboBoxItem)OperationType_CB.SelectedItem).Content.ToString()) == DAS_ENUM_CustomArray.Front)
            {
                _Controller.myObjFront.AddToFront<object>((TextBox_Num.Text) as object, (AddMethodFront)Enum.Parse(typeof(AddMethodFront), ((ComboBoxItem)FrontAddType_CB.SelectedItem).Content.ToString()));
                TextBox_ArrayContent.Text = _Controller.myObjFront.GetFrontContent();
                TextBox_Num.Text = rnd.Next(-200, 200).ToString();
            }
            else
            {
                _Controller.myObjList.AddToList<object>((TextBox_Num.Text) as object, (AddMethodList)Enum.Parse(typeof(AddMethodList), ((ComboBoxItem)ListAddType_CB.SelectedItem).Content.ToString()));
                TextBox_ArrayContent.Text = _Controller.myObjList.GetListContent();
                TextBox_Num.Text = rnd.Next(-200, 200).ToString();
            }
        }



        private void RemoveVal(object sender, RoutedEventArgs e)
        {
            if ((DAS_ENUM_CustomArray)Enum.Parse(typeof(DAS_ENUM_CustomArray), ((ComboBoxItem)OperationType_CB.SelectedItem).Content.ToString()) == DAS_ENUM_CustomArray.Front)
            {
                _Controller.myObjFront.RemoveFromFront<object>();
                TextBox_ArrayContent.Text = _Controller.myObjFront.GetFrontContent();
            }
            else
            {
                _Controller.myObjList.RemoveFromList<object>();
                TextBox_ArrayContent.Text = _Controller.myObjList.GetListContent();
            }

        }

        private void OperationType_CB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_Controller != null)
            {
                if ((DAS_ENUM_CustomArray)Enum.Parse(typeof(DAS_ENUM_CustomArray), ((ComboBoxItem)OperationType_CB.SelectedItem).Content.ToString()) == DAS_ENUM_CustomArray.Front)
                {
                    _Controller.myObjFront = new MyFront<object>();
                    FrontAddType_CB.Visibility = Visibility.Visible;
                    ListAddType_CB.Visibility = Visibility.Hidden;
                }
                else
                {
                    _Controller.myObjList = new MyList<object>();
                    ListAddType_CB.Visibility = Visibility.Visible;
                    FrontAddType_CB.Visibility = Visibility.Hidden;
                }
            }
            TextBox_ArrayContent.Text = null;
        }

        private void Cont_InitButton_Click(object sender, RoutedEventArgs e)
        {
            SetConsolePosition((int)this.Left, (int)this.Top);
            Thread _thread = new Thread(delegate () {
                _Controller = new SDK_Controller(this, this.Dispatcher.Thread);
            });
            // _thread.Start();
            _Controller = new SDK_Controller(this, this.Dispatcher.Thread);
            // Console.SetWindowPosition((int)(this.Left + this.Width), (int)this.Top);
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
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr opt_hWnd, int x, int y, int cx, int cy, uint uFlags);

        public void SetConsolePosition(int left, int top)
        {
            SetForegroundWindow(GetConsoleWindow());
            left = left + (int)this.Width + 200;
            SetWindowPos(GetConsoleWindow(), IntPtr.Zero, left, top, (int)(this.Width*1.3), (int)this.Height,0);
        }
    }
}
