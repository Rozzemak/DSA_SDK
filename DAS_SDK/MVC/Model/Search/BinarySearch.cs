using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using DAS_SDK.MVC.Model.Debug;
using DAS_SDK.MVC.Model.FrontEND;
using DAS_SDK.MVC.Model.Search.BaseSearch;

namespace DAS_SDK.MVC.Model.Search
{
    class BinarySearch<T> : BaseSearch<T> where T : IComparable
    {
        public BinarySearch(string path, BaseDebug debug, SortFrontEnd frontEnd, T searched, SortType sortType)
            : base(path, debug, frontEnd, searched, sortType)
        {
            if (double.TryParse(GetSort().List[0].ToString(), out var test))
            {
                if (!GetSort().IsSorted())
                {
                    // This is very dirty quick workaround of async wait. Though it´s not async. Wanted to catch an event here, but :P.
                    GetSort().Sort();
                    while (!GetSort().IsSrted)
                    {

                    }
                }
                else
                {
                    // maybe something clever here ?
                }
                DoSearchBase = B_Search;
            }
        }

        private List<T> B_Search(List<T> list, SortFrontEnd frontEnd)
        {
            var result = false;
            if (GetSort().IsSorted())
            {
                Dispatcher.FromThread(frontEnd.UiThread).Invoke(() =>
                {
                    frontEnd.ProgressBar.Value = 0;
                    frontEnd.ProgressBar.Maximum = list.Count;
                    // (pozn. => for progress bar precision
                    frontEnd.ProgressValIncrement = list.Count / 10000;
                });
                // Try to sort here, if not successfull, then throw ex.
                result = Search_Logic(frontEnd);
                Dispatcher.FromThread(frontEnd.UiThread).Invoke(() =>
                {
                    // Just for the user, to look nice.
                    frontEnd.ProgressBar.Value = frontEnd.ProgressBar.Maximum;
                });
            }
            if (result)
            {
                var msg = new Message<object>("");
                foreach (var item in Matches)
                {
                    msg.MessageContent += "[" + item + "]";
                }
                Debug.AddMessage<object>(msg);
                return Matches;
            }
            else throw CouldNotBeFound();
        }

        private bool Search_Logic(BaseFrontEnd frontEnd)
        {
            int min = 0, max = GetSort().List.Count -1;
            while (min <= max)
            {
                var m = (min + max) / 2;
                // Check if searched is present at mid
                if (GetSort().List[m].CompareTo(Searched) == 0)
                {
                    Matches.Add(GetSort().List[m]);
                    var index = m;
                    while (GetSort().List[Math.Abs(index--)].CompareTo(Searched) == 0)
                    {
                        Matches.Add(GetSort().List[index]);
                    }
                    var index1 = m;
                    while (GetSort().List[index1++].CompareTo(Searched) == 0)
                    {
                        Matches.Add(GetSort().List[index1]);
                    }
                    for (var i = 0; i < Matches.Count; i++)
                    {
                        if ((Matches[i]).CompareTo(Searched) != 0)
                        {
                            Matches[i] = default(T);
                        }
                    }
                    return true;
                }
                // If searched greater, ignore left halfy
                if (GetSort().List[m].CompareTo(Searched) < 0)
                {
                    min = m + 1;
                }// If searched is smaller, ignore right half
                else if ((GetSort().List[m].CompareTo(Searched) > 0))
                {
                    max = m - 1;
                }
            }
            return false;
        }

    }
}
