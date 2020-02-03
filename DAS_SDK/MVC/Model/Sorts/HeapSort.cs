using DAS_SDK.MVC.Model.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DAS_SDK.MVC.Model.FrontEND;
using DAS_SDK.MVC.Model.Sorts.BaseSort;

namespace DAS_SDK.MVC.Model.Sorts
{
    class HeapSort<T> : BaseSort<T> where T : IComparable
    {
        private int heapSize;

        public HeapSort(BaseDebug debug, SortFrontEnd frontEnd, string path = "unsorted.txt", OrderEnum order = OrderEnum.Ascending)
            : base(debug, frontEnd, path, order)
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
                HeapLogic(frontEnd);
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

        private void BuildHeap(List<T> arr)
        {
            heapSize = arr.Count - 1;
            for (var i = heapSize / 2; i >= 0; i--)
            {
                DoHeap_Rec(arr, i);
            }
        }

        private void HeapLogic(SortFrontEnd frontEnd)
        {
            PerformHeapSort(List);
        }

        private void Swap(List<T> list, int x, int y)//function to swap elements
        {
            var temp = list[x];
            list[x] = list[y];
            list[y] = temp;
        }
        private void DoHeap_Rec(List<T> list, int index)
        {
            var left = 2 * index + 1;
            var right = 2 * index + 2;
            var largest = index;
            if (left <= heapSize && list[left].CompareTo(list[index]) > 0)
            {
                largest = left;
            } 
            if (right <= heapSize && list[right].CompareTo(list[largest]) > 0)
            {
                largest = right;
            }
            if (largest != index)
            {
                Swap(list, index, largest);
                DoHeap_Rec(list, largest);
            }
        }
        public void PerformHeapSort(List<T> list)
        {
            BuildHeap(list);
            for (var i = list.Count - 1; i >= 0; i--)
            {
                Swap(list, 0, i);
                heapSize--;
                DoHeap_Rec(list, 0);
            }
        }
    }
}
