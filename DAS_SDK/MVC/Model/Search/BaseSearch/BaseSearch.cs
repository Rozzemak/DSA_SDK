using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DAS_SDK.MVC.Model.Debug;
using DAS_SDK.MVC.Model.FrontEND;
using DAS_SDK.MVC.Model.Sorts;
using DAS_SDK.MVC.Model.Sorts.BaseSort;

namespace DAS_SDK.MVC.Model.Search.BaseSearch
{
    enum SortType
    {
        Bubble,
        Select,
        Quick,
        Radix,
        Heap,
        None 
    }

    class BaseSearch<T> where T : IComparable
    {
        Random random = new Random();

        protected List<T> Matches = new List<T>();
        protected T Searched = default(T);
        protected SortFrontEnd FrontEnd;
        protected SortType SortType;
        protected BaseDebug Debug;

        public event EventHandler Searching;

        protected delegate List<T> DoSearch(List<T> sortedList, SortFrontEnd frontEnd);
        protected DoSearch DoSearchBase;
        private object sort = new object();

        /// <summary>
        /// This event is raised upon searching start. (After file is sorted || if it is sorted)
        /// </summary>
        protected event EventHandler SearchStart;
        /// <summary>
        /// This event is raide upon searching end. (After result are found.)
        /// </summary>
        protected event EventHandler SearchEnd;

        public BaseSearch(string path, BaseDebug debug, SortFrontEnd frontEnd, T searched, SortType sortType)
        {
            this.SortType = sortType;
            this.Debug = debug;
            this.FrontEnd = frontEnd;
            switch (sortType)
            {
                case SortType.Bubble:
                    sort = new BubbleSort<T>(debug, frontEnd, path);
                    break;
                case SortType.Select:
                    sort = new SelectSort<T>(debug, frontEnd, path);
                    break;
                case SortType.Quick:
                    sort = new QuickSort<T>(debug, frontEnd, path);
                    break;
                case SortType.Radix:
                    sort = new RadixSort<T>(debug, frontEnd, path);
                    break;
                case SortType.None:
                    throw new Exception("You cannot search in unsorted file!, yet.");
            }
            this.Searched = searched;
        }

        /// <summary>
        /// Sorts list with new thread,
        /// with it´s chilrden method via delegate.
        /// </summary>
        public void Search()
        {
            Dispatcher.FromThread(FrontEnd.UiThread).Invoke(() =>
            {
                FrontEnd.ControllerInitButton.Content = "Searching!";
                FrontEnd.ControllerInitButton.Visibility = Visibility.Hidden;
            });
            var t = new Thread(delegate ()
            {
                Debug.AddMessage<object>(new Message<object>("Thread for searching started."));
               // OnSortingStart(new SortingStartedEventArgs());
               // startedSortingDate = DateTime.Now;
               DoSearchBase(GetSort().List, FrontEnd);
            });
            t.Start();
        }

        protected void UpdateUserInterface(int cycleOfSort, SortFrontEnd frontEnd)
        {
            // I dont know, if is safe to do front end public due to thread unsafe exceptions.
            if (cycleOfSort % frontEnd.ProgressValIncrement == 0)
            {
                Dispatcher.FromThread(frontEnd.UiThread).Invoke(() =>
                {
                    frontEnd.ProgressBar.Value += frontEnd.ProgressValIncrement;
                });
            }
        }

        public BaseSort<T> GetSort()
        {
            switch (this.SortType)
            {
                case SortType.Bubble:
                    return (sort as BubbleSort<T>);
                case SortType.Select:
                    return (sort as SelectSort<T>);
                case SortType.Quick:
                    return (sort as QuickSort<T>);
                case SortType.Radix:
                    return (sort as RadixSort<T>);
                case SortType.None:
                    return null;
            }
            return null;
        }

        public Exception CouldNotBeFound()
        {
            var ex = new Exception("Did not find any matches[" + Searched + "] with:[" + this.GetType().Name + "]");
            var task = Task.Run(async () =>
            {
                await Debug.AddMessage_Assync<object>(new Message<object>(ex.Message, MessageTypeEnum.Exception));
            });
            task.Wait();
            Thread.Sleep(5);
            throw ex;
        }
    }
}
