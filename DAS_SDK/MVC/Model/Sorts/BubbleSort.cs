using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using DAS_SDK.MVC.Model.Debug;
using DAS_SDK.MVC.Model.FrontEND;
using DAS_SDK.MVC.Model.Sorts.BaseSort;

namespace DAS_SDK.MVC.Model.Sorts
{
    class BubbleSort<T> : BaseSort<T> where T : IComparable
    {
        public BubbleSort(BaseDebug debug, SortFrontEnd frontEnd, string path = "sorted.txt")
            : base(debug, frontEnd, path)
        {
            if (double.TryParse(List[0].ToString(), out var test))
            {
                //_DoSortBase = Sort;
                DoSortBase = ObjSort;
            }
            else
            {
                MessageBox.Show("Generic object val sort not yet fully impl.");
                DoSortBase = ObjSort;
            }
        }

        /// <summary>
        /// DEPRECEATED
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<T> Sort(List<T> list)
        {
            var doubleList = list.Select(x => Double.Parse(x.ToString())).ToList();
            for (var i = 0; i < list.Count - 1; i++)
            {
                for (var j = 0; j < list.Count - i - 1; j++)
                {
                    if (doubleList[j + 1] < doubleList[j])
                    {
                        var tmp = doubleList[j + 1];
                        doubleList[j + 1] = doubleList[j];
                        doubleList[j] = tmp;
                    }
                }
            }
            this.List = doubleList.Select(x => (T)Convert.ChangeType(x, typeof(T))).ToList();
            if (IsSorted())
            {
                CreateSortedFile();
                return list;
            }
            if (IsSorted())
            {
                CreateSortedFile();
                return list;
            }
            else throw CouldNotBeSorted();
        }

        private List<T> ObjSort(List<T> list, SortFrontEnd frontEnd)
        {
            if (!IsSorted())
            {
                Dispatcher.FromThread(frontEnd.UiThread).Invoke(() =>
                {
                    frontEnd.ProgressBar.Value = 0;
                    frontEnd.ProgressBar.Maximum = list.Count;
                    // (pozn. => for progress bar precision
                    frontEnd.ProgressValIncrement = list.Count / 10000;
                });
                BubbleLogic(frontEnd);
                Dispatcher.FromThread(frontEnd.UiThread).Invoke(() =>
                {
                    // Just for the user, to look nice.
                    frontEnd.ProgressBar.Value = frontEnd.ProgressBar.Maximum;
                });
            }
            if (IsSorted())
            {
                CreateSortedFile();
                return list;
            }
            else throw CouldNotBeSorted();
        }

        private void BubbleLogic(SortFrontEnd frontEnd)
        {
            //var gpu = Gpu.Default;
            for (var i = 0; i < List.Count - 1; i++)
            {
                UpdateUserInterface(i, frontEnd);
                for (var j = 0; j < List.Count - i - 1; j++)
                {
                    if (List[j + 1].CompareTo(List[j]) == -1)
                    {
                        var tmp = List[j + 1];
                        List[j + 1] = List[j];
                        List[j] = tmp;
                    }
                }
            }
           // gpu.EvalAction(delegate () {
           //     T tmp = list[j + 1];
           //     list[j + 1] = list[j];
           //     list[j] = tmp;
           //  });
        }

        private void UpdateUserInterface(int cycleOfSort, SortFrontEnd frontEnd)
        {
            // I dont know, if is safe to do front end public due to thread unsafe exceptions.       
            if(frontEnd.ProgressValIncrement != 0)
            {
                if (cycleOfSort % frontEnd.ProgressValIncrement == 0)
                {
                    Dispatcher.FromThread(frontEnd.UiThread).Invoke(() =>
                    {
                        frontEnd.ProgressBar.Value += frontEnd.ProgressValIncrement;
                    });
                }
            }
        }
    }

}

