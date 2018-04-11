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

namespace DAS_SDK.MVC.Controller
{
    class SDK_Controller
    {
        private List<Window> windowRefList;
        private Base_Debug debug;

        MyFront<int?> myFront;
        MyFront<string> myStrFront;
        MyList<int?> myList;

        File_Generator_Base<object> file_Generator;

        Front_END front_END;

        public MyFront<object> myObjFront;
        public MyList<object> myObjList;

        //Base_Sort<object> Base_Sort;

        Bubble_Sort<int> bubble_Sort;
        Select_Sort<float> select_Sort;
        Quick_Sort<int> quick_Sort;
        Radix_Sort<int> radix_Sort;

        //Base_Search<int> search_Base;

        Binary_Search<int> binary_Search;

        public SDK_Controller(Window windowRef, Thread _UI_Thread)
        {
            windowRefList = new List<Window>();
            windowRefList.Add(windowRef);

            debug = new Base_Debug();

            front_END = new Front_END(windowRefList, _UI_Thread, debug);

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




            Thread _threadRd = new Thread(delegate ()
            {
                //radix_Sort = new Radix_Sort<int>(debug, front_END);
                //radix_Sort.Sort();
                
                //file_Generator = new File_Generator_Base<object>(((ulong)(Math.Pow(100, 3) * 10)), "file2.txt");
                //file_Generator.CreateAndFill<int>();

                binary_Search = new Binary_Search<int>("file2.txt", debug, front_END, 267841804, Sort_Type.Quick);
                binary_Search.Search();
            });
            _threadRd.Start();


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
