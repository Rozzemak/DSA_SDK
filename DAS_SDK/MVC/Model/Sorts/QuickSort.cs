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
    class QuickSort<T> : BaseSort<T> where T : IComparable
    {
        Random random = new Random();

        public QuickSort(BaseDebug debug, SortFrontEnd frontEnd, string path = "sorted.txt")
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
            else
            {
                throw new Exception("File could not be sorted.");
            }
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
                // Try to sort here, if not successfull, then throw ex.
                QuickLogic(frontEnd);
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

        private void QuickLogic(SortFrontEnd frontEnd)
        {
            Recursion(0, List.Count, frontEnd);
        }

        private bool Recursion(int left, int right, SortFrontEnd frontEnd)
        {
            if (left < right)
            {
                UpdateUserInterface(right - (right / 2 - (left / 2)), frontEnd); // Tried many variants, this works;
                var bounds = left;
                for (var i = left + 1; i < right; i++)
                {
                    if (List[i].CompareTo(List[left]) == -1)
                    {
                        Swap(i, ++bounds);
                    }
                }
                Swap(left, bounds);
                Recursion(left, bounds, frontEnd);
                Recursion(bounds +1, right , frontEnd);
                return false;
            }
            return true;
        }

        private void Swap(int left, int right)
        {
            var tmp = List[right];
            List[right] = List[left];
            List[left] = tmp;
        }

        private void UpdateUserInterface(int cycleOfSort, SortFrontEnd frontEnd)
        {
            // I dont know, if is safe to do front end public due to thread unsafe exceptions.
            if (frontEnd.ProgressValIncrement != 0)
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

