using DAS_SDK.MVC.Model;
using DAS_SDK.MVC.Model.File_Generator;
using DAS_SDK.MVC.Model.Front_END;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DAS_SDK.MVC.Model.Sorts.Base_Sort;
using DAS_SDK.MVC.Model.Sorts;
using System.Threading;
using DAS_SDK.MVC.Model.Debug;
using DAS_SDK.PGL_2;
using DAS_SDK.MVC.Model.Search;
using DAS_SDK.MVC.Model.Collisions;
using DAS_SDK.MVC.Model.Trees.Base_Tree;
using DAS_SDK.MVC.Model.Trees.Base_Tree.Node;
using DAS_SDK.MVC.Model.Trees.Base_Tree.Shapes;
using DAS_SDK.MVC.Model.Trees;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Diagnostics;
using System.IO;
using DAS_SDK.MVC.Enums;
using DAS_SDK.MVC.Model.Trees.BinaryTree;

namespace DAS_SDK.MVC.Controller
{
    class SDK_Controller<T> where T : IComparable
    {
        private List<Window> windowRefList;
        private Base_Debug debug;

        MyFront<int?> myFront;
        MyFront<string> myStrFront;
        MyList<int?> myList;
        Base_Tree<string> base_Tree;
        List<Base_Tree<string>> Trees = new List<Base_Tree<string>>();

        File_Generator_Base<object> file_Generator;
        BTreeManager<int> bTreeManager;

        Sort_Front_END sort_Front_END;
        TreeService<string> treeService = new TreeService<string>();
        Tree_Front_END<string> tree_Front_END;

        public MyFront<object> myObjFront;
        public MyList<object> myObjList;

        Bubble_Sort<T> bubble_Sort;
        Select_Sort<T> select_Sort;
        Quick_Sort<T> quick_Sort;
        Radix_Sort<T> radix_Sort;
        HeapSort<T> heap_Sort;

        Binary_Search<int> binary_Search;

        public void WriteLevels()
        {
            foreach (var item in treeService.GetCollection())
            {
                debug.AddMessage<object>(new Message<object>(item.Root.Level.ToString() + " _ " + item.Root.GetType().Name + " _ " + item.Root.Content));
                foreach (var branches in item.Branches)
                {
                    debug.AddMessage<object>(new Message<object>(branches.BranchMasterNode.Level.ToString() + " _ " + branches.BranchMasterNode.GetType().Name + " _ " + branches.BranchMasterNode.Content));
                    foreach (var bNode in branches.BranchNodes)
                    {
                        debug.AddMessage<object>(new Message<object>(bNode.Level.ToString() + " _ " + bNode.GetType().Name + " _ " + bNode.Content));
                    }
                }
            }
        }

        public SDK_Controller(Window windowRef, Thread _UI_Thread)
        {
            windowRefList = new List<Window>();
            windowRefList.Add(windowRef);
            debug = new Base_Debug();
            sort_Front_END = new Sort_Front_END(windowRefList, _UI_Thread, debug);
            debug.AddMessage<string>(new Message<object>("Controller_Init", MessageType_ENUM.Indifferent));
            Init();

            Thread _threadRd = new Thread(delegate ()
            {
                //Graph Theorem stuff, unfinished, has no good facade/service,.. nothing. Works though.
                Node<string> node6l = new Node<string>("6l", new List<Node<string>> { }); //node lvl3
                Node<string> node5n = new Node<string>("5n", new List<Node<string>> { node6l }); //node lvl3
                Node<string> node4n = new Node<string>("4n", new List<Node<string>> { node5n }); //node lvl3
                Node<string> node3n = new Node<string>("3n", new List<Node<string>> { node4n }); //node lvl3
                Node<string> node28 = new Node<string>("28", new List<Node<string>> { }); //node lvl 4
                Node<string> node6 = new Node<string>("6", new List<Node<string>> { });//leaf lvl 4
                Node<string> node23 = new Node<string>("23", new List<Node<string>> { node28 }); //node lvl 3
                Node<string> node5 = new Node<string>("5", new List<Node<string>> { node6 }); //node lvl 3
                Node<string> node4 = new Node<string>("4", new List<Node<string>> { node5 }); //node lvl 2
                Node<string> node3 = new Node<string>("3", new List<Node<string>> { node23, node3n }); //Leaf lvl 4
                Node<string> node2 = new Node<string>("2", new List<Node<string>> { }); //Leaf lvl 4
                Node<string> node1 = new Node<string>("1", new List<Node<string>> { node2, node3 }); //node lvl3
                Node<string> node = new Node<string>("0", new List<Node<string>> { node1 }); //node lvl2

                List<Node<string>> nodes = new List<Node<string>>
                {
                    node3,
                    node2,
                    node1,
                    node
                };
                Root<string> root = new Root<string>("rootContent1");

                List<BranchMasterNode<string>> Bnodes = new List<BranchMasterNode<string>> {
                    new BranchMasterNode<string>("BMNodeContent1", new List<Node<string>>{ root as Node<string>, node, node4 })
                    //,new BranchMasterNode<string>("BNodeContent2", new List<Node<string>>{ root as Node<string>, node3 })
                };

                foreach (var bnode in Bnodes)
                {
                    bnode.ParentNode = root;
                }

                foreach (var item in root.BranchMasterNodes)
                {
                    debug.AddMessage<string>(new Message<object>(
                       "Type: [" + item.GetType().Name.Substring(0, item.GetType().Name.Length - 2) + "] " +
                       "Content: [" + item.Content + "] "));
                    foreach (var item2 in item.ConnectedNodes)
                    {
                        debug.AddMessage<string>(new Message<object>(
                      "Type: [" + item2.GetType().Name.Substring(0, item2.GetType().Name.Length - 2) + "] " +
                      "Content: [" + item2.Content + "] ", MessageType_ENUM.________));
                    }
                }

                Branch<string> bTest = new Branch<string>(new List<Node<string>> {
                    node,
                    node2,node1,node6l, node5n, node4n, node3n,node3, node23,node28, node4, node5, node6},
                    root.BranchMasterNodes[0]);
                //debug.AddMessage<string>(new Message<object>("Leaves Count: " + bTest.Leaves.Count));
                //foreach (var item in bTest.BranchNodes)
                //{
                //    debug.AddMessage<string>(new Message<object>("BranchNodes Content : " + item.Content));
                //}
                //foreach (var item in bTest.Leaves)
                //{
                //    debug.AddMessage<string>(new Message<object>("Leaves Content : " + item.Content));
                //}
                //debug.AddMessage<string>(new Message<object>("BMaster Content : " + bTest.BranchMasterNode.Content));
                base_Tree = new Base_Tree<string>(new List<Branch<string>>() { bTest });
                Trees.Add(base_Tree);
                Dispatcher.FromThread(_UI_Thread).Invoke(() =>
                {
                    treeService.AddToCollection(base_Tree);
                    tree_Front_END = new Tree_Front_END<string>(windowRefList, _UI_Thread, debug, treeService);

                });


                bTreeManager = new BTreeManager<int>(debug, sort_Front_END);
                bTreeManager.Init(100000);
                bTreeManager.FindNode(1);
                bTreeManager.WriteTree();
                bTreeManager.WriteTree(BinaryTree<int>.TraversalMode.PreOrder);


                //WriteLevels();
                SubscribeToSortFrontEndEvents(sort_Front_END);
            });
            _threadRd.Start();
        }

        public void Init()
        {
            // I used this for tests, to determine, if the front was  usable "enough".
            // Also, when you do generic, it is almost a neccesity to do a proper unit tests, but this was faster.

            myFront = new MyFront<int?>();
            myStrFront = new MyFront<string>();
            myList = new MyList<int?>();

            myObjFront = new MyFront<object>();
            myObjList = new MyList<object>();

            //file_Generator = new File_Generator_Base<object>(((ulong)(Math.Pow(100, 3) * 10)),"300.txt",0,
            //    Enums.DAS_FORMAT_ENUM.TXT_1ROW_NVALS_COMMA_SEPARTOR);

            // TestZone
            if (false)
            {

                #region FirstFront

                for (int i = 0; i < myFront.customArray.GetArrayAsType<int?>().Length * 2; i++)
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
                myObjFront.AddToFront((object)"aaasda");
                myObjFront.AddToFront((object)123);

                Console.WriteLine(myObjFront.GetFrontContent());

                myObjFront.ClearFront(20);

                myObjFront.AddToFront(1 as object, AddMethodFront.OnlyFill);
                myObjFront.AddToFront("asdha" as object);
                myObjFront.AddToFront("To be removed" as object);

                myObjFront.RemoveFromFront();

                Console.WriteLine(myObjFront.GetFrontContent());
                #endregion



                for (int i = 0; i < myList.customArray.GetArrayAsType<int?>().Length * 2; i++)
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

        public void Sort_FrontEND_Init(DAS_ENUM_SORT_TYPE SortType, int fileSizeInMB)
        {
            Thread _thread = new Thread(delegate ()
            {
                string name = "Unsorted.txt";
                int filesize = 10;
                Dispatcher.FromThread(sort_Front_END.UI_Thread).Invoke(new Action(() =>
                {
                    name = sort_Front_END.FileNameTextBox.Text;
                    filesize = sort_Front_END.SizeOfFileInMB;
                }));
                file_Generator = new File_Generator_Base<object>(
                    ((ulong)(Math.Pow(100, 3) * filesize)), name);
                file_Generator.CreateAndFill<T>();
                switch (SortType)
                {
                    case DAS_ENUM_SORT_TYPE.Bubble:
                        bubble_Sort = new Bubble_Sort<T>(debug, sort_Front_END, file_Generator.path);
                        (bubble_Sort as Base_Sort<T>).FileSorted += SDK_Controller_FileSorted;
                        bubble_Sort.Sort();
                        break;
                    case DAS_ENUM_SORT_TYPE.Select:
                        select_Sort = new Select_Sort<T>(debug, sort_Front_END, file_Generator.path);
                        (select_Sort as Base_Sort<T>).FileSorted += SDK_Controller_FileSorted;
                        select_Sort.Sort();
                        break;
                    case DAS_ENUM_SORT_TYPE.Quick:
                        quick_Sort = new Quick_Sort<T>(debug, sort_Front_END, file_Generator.path);
                        (quick_Sort as Base_Sort<T>).FileSorted += SDK_Controller_FileSorted;
                        quick_Sort.Sort();
                        break;
                    case DAS_ENUM_SORT_TYPE.Radix:
                        radix_Sort = new Radix_Sort<T>(debug, sort_Front_END, file_Generator.path);
                        (radix_Sort as Base_Sort<T>).FileSorted += SDK_Controller_FileSorted;
                        radix_Sort.Sort();
                        break;
                    case DAS_ENUM_SORT_TYPE.Heap:
                        heap_Sort = new HeapSort<T>(debug, sort_Front_END, file_Generator.path);
                        (heap_Sort as Base_Sort<T>).FileSorted += SDK_Controller_FileSorted;
                        heap_Sort.Sort();
                        break;
                    case DAS_ENUM_SORT_TYPE.none:
                        throw new Exception("No sort selected.");
                }
            });
            _thread.Start();
        }

        /// <summary>
        /// Just subscription to FileSorted event, you can basically do whatever you want in here
        /// I chose to open dir and make UIelements visible again for another sort..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SDK_Controller_FileSorted(object sender, EventArgs e)
        {
            foreach (var sortInitButton in sort_Front_END.UiElements)
            {
                Dispatcher.FromThread(sort_Front_END.UI_Thread).Invoke(new Action(() =>
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
        private void SubscribeToSortFrontEndEvents(Sort_Front_END sort_Front_END)
        {
            foreach (var sortInitButton in sort_Front_END.UiElements)
            {
                foreach (var item in Enum.GetNames(typeof(DAS_ENUM_SORT_TYPE)))
                {
                    Dispatcher.FromThread(sort_Front_END.UI_Thread).Invoke(new Action(() =>
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
            foreach (var sortInitButton in sort_Front_END.UiElements)
            {
                sortInitButton.Visibility = Visibility.Hidden;
            }
            Sort_FrontEND_Init((DAS_ENUM_SORT_TYPE)Enum.Parse(typeof(DAS_ENUM_SORT_TYPE), (sender as Button).Name), 1);
            debug.AddMessage<string>(new Message<object>((sender as Button).Name + "sort initiated!"));
        }
    }
}
