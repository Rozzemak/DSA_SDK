using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAS_SDK.MVC.Model.Sorts.Base_Sort;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using DAS_SDK.MVC.Model.Debug;
using DAS_SDK.MVC.Model.Front_END;

namespace DAS_SDK.MVC.Model.Sorts
{
    class Quick_Sort<T> : Base_Sort<T> where T : IComparable
    {
        Random random = new Random();

        public Quick_Sort(Base_Debug debug, Sort_Front_END front_END, string path = "sorted.txt")
            : base(debug, front_END, path)
        {
            if (double.TryParse(list[0].ToString(), out double test))
            {
                //_DoSort = Sort;
                _DoSort = ObjSort;
            }
            else
            {
                MessageBox.Show("Generic object val sort not yet fully impl.");
                _DoSort = ObjSort;
            }
        }

        /// <summary>
        /// DEPRECEATED
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<T> Sort(List<T> list)
        {
            List<Double> doubleList = list.Select(x => Double.Parse(x.ToString())).ToList();
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = 0; j < list.Count - i - 1; j++)
                {
                    if (doubleList[j + 1] < doubleList[j])
                    {
                        double tmp = doubleList[j + 1];
                        doubleList[j + 1] = doubleList[j];
                        doubleList[j] = tmp;
                    }
                }
            }
            this.list = doubleList.Select(x => (T)Convert.ChangeType(x, typeof(T))).ToList();
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

        private List<T> ObjSort(List<T> list, Sort_Front_END front_END)
        {
            if (!IsSorted())
            {
                Dispatcher.FromThread(front_END.UI_Thread).Invoke(() =>
                {
                    front_END.progressBar.Value = 0;
                    front_END.progressBar.Maximum = list.Count;
                    // (pozn. => for progress bar precision
                    front_END.ProgressValIncrement = list.Count / 10000;
                });
                // Try to sort here, if not successfull, then throw ex.
                QuickLogic(front_END);
                Dispatcher.FromThread(front_END.UI_Thread).Invoke(() =>
                {
                    // Just for the user, to look nice.
                    front_END.progressBar.Value = front_END.progressBar.Maximum;
                });
            }
            if (IsSorted())
            {
                CreateSortedFile();
                return list;
            }
            else throw CouldNotBeSorted();
        }

        private void QuickLogic(Sort_Front_END front_END)
        {
            Recursion(0, list.Count, front_END);
        }

        private bool Recursion(int left, int right, Sort_Front_END front_END)
        {
            if (left < right)
            {
                UpdateUserInterface(right - (right / 2 - (left / 2)), front_END); // Tried many variants, this works;
                int bounds = left;
                for (int i = left + 1; i < right; i++)
                {
                    if (list[i].CompareTo(list[left]) == -1)
                    {
                        Swap(i, ++bounds);
                    }
                }
                Swap(left, bounds);
                Recursion(left, bounds, front_END);
                Recursion(bounds +1, right , front_END);
                return false;
            }
            return true;
        }

        private void Swap(int left, int right)
        {
            T tmp = list[right];
            list[right] = list[left];
            list[left] = tmp;
        }

        private void UpdateUserInterface(int cycleOfSort, Sort_Front_END front_END)
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
    }

}

