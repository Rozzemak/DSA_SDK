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

namespace DAS_SDK.MVC.Model.Sorts
{
    class Bubble_Sort<T> : Base_Sort<T> where T : IComparable
    {
        public Bubble_Sort(Base_Debug debug,Front_END.Front_END front_END)
            : base(debug,front_END)
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
            if (IsSorted())
            {
                CreateSortedFile();
                return list;
            }
            else throw CouldNotBeSorted();
        }

        private List<T> ObjSort(List<T> list, Front_END.Front_END front_END)
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
                BubbleLogic(front_END);
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

        private void BubbleLogic(Front_END.Front_END front_END)
        {
            //var gpu = Gpu.Default;
            for (int i = 0; i < list.Count - 1; i++)
            {
                UpdateUserInterface(i, front_END);
                for (int j = 0; j < list.Count - i - 1; j++)
                {
                    if (list[j + 1].CompareTo(list[j]) == -1)
                    {
                        T tmp = list[j + 1];
                        list[j + 1] = list[j];
                        list[j] = tmp;
                    }
                }
            }
           // gpu.EvalAction(delegate () {
           //     T tmp = list[j + 1];
           //     list[j + 1] = list[j];
           //     list[j] = tmp;
           //  });
        }

        private void UpdateUserInterface(int cycleOfSort, Front_END.Front_END front_END)
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

