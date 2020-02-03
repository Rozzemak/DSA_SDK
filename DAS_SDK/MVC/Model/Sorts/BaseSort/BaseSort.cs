using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DAS_SDK.MVC.Enums;
using DAS_SDK.MVC.Model.Debug;
using DAS_SDK.MVC.Model.FrontEND;

namespace DAS_SDK.MVC.Model.Sorts.BaseSort
{
    class BaseSort<T>
    {
        private DateTime startedSortingDate;

        public List<T> List;
        public OrderEnum OrderEnum;
        public string FilePath;
        public delegate List<T> DoSort(List<T> list, SortFrontEnd frontEnd);
        public DoSort DoSortBase;
        private SortFrontEnd frontEnd;
        protected BaseDebug Debug;
        public bool IsSrted;
        
        /// <summary>
        /// This enum tells you sortstate of this sort.
        /// </summary>
        public SortState SortState;
        /// <summary>
        /// Event, that is risen upon sort start.
        /// </summary>
        public event EventHandler FileSortStart;
        /// <summary>
        /// Event, that is risen upon file sorted. (Successfully)
        /// </summary>
        public event EventHandler FileSorted;

        protected BaseSort(BaseDebug debug, SortFrontEnd frontEnd, string path = "unsorted.txt", OrderEnum order = OrderEnum.Ascending)
        {
            this.Debug = debug;
            this.frontEnd = frontEnd;
            this.FilePath = path;
            this.List = ReadFile(path);
            this.OrderEnum = order;
        }

        /// <summary>
        /// This constructor is not finished, do not use.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="order"></param>
        protected BaseSort(List<T> list, OrderEnum order = OrderEnum.Ascending)
        {
            this.List = list;
            this.OrderEnum = order;
        }

        /// <summary>
        /// While reading a file, 
        /// it fills list with according
        /// format elements.
        /// </summary>
        /// <param name="path">Path to file prefeer {currect_working_dir}</param>
        /// <param name="format">File format, could be txt with different txt format.</param>
        /// <returns>Filled list, mus</returns>
        public List<T> ReadFile(string path, DasFormatEnum format = DasFormatEnum.TxtRow1Val)
        {
            var list = new List<T>();
            switch (format)
            {
                case DasFormatEnum.Binary:
                    break;
                case DasFormatEnum.TxtRow1Val:
                    using (var sr = new StreamReader(path))
                    {
                        var line = sr.ReadLine();
                        while (line != null)
                        {
                            if (line != "")
                            {
                                list.Add((T)Convert.ChangeType(line, typeof(T)));
                                line = sr.ReadLine();
                            }
                        }
                    }
                    break;
                case DasFormatEnum.Txt1RowNvalsCommaSepartor:
                    break;
                case DasFormatEnum.TxtNoFormat:
                    break;
                default:
                    break;
            }

            // This will surely create ambiguity for user, thus, it has to be implemented differently!
            //  list = _list as List<T>;
            return list as List<T>;
        }

        /// <summary>
        /// Sorts list with new thread,
        /// with it´s chilrden method via delegate.
        /// </summary>
        public void Sort()
        {
            Dispatcher.FromThread(frontEnd.UiThread).Invoke(() =>
            {
                frontEnd.ControllerInitButton.Content = "Sorting!";
                frontEnd.ControllerInitButton.Visibility = Visibility.Hidden;
            });
            var t = new Thread(delegate ()
            {
                Debug.AddMessage<object>(new Message<object>("Thread for sort started."));
                OnSortingStart(new SortingStartedEventArgs<T>(this));
                startedSortingDate = DateTime.Now;
                DoSortBase(this.List, frontEnd);
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
                Debug.AddMessage<object>(new Message<object>("Sorting!"));
                // try to sort here...
                Sort();
                CreateSortedFile(true);
            }
            else
            {
                OnFileSorted(new FileSortedEventArgs<T>(startedSortingDate,this));
                Debug.AddMessage<object>(new Message<object>("Creating sorted file [" + FilePath + "_sorted" + ".txt" + "]"));
                Thread.Sleep(100);
                using (var sr = new StreamWriter(FilePath.Substring(0, FilePath.Length - 4) + "_sorted" + ".txt", false, Encoding.UTF8))
                {
                    for (var i = 0; i < List.Count; i++)
                    {
                        sr.WriteLine(List[i]);
                    }
                }
                var sortType = "";
                MessageBox.Show("Sorted file[" + FilePath.Substring(0, FilePath.Length - 4) + "_sorted" + ".txt" + "] \n has been created, \n using " + GetType().Name.Substring(0, GetType().Name.Length - 2) + ".");
                Dispatcher.FromThread(frontEnd.UiThread)
                    ?.Invoke(() =>
                {
                    frontEnd.ControllerInitButton.Content = "Cont_Init";
                    frontEnd.ControllerInitButton.Visibility = Visibility.Visible;
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
        public bool IsSorted(OrderEnum order = OrderEnum.Ascending)
        {
            Debug.AddMessage<object>(new Message<object>("Is sorted check[" + Enum.GetName(typeof(OrderEnum), order) + "]: [" + this.GetType().Name.Substring(0,this.GetType().Name.Length-2) + "]"));
            switch (order)
            {
                case OrderEnum.Ascending:
                    foreach (var (item, index) in List.WithIndex())
                    {
                        // Debug.WriteLine($"{index}: {item}");
                        // Check for incremental val size order;
                        if (index < List.Count - 1)
                        {
                            if (!IncrementalCheck(item, List.ElementAt(index + 1)))
                            {
                                Debug.AddMessage<object>(new Message<object>("{False}", MessageTypeEnum.________));
                                return false;
                            }
                        }
                    }
                    break;
                case OrderEnum.Descending:
                    foreach (var (item, index) in List.WithIndex())
                    {
                        // Debug.WriteLine($"{index}: {item}");
                        // Check for incremental val size order;
                        if (index < List.Count - 1)
                        {
                            if (!DecrementalCheck(item, List.ElementAt(index + 1)))
                            {
                                Debug.AddMessage<object>(new Message<object>("{False}", MessageTypeEnum.________));
                                return false;
                            }
                        }
                    }
                    break;
                case OrderEnum.CannotBeSpecified:
                    return false;
                case OrderEnum.AscLength:
                    break;
                case OrderEnum.DescLength:
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

        protected virtual void OnSortingStart(SortingStartedEventArgs<T> e)
        {
            Debug.AddMessage<object>(new Message<object>(MethodBase.GetCurrentMethod().Name, MessageTypeEnum.Event));
            FileSortStart?.Invoke(this, e);
            Debug.AddMessage<object>(new Message<object>(e.StartDateTime.TimeOfDay, MessageTypeEnum.________));
            Debug.AddMessage<object>(new Message<object>("Sort state: "+ SortState.ToString(), MessageTypeEnum.________));
        }

        protected virtual void OnFileSorted(FileSortedEventArgs<T> e)
        {
            this.IsSrted = true;
            Debug.AddMessage<object>(new Message<object>(MethodBase.GetCurrentMethod().Name, MessageTypeEnum.Event));
            FileSorted?.Invoke(this, e);
            Debug.AddMessage<object>(new Message<object>("Started: [" + e.StartDateTime.TimeOfDay + "]", MessageTypeEnum.________));
            Debug.AddMessage<object>(new Message<object>("Finished: [" + e.CreatedDateTime.TimeOfDay + "]", MessageTypeEnum.________));
            Debug.AddMessage<object>(new Message<object>("Delta: [" + e.GetDeltaTime() + "]", MessageTypeEnum.________));
            Debug.AddMessage<object>(new Message<object>("Sort state: " + SortState.ToString(), MessageTypeEnum.________));
        }

        public Exception CouldNotBeSorted()
        {
            var ex = new Exception("File could not be sorted.[" + Enum.GetName(typeof(OrderEnum), this.OrderEnum) + "]: [" + this.GetType().Name + "]");
            var task = Task.Run(async () =>
            {
                await Debug.AddMessage_Assync<object>(new Message<object>(ex.Message, MessageTypeEnum.Exception));
            });
            task.Wait();
            throw ex;
        }
    }
}
