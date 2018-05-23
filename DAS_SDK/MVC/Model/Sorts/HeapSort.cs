using DAS_SDK.MVC.Model.Debug;
using DAS_SDK.MVC.Model.Front_END;
using DAS_SDK.MVC.Model.Sorts.Base_Sort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DAS_SDK.MVC.Model.Sorts
{
    class HeapSort<T> : Base_Sort<T> where T : IComparable
    {
        private int heapSize;

        public HeapSort(Base_Debug debug, Sort_Front_END front_END, string path = "unsorted.txt", Order_Enum order = Order_Enum.ASCENDING)
            : base(debug, front_END, path, order)
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
                HeapLogic(front_END);
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

        private void BuildHeap(List<T> arr)
        {
            heapSize = arr.Count - 1;
            for (int i = heapSize / 2; i >= 0; i--)
            {
                DoHeap_Rec(arr, i);
            }
        }

        private void HeapLogic(Sort_Front_END front_END)
        {
            PerformHeapSort(list);
        }

        private void Swap(List<T> list, int x, int y)//function to swap elements
        {
            T temp = list[x];
            list[x] = list[y];
            list[y] = temp;
        }
        private void DoHeap_Rec(List<T> list, int index)
        {
            int left = 2 * index + 1;
            int right = 2 * index + 2;
            int largest = index;
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
            for (int i = list.Count - 1; i >= 0; i--)
            {
                Swap(list, 0, i);
                heapSize--;
                DoHeap_Rec(list, 0);
            }
        }
    }
}
