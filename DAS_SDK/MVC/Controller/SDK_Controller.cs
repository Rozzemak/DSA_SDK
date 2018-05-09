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

namespace DAS_SDK.MVC.Controller
{
    class SDK_Controller
    {
        private List<Window> windowRefList;
        private Base_Debug debug;

        MyFront<int?> myFront;
        MyFront<string> myStrFront;
        MyList<int?> myList;
        Base_Tree<string> base_Tree;
        List<Base_Tree<string>> Trees = new List<Base_Tree<string>>();

        File_Generator_Base<object> file_Generator;

        Sort_Front_END sort_Front_END;
        TreeService<string> treeService = new TreeService<string>();
        Tree_Front_END<string> tree_Front_END;


        public MyFront<object> myObjFront;
        public MyList<object> myObjList;

        //Base_Sort<object> Base_Sort;

        Bubble_Sort<int> bubble_Sort;
        Select_Sort<float> select_Sort;
        Quick_Sort<int> quick_Sort;
        Radix_Sort<int> radix_Sort;

        //Base_Search<int> search_Base;

        Binary_Search<int> binary_Search;
        ConvexHull convexHull;

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
            //Base_Sort = new Base_Sort<object>();
            //Base_Sort.CreateSortedFile();
            //bubble_Sort = new Bubble_Sort<int>(front_END);
            //select_Sort = new Select_Sort<float>(front_END);

            //Console.WriteLine(bubble_Sort.progressBar.Name.ToString());
            // bubble_Sort.Sort();
            //Thread _thread = new Thread(delegate () {
            //    quick_Sort = new Quick_Soort<int>(debug,front_END);
            //    quick_Sort.Sort();
            //});
            //  _thread.Start();



            //radix_Sort = new Radix_Sort<int>(debug, front_END);
            //radix_Sort.Sort();

            //file_Generator = new File_Generator_Base<object>(((ulong)(Math.Pow(100, 3) * 10)), "file2.txt");
            //file_Generator.CreateAndFill<int>();

            //binary_Search = new Binary_Search<int>("file2_sorted.txt", debug, front_END, 267841804, Sort_Type.Quick);
            //binary_Search.Search();

            /*
            List<Point> points = new List<Point> {
                new Point(1,2),
                new Point(2,1),
                new Point(3,2),
                new Point(5,2),
                new Point(5,3),
                new Point(1000,1000)
            };

            convexHull = new ConvexHull(front_END);  
            foreach (var item in convexHull.GetConvexHull(points))
            {
                debug.AddMessage<object>(new Message<object>(item.ToString()));
            }
       */

            Thread _threadRd = new Thread(delegate ()
            {

                Node<string> node6 = new Node<string>("6", new List<Node<string>> { });//leaf lvl 5
                Node<string> node5 = new Node<string>("5", new List<Node<string>> { node6 }); //node lvl 4
                Node<string> node4 = new Node<string>("4", new List<Node<string>> { node5 }); //node lvl 3
                Node<string> node3 = new Node<string>("3", new List<Node<string>> { }); //Leaf lvl 4
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

                Bnodes[0].ParentNode = root;
                node.ParentNode = Bnodes[0];
                node1.ParentNode = node;
                node2.ParentNode = node1;
                node3.ParentNode = node1;
                node4.ParentNode = Bnodes[0];
                node5.ParentNode = node4;
                node6.ParentNode = node5;


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
                    Bnodes[0].ConnectedNodes[1],
                    node2,node1,node3, node4, node5, node6},
                    root.BranchMasterNodes[0]);
                debug.AddMessage<string>(new Message<object>("Leaves Count: " + bTest.Leaves.Count));
                foreach (var item in bTest.BranchNodes)
                {
                    debug.AddMessage<string>(new Message<object>("BranchNodes Content : " + item.Content));
                }
                foreach (var item in bTest.Leaves)
                {
                    debug.AddMessage<string>(new Message<object>("Leaves Content : " + item.Content));
                }
                debug.AddMessage<string>(new Message<object>("BMaster Content : " + bTest.BranchMasterNode.Content));
                base_Tree = new Base_Tree<string>(new List<Branch<string>>() { bTest });
                Trees.Add(base_Tree);

                Dispatcher.FromThread(_UI_Thread).Invoke(() =>
                {
                    treeService.AddToCollection(base_Tree);
                    tree_Front_END = new Tree_Front_END<string>(windowRefList, _UI_Thread, debug, treeService);
                   
                });

                WriteLevels();
            });
            _threadRd.Start();



            // XML_Lib xML_Lib = new XML_Lib(debug);
            // xML_Lib.ReadAll();

            //Dog alik = new Dog("Haf");
            //alik.Make_a_Sound();


            //RunningAnimal runningAlik = new RunningAnimal();
            //RunningAnimal runningRozzara = new RunningAnimal();


            //runningAlik.SetAnimal(alik);
            //runningAlik.Run();
            //runningAlik.Run();
            //runningAlik.Make_a_Sound();

            //runningRozzara.SetAnimal(rozzara);
            //rozzara.Make_a_Sound();
            //runningRozzara.Run();
            //runningRozzara.Run();
            //runningRozzara.Make_a_Sound();

            //Cat rozzara = new Cat("Meow");
            //FlyingAnimal bird = new FlyingAnimal();
            //bird.SetAnimal(rozzara);
            //bird.Make_a_Sound();
            //bird.Fly();
            //bird.Fly();
            //bird.Fly();
            //bird.GetRunningAnimal().Run();

            //Receiver rec = new Receiver();
            //Adapter adp = new Adapter();

            //string write = "";
            //foreach (var item in rec.Receive())
            //{
            //    write += item.ToString();
            //}
            //debug.AddMessage<object>(new Message<object>(write));

            //string write2 = "";
            //foreach (var item in adp.Receive())
            //{
            //    write2 += item.ToString();
            //}
            //debug.AddMessage<object>(new Message<object>(write2));



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

    }
}
