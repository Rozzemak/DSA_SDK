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

    class Base_Search<T> where T : IComparable
    {
        Random random = new Random();

        protected List<T> matches = new List<T>();
        protected T searched = default(T);
        protected Front_END.Front_END front_END;
        protected Sort_Type sort_Type;
        protected Base_Debug _Debug;

        public event EventHandler Searching;

        protected delegate List<T> DoSearch(List<T> sortedList, Front_END.Front_END front_END);
        protected DoSearch _DoSearch;
        private object sort = new object();

        /// <summary>
        /// This event is raised upon searching start. (After file is sorted || if it is sorted)
        /// </summary>
        protected event EventHandler SearchStart;
        /// <summary>
        /// This event is raide upon searching end. (After result are found.)
        /// </summary>
        protected event EventHandler SearchEnd;

        public Base_Search(string path, Base_Debug debug, Front_END.Front_END front_END, T searched, Sort_Type sort_Type)
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
                    sort = new Quick_Sort<T>(debug, front_END, path);
                    break;
                case Sort_Type.Radix:
                    sort = new Radix_Sort<T>(debug, front_END, path);
                    break;
                case Sort_Type.none:
                    throw new Exception("You cannot search in unsorted file!, yet.");
            }
            this.searched = searched;
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
                    return (sort as Quick_Sort<T>);
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
            Thread.Sleep(5);
            throw ex;
        }
    }
}
