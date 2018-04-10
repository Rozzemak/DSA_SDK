using DAS_SDK.MVC.Model.Debug;
using DAS_SDK.MVC.Model.Sorts;
using DAS_SDK.MVC.Model.Sorts.Base_Sort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DAS_SDK.MVC.Model.Search
{
    enum Sort_Type
    {
        Bubble,
        Select,
        Quick,
        Radix,
        none 
    }

    class Search_Base<T> where T : IComparable
    {
        List<T> matches = new List<T>();
        Random random = new Random();
        T searched = default(T);
        Front_END.Front_END front_END;
        Sort_Type sort_Type;
        Base_Debug _Debug;

        protected delegate List<T> DoSearch(List<T> sortedList, Front_END.Front_END front_END);
        protected DoSearch _DoSearch;
        private object sort = new object();

        public Search_Base(string path, Base_Debug debug, Front_END.Front_END front_END, T searched, Sort_Type sort_Type)
        {
            this.sort_Type = sort_Type;
            this._Debug = debug;
            this.front_END = front_END;
            switch (sort_Type)
            {
                case Sort_Type.Bubble:
                    sort = new Bubble_Sort<T>(debug, front_END, path);
                    break;
                case Sort_Type.Select:
                    sort = new Select_Sort<T>(debug, front_END, path);
                    break;
                case Sort_Type.Quick:
                    sort = new Quick_Soort<T>(debug, front_END, path);
                    break;
                case Sort_Type.Radix:
                    sort = new Radix_Sort<T>(debug, front_END, path);
                    break;
                case Sort_Type.none:
                    throw new Exception("You cannot search in unsorted file!, yet.");
            }
            this.searched = searched;
            if (double.TryParse(GetSort().list[0].ToString(), out double test))
            {
                if (!GetSort().IsSorted())
                {
                    // This is very dirty quick workaround of async wait. Though it´s not async. Wanted to catch an event here, but :P.
                    GetSort().Sort();
                    while (!GetSort().isSorted)
                    {

                    }             
                } else
                {
                    // maybe something clever here ?
                }
                // This is quick impl for Binary search start, but this should have it´s own class.
                _DoSearch = B_Search;
            }
        }

        /// <summary>
        /// Sorts list with new thread,
        /// with it´s chilrden method via delegate.
        /// </summary>
        public void Search()
        {
            Dispatcher.FromThread(front_END.UI_Thread).Invoke(() =>
            {
                front_END.SortButton.Content = "Searching!";
                front_END.SortButton.Visibility = Visibility.Hidden;
            });
            Thread t = new Thread(delegate ()
            {
                _Debug.AddMessage<object>(new Message<object>("Thread for searching started."));
               // OnSortingStart(new SortingStartedEventArgs());
               // startedSortingDate = DateTime.Now;
               _DoSearch(GetSort().list, front_END);
            });
            t.Start();
        }

        private List<T> B_Search(List<T> list, Front_END.Front_END front_END)
        {
            bool result = false;
            if (GetSort().IsSorted())
            {
                Dispatcher.FromThread(front_END.UI_Thread).Invoke(() =>
                {
                    front_END.progressBar.Value = 0;
                    front_END.progressBar.Maximum = list.Count;
                    // (pozn. => for progress bar precision
                    front_END.ProgressValIncrement = list.Count / 10000;
                });
                // Try to sort here, if not successfull, then throw ex.
                result = Search_Logic(front_END);
                Dispatcher.FromThread(front_END.UI_Thread).Invoke(() =>
                {
                    // Just for the user, to look nice.
                    front_END.progressBar.Value = front_END.progressBar.Maximum;
                });
            }
            if (result)
            {
                var msg = new Message<object>("");
                foreach (var item in matches)
                {
                    msg.MessageContent += "[" + item + "]";
                }
                _Debug.AddMessage<object>(msg);
                return matches;
            }
            else throw CouldNotBeFound();
        }

        protected virtual bool Search_Logic(Front_END.Front_END front_END)
        {
            int l = 0, r = GetSort().list.Count-1;
            while (l < r)
            {
                int m = l + (r - l) / 2;
                // Check if searched is present at mid
                if (GetSort().list[m].CompareTo(searched) == 0)
                {
                    matches.Add(GetSort().list[m]);
                    int _m = m;
                    while (GetSort().list[Math.Abs(_m--)].CompareTo(searched) == 0)
                    {
                        matches.Add(GetSort().list[_m]);
                    }
                    int _c = m;
                    while (GetSort().list[_c++].CompareTo(searched) == 0)
                    {
                        matches.Add(GetSort().list[_c]);
                    }
                    for (int i = 0; i < matches.Count; i++)
                    {
                        if ((matches[i]).CompareTo(searched) != 0)
                        {
                            matches[i] = default(T);
                        }
                    }
                    return true;
                }
                // If searched greater, ignore left halfy
                if (GetSort().list[m].CompareTo(searched) < 0)
                {
                    l = m + 1;
                }// If searched is smaller, ignore right half
                else if((GetSort().list[m].CompareTo(searched) > 0))
                {
                    r = m - 1;
                }
            }
            return false;
        }

        protected void UpdateUserInterface(int cycleOfSort, Front_END.Front_END front_END)
        {
            // I dont know, if is safe to do front end public due to thread unsafe exceptions.
            if (cycleOfSort % front_END.ProgressValIncrement == 0)
            {
                Dispatcher.FromThread(front_END.UI_Thread).Invoke(() =>
                {
                    front_END.progressBar.Value += front_END.ProgressValIncrement;
                });
            }
        }

        public Base_Sort<T> GetSort()
        {
            switch (this.sort_Type)
            {
                case Sort_Type.Bubble:
                    return (sort as Bubble_Sort<T>);
                case Sort_Type.Select:
                    return (sort as Select_Sort<T>);
                case Sort_Type.Quick:
                    return (sort as Quick_Soort<T>);
                case Sort_Type.Radix:
                    return (sort as Radix_Sort<T>);
                case Sort_Type.none:
                    return null;
            }
            return null;
        }

        public Exception CouldNotBeFound()
        {
            Exception ex = new Exception("Did not find any matches[" + searched + "] with:[" + this.GetType().Name + "]");
            var task = Task.Run(async () =>
            {
                await _Debug.AddMessage_Assync<object>(new Message<object>(ex.Message, MessageType_ENUM.Exception));
            });
            task.Wait();
            throw ex;
        }
    }
}
