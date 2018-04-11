using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using DAS_SDK.MVC.Enums;
using DAS_SDK.MVC.Model.Debug;
using DAS_SDK.MVC.Model.Front_END;

namespace DAS_SDK.MVC.Model.Sorts.Base_Sort
{
    enum Order_Enum
    {
        ASCENDING,
        DESCENDING,
        CANNOT_BE_SPECIFIED,
        ASC_LENGTH,
        DESC_LENGTH
    }

    static class Ext
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
            => self.Select((item, index) => (item, index));
    }

    class Base_Sort<T>
    {
        private DateTime startedSortingDate;

        public List<T> list;
        public Order_Enum order_Enum;
        public string filePath;
        public delegate List<T> DoSort(List<T> list, Front_END.Front_END front_END);
        public DoSort _DoSort;
        private Front_END.Front_END front_END;
        protected Base_Debug _Debug;
        public bool isSorted;

        /// <summary>
        /// Event, that is risen upon sort start.
        /// </summary>
        public event EventHandler FileSortStart;
        /// <summary>
        /// Event, that is risen upon file sorted. (Successfully)
        /// </summary>
        public event EventHandler FileSorted;


        protected Base_Sort(Base_Debug debug, Front_END.Front_END front_END, string path = "unsorted.txt", Order_Enum order = Order_Enum.ASCENDING)
        {
            this._Debug = debug;
            this.front_END = front_END;
            this.filePath = path;
            this.list = ReadFile(path);
            this.order_Enum = order;
        }

        /// <summary>
        /// This constructor is not finished, do not use.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="order"></param>
        protected Base_Sort(List<T> list, Order_Enum order = Order_Enum.ASCENDING)
        {
            this.list = list;
            this.order_Enum = order;
        }

        /// <summary>
        /// While reading a file, 
        /// it fills list with according
        /// format elements.
        /// </summary>
        /// <param name="path">Path to file prefeer {currect_working_dir}</param>
        /// <param name="format">File format, could be txt with different txt format.</param>
        /// <returns>Filled list, mus</returns>
        public List<T> ReadFile(string path, DAS_FORMAT_ENUM format = DAS_FORMAT_ENUM.TXT_ROW_1VAL)
        {
            List<T> _list = new List<T>();
            switch (format)
            {
                case DAS_FORMAT_ENUM.BINARY:
                    break;
                case DAS_FORMAT_ENUM.TXT_ROW_1VAL:
                    using (StreamReader sr = new StreamReader(path))
                    {
                        String line = sr.ReadLine();
                        while (line != null)
                        {
                            if (line != "")
                            {
                                _list.Add((T)Convert.ChangeType(line, typeof(T)));
                                line = sr.ReadLine();
                            }
                        }
                    }
                    break;
                case DAS_FORMAT_ENUM.TXT_1ROW_NVALS_COMMA_SEPARTOR:
                    break;
                case DAS_FORMAT_ENUM.TXT_NO_FORMAT:
                    break;
                default:
                    break;
            }

            // This will surely create ambiguity for user, thus, it has to be implemented differently!
            //  list = _list as List<T>;
            return _list as List<T>;
        }

        /// <summary>
        /// Sorts list with new thread,
        /// with it´s chilrden method via delegate.
        /// </summary>
        public void Sort()
        {
            Dispatcher.FromThread(front_END.UI_Thread).Invoke(() =>
            {
                front_END.SortButton.Content = "Sorting!";
                front_END.SortButton.Visibility = Visibility.Hidden;
            });
            Thread t = new Thread(delegate ()
            {
                _Debug.AddMessage<object>(new Message<object>("Thread for sort started."));
                OnSortingStart(new SortingStartedEventArgs());
                startedSortingDate = DateTime.Now;
                _DoSort(this.list, front_END);
            });
            t.Start();
        }

        /// <summary>
        /// Checks if list is sorted, then if not, sorts it. 
        /// Then, recursively calls itself, check if list is sorted,
        /// if it is, it creates a sorted file.
        /// </summary>
        /// <returns>True if file was created, in every other case, returns false</returns>
        public bool CreateSortedFile(bool recursion = false)
        {
            if (!recursion && !IsSorted())
            {
                _Debug.AddMessage<object>(new Message<object>("Sorting!"));
                // try to sort here...
                Sort();
                CreateSortedFile(true);
            }
            else
            {
                OnFileSorted(new FileSortedEventArgs(startedSortingDate));
                _Debug.AddMessage<object>(new Message<object>("Creating sorted file [" + filePath + "_sorted" + ".txt" + "]"));
                Thread.Sleep(100);
                using (StreamWriter sr = new StreamWriter(filePath.Substring(0, filePath.Length - 4) + "_sorted" + ".txt", false, Encoding.UTF8))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        sr.WriteLine(list[i]);
                    }
                }
                string sortType = "";
                MessageBox.Show("Sorted file[" + filePath.Substring(0, filePath.Length - 4) + "_sorted" + ".txt" + "] \n has been created, \n using " + GetType().Name.Substring(0, GetType().Name.Length - 2) + ".");
                Dispatcher.FromThread(front_END.UI_Thread).Invoke(() =>
                {
                    front_END.SortButton.Content = "Cont_Init";
                    front_END.SortButton.Visibility = Visibility.Visible;
                });
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method should only be used as final IS_SORTED if statement.
        /// It will itterate trough every element in List<T> and should not
        /// be used recklesly.
        /// Many sorts will have different impl. thus requirements for 
        /// complete list check.
        /// </summary>
        /// <param name="order">What type of order mechanism Sort is using.</param>
        /// <returns>True if elements in list are sorted by according order.</returns>
        public bool IsSorted(Order_Enum order = Order_Enum.ASCENDING)
        {
            _Debug.AddMessage<object>(new Message<object>("Is sorted check[" + Enum.GetName(typeof(Order_Enum), order) + "]: [" + this.GetType().Name.Substring(0,this.GetType().Name.Length-2) + "]"));
            switch (order)
            {
                case Order_Enum.ASCENDING:
                    foreach (var (item, index) in list.WithIndex())
                    {
                        // Debug.WriteLine($"{index}: {item}");
                        // Check for incremental val size order;
                        if (index < list.Count - 1)
                        {
                            if (!IncrementalCheck(item, list.ElementAt(index + 1)))
                            {
                                _Debug.AddMessage<object>(new Message<object>("{False}", MessageType_ENUM.________));
                                return false;
                            }
                        }
                    }
                    break;
                case Order_Enum.DESCENDING:
                    foreach (var (item, index) in list.WithIndex())
                    {
                        // Debug.WriteLine($"{index}: {item}");
                        // Check for incremental val size order;
                        if (index < list.Count - 1)
                        {
                            if (!DecrementalCheck(item, list.ElementAt(index + 1)))
                            {
                                _Debug.AddMessage<object>(new Message<object>("{False}", MessageType_ENUM.________));
                                return false;
                            }
                        }
                    }
                    break;
                case Order_Enum.CANNOT_BE_SPECIFIED:
                    return false;
                case Order_Enum.ASC_LENGTH:

                    break;
                case Order_Enum.DESC_LENGTH:
                    break;
                default:
                    break;
            }
            return true;
        }

        /// <summary>
        /// Checks for value of specified types. 
        /// Check MC documentation if usure what ConvertTo double excatly does.
        /// Could not be used on types which have not defined numeric values.
        /// If you still want to compare non-numeric type, copare them by their size property. 
        /// </summary>
        /// <param name="type1">Checked elem. in list</param>
        /// <param name="type2">(Checked elem. in list) + 1</param>
        /// <returns>True if type1 < type2</returns>
        private bool IncrementalCheck(T type1, T type2)
        {
            if (Convert.ToDouble(type1) <= Convert.ToDouble(type2))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks for value of specified types. 
        /// Check MC documentation if usure what ConvertTo double excatly does.
        /// Could not be used on types which have not defined numeric values.
        /// If you still want to compare non-numeric type, copare them by their size property. 
        /// </summary>
        /// <param name="type1">Checked elem. in list</param>
        /// <param name="type2">(Checked elem. in list) + 1 </param>
        /// <returns>True if type1 > type2</returns>
        private bool DecrementalCheck(T type1, T type2)
        {
            if (Convert.ToDouble(type1) >= Convert.ToDouble(type2))
            {
                return true;
            }
            return false;
        }

        protected virtual void OnSortingStart(SortingStartedEventArgs e)
        {
            _Debug.AddMessage<object>(new Message<object>(MethodBase.GetCurrentMethod().Name, MessageType_ENUM.Event));
            FileSortStart?.Invoke(this, e);
            _Debug.AddMessage<object>(new Message<object>(e.startDateTime.TimeOfDay));
        }

        protected virtual void OnFileSorted(FileSortedEventArgs e)
        {
            this.isSorted = true;
            _Debug.AddMessage<object>(new Message<object>(MethodBase.GetCurrentMethod().Name, MessageType_ENUM.Event));
            FileSorted?.Invoke(this, e);
            _Debug.AddMessage<object>(new Message<object>("Started: [" + e.startDateTime.TimeOfDay + "]", MessageType_ENUM.________));
            _Debug.AddMessage<object>(new Message<object>("Finished: [" + e.createdDateTime.TimeOfDay + "]", MessageType_ENUM.________));
            _Debug.AddMessage<object>(new Message<object>("Delta: [" + e.GetDeltaTime() + "]", MessageType_ENUM.________));
        }

        public Exception CouldNotBeSorted()
        {
            Exception ex = new Exception("File could not be sorted.[" + Enum.GetName(typeof(Order_Enum), this.order_Enum) + "]: [" + this.GetType().Name + "]");
            var task = Task.Run(async () =>
            {
                await _Debug.AddMessage_Assync<object>(new Message<object>(ex.Message, MessageType_ENUM.Exception));
            });
            task.Wait();
            throw ex;
        }
    }
}
