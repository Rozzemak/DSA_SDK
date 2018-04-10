using DAS_SDK.MVC.Model.Debug;
using DAS_SDK.MVC.Model.Sorts;
using DAS_SDK.MVC.Model.Sorts.Base_Sort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    class Search_Base<T>  where T : IComparable
    {
        List<T> matches = new List<T>();
        Random random = new Random();
        T searched = default(T);
        Sort_Type sort_Type;
        Base_Debug _Debug;
        private Object sort;

        public Search_Base(string path, Base_Debug debug, Front_END.Front_END front_END, T searched, Sort_Type sort_Type)
        {
            this.sort_Type = sort_Type;
            this._Debug = debug;
            switch (sort_Type)
            {
                case Sort_Type.Bubble:
                    sort = new Bubble_Sort<T>(debug, front_END);
                    break;
                case Sort_Type.Select:
                    sort = new Select_Sort<T>(debug, front_END);
                    break;
                case Sort_Type.Quick:
                    sort = new Quick_Soort<T>(debug, front_END);
                    break;
                case Sort_Type.Radix:
                    sort = new Radix_Sort<T>(debug, front_END);
                    break;
                case Sort_Type.none:
                    throw new Exception("You cannot search in unsorted file!, yet.");
            }
            this.searched = searched;
            if (double.TryParse(GetSort().list[0].ToString(), out double test))
            {
               GetSort()._DoSort = B_Search;
            }
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
            else throw  GetSort().CouldNotBeSorted();
        }

        protected virtual bool Search_Logic(Front_END.Front_END front_END)
        {
            int l = 0, r = GetSort().list.Count - 1;
            while (l <= r)
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
                    for (int i = 0; i < matches.Capacity-1; i++)
                    {
                        if (Convert.ToDouble(matches[i]) != Convert.ToDouble(searched))
                        {
                            matches.RemoveAt(i);
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
                    return (sort as Bubble_Sort<T>);
                case Sort_Type.Quick:
                    return (sort as Bubble_Sort<T>);
                case Sort_Type.Radix:
                    return (sort as Bubble_Sort<T>);
                case Sort_Type.none:
                    return (sort as Bubble_Sort<T>);
            }
            return null;
        }
    }
}
