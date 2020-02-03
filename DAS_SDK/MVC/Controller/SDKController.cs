using DAS_SDK.MVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DAS_SDK.MVC.Model.Sorts;
using System.Threading;
using DAS_SDK.MVC.Model.Debug;
using DAS_SDK.MVC.Model.Search;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Diagnostics;
using System.IO;
using DAS_SDK.MVC.Enums;
using DAS_SDK.MVC.Model.FileGenerator;
using DAS_SDK.MVC.Model.FrontAndList;
using DAS_SDK.MVC.Model.FrontEND;
using DAS_SDK.MVC.Model.Sorts.BaseSort;

namespace DAS_SDK.MVC.Controller
{
    class SdkController<T> where T : IComparable
    {
        private List<Window> windowRefList;
        private BaseDebug debug;

        MyFront<int?> myFront;
        MyFront<string> myStrFront;
        MyList<int?> myList;


        FileGeneratorBase<object> fileGenerator;


        SortFrontEnd sortFrontEnd;
        
        public MyFront<object> MyObjFront;
        public MyList<object> MyObjList;

        BubbleSort<T> bubbleSort;
        SelectSort<T> selectSort;
        QuickSort<T> quickSort;
        RadixSort<T> radixSort;
        HeapSort<T> heapSort;

        BinarySearch<int> binarySearch;
        
        public SdkController(Window windowRef, Thread uiThread)
        {
            windowRefList = new List<Window>();
            windowRefList.Add(windowRef);
            debug = new BaseDebug();
            sortFrontEnd = new SortFrontEnd(windowRefList, uiThread, debug);
            debug.AddMessage<string>(new Message<object>("Controller_Init", MessageTypeEnum.Indifferent));
            Init();

            var threadRd = new Thread(delegate ()
            {
                //WriteLevels();
                SubscribeToSortFrontEndEvents(sortFrontEnd);
            });
            threadRd.Start();
        }

        public void Init()
        {
            // I used this for tests, to determine, if the front was  usable "enough".
            // Also, when you do generic, it is almost a neccesity to do a proper unit tests, but this was faster.

            myFront = new MyFront<int?>();
            myStrFront = new MyFront<string>();
            myList = new MyList<int?>();

            MyObjFront = new MyFront<object>();
            MyObjList = new MyList<object>();

            //file_Generator = new File_Generator_Base<object>(((ulong)(Math.Pow(100, 3) * 10)),"300.txt",0,
            //    Enums.DAS_FORMAT_ENUM.TXT_1ROW_NVALS_COMMA_SEPARTOR);

            // TestZone
            if (false)
            {

                #region FirstFront

                for (var i = 0; i < myFront.CustomArray.GetArrayAsType<int?>().Length * 2; i++)
                {
                    myFront.AddToFront<int?>(i);
                }

                //myFront.AddToFront<int?>(6,AddMethodFront.ReplaceFirst);
                //myFront.AddToFront<int?>(2,AddMethodFront.OnlyFill);

                Console.WriteLine(myFront.GetFrontContent());

                myFront.ClearFront(20);

                myFront.AddToFront<int?>(2);
                myFront.AddToFront<int?>(3);
                myFront.AddToFront<int?>(4);

                myFront.RemoveFromFront();

                Console.WriteLine(myFront.GetFrontContent());
                #endregion

                #region SecFront, With implicitly nullable type
                myStrFront.AddToFront("aaasda");
                myStrFront.AddToFront("aa");

                Console.WriteLine(myStrFront.GetFrontContent());

                myStrFront.ClearFront(20);

                myStrFront.AddToFront("aa");
                myStrFront.AddToFront("asdha");
                myStrFront.AddToFront("aaasdw");

                myStrFront.RemoveFromFront();

                Console.WriteLine(myStrFront.GetFrontContent());
                #endregion

                #region ThirdFront, objectively :D, As you see, it is possible to mix different obj. here.
                MyObjFront.AddToFront((object)"aaasda");
                MyObjFront.AddToFront((object)123);

                Console.WriteLine(MyObjFront.GetFrontContent());

                MyObjFront.ClearFront(20);

                MyObjFront.AddToFront(1 as object, AddMethodFront.OnlyFill);
                MyObjFront.AddToFront("asdha" as object);
                MyObjFront.AddToFront("To be removed" as object);

                MyObjFront.RemoveFromFront();

                Console.WriteLine(MyObjFront.GetFrontContent());
                #endregion



                for (var i = 0; i < myList.CustomArray.GetArrayAsType<int?>().Length * 2; i++)
                {
                    myList.AddToList<int?>(i);
                }

                Console.WriteLine(myList.GetListContent());

                myList.ClearList(20);

                myList.AddToList<int?>(2);
                myList.AddToList<int?>(3);
                myList.AddToList<int?>(4);
                myList.AddToList<int?>(5);

                myList.RemoveFromList();

                Console.WriteLine(myList.GetListContent());
            }
        }

        public void Sort_FrontEND_Init(DasEnumSortType sortType, int fileSizeInMb)
        {
            var thread = new Thread(delegate ()
            {
                var name = "Unsorted.txt";
                var filesize = 10;
                Dispatcher.FromThread(sortFrontEnd.UiThread).Invoke(new Action(() =>
                {
                    name = sortFrontEnd.FileNameTextBox.Text;
                    filesize = sortFrontEnd.SizeOfFileInMb;
                }));
                fileGenerator = new FileGeneratorBase<object>(
                    ((ulong)(Math.Pow(100, 3) * filesize)), name);
                fileGenerator.CreateAndFill<T>();
                switch (sortType)
                {
                    case DasEnumSortType.Bubble:
                        bubbleSort = new BubbleSort<T>(debug, sortFrontEnd, fileGenerator.Path);
                        (bubbleSort as BaseSort<T>).FileSorted += SDK_Controller_FileSorted;
                        bubbleSort.Sort();
                        break;
                    case DasEnumSortType.Select:
                        selectSort = new SelectSort<T>(debug, sortFrontEnd, fileGenerator.Path);
                        (selectSort as BaseSort<T>).FileSorted += SDK_Controller_FileSorted;
                        selectSort.Sort();
                        break;
                    case DasEnumSortType.Quick:
                        quickSort = new QuickSort<T>(debug, sortFrontEnd, fileGenerator.Path);
                        (quickSort as BaseSort<T>).FileSorted += SDK_Controller_FileSorted;
                        quickSort.Sort();
                        break;
                    case DasEnumSortType.Radix:
                        radixSort = new RadixSort<T>(debug, sortFrontEnd, fileGenerator.Path);
                        (radixSort as BaseSort<T>).FileSorted += SDK_Controller_FileSorted;
                        radixSort.Sort();
                        break;
                    case DasEnumSortType.Heap:
                        heapSort = new HeapSort<T>(debug, sortFrontEnd, fileGenerator.Path);
                        (heapSort as BaseSort<T>).FileSorted += SDK_Controller_FileSorted;
                        heapSort.Sort();
                        break;
                    case DasEnumSortType.None:
                        throw new Exception("No sort selected.");
                }
            });
            thread.Start();
        }

        /// <summary>
        /// Just subscription to FileSorted event, you can basically do whatever you want in here
        /// I chose to open dir and make UIelements visible again for another sort..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SDK_Controller_FileSorted(object sender, EventArgs e)
        {
            foreach (var sortInitButton in sortFrontEnd.UiElements)
            {
                Dispatcher.FromThread(sortFrontEnd.UiThread).Invoke(new Action(() =>
                {
                    sortInitButton.Visibility = Visibility.Visible;
                }));
            };
            Process.Start("explorer.exe", Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Front_END buttons event subscription.
        /// </summary>
        /// <param name="sort_Front_END"></param>
        private void SubscribeToSortFrontEndEvents(SortFrontEnd sortFrontEnd)
        {
            foreach (var sortInitButton in sortFrontEnd.UiElements)
            {
                foreach (var item in Enum.GetNames(typeof(DasEnumSortType)))
                {
                    Dispatcher.FromThread(sortFrontEnd.UiThread).Invoke(new Action(() =>
                    {
                        if ((sortInitButton as Button).Name.ToLower().Contains(item.ToLower()))
                            (sortInitButton as Button).Click += SDK_Controller_Click;
                    }));
                }
            }
        }

        /// <summary>
        /// Front_END button click event subscription.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SDK_Controller_Click(object sender, RoutedEventArgs e)
        {
            foreach (var sortInitButton in sortFrontEnd.UiElements)
            {
                sortInitButton.Visibility = Visibility.Hidden;
            }
            Sort_FrontEND_Init((DasEnumSortType)Enum.Parse(typeof(DasEnumSortType), (sender as Button).Name), 1);
            debug.AddMessage<string>(new Message<object>((sender as Button).Name + "sort initiated!"));
        }
    }
}
