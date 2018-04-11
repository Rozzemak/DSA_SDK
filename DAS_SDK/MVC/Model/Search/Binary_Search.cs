using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using DAS_SDK.MVC.Model.Debug;
using DAS_SDK.MVC.Model.Front_END;

namespace DAS_SDK.MVC.Model.Search
{
    class Binary_Search<T> : Base_Search<T> where T : IComparable
    {
        public Binary_Search(string path, Base_Debug debug, Front_END.Front_END front_END, T searched, Sort_Type sort_Type)
            : base(path, debug, front_END, searched, sort_Type)
        {
            if (double.TryParse(GetSort().list[0].ToString(), out double test))
            {
                if (!GetSort().IsSorted())
                {
                    // This is very dirty quick workaround of async wait. Though it´s not async. Wanted to catch an event here, but :P.
                    GetSort().Sort();
                    while (!GetSort().isSorted)
                    {

                    }
                }
                else
                {
                    // maybe something clever here ?
                }
                _DoSearch = B_Search;
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
            else throw CouldNotBeFound();
        }

        protected bool Search_Logic(Front_END.Front_END front_END)
        {
            int min = 0, max = GetSort().list.Count -1;
            while (min <= max)
            {
                int m = (min + max) / 2;
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
                    for (int i = 0; i < matches.Count; i++)
                    {
                        if ((matches[i]).CompareTo(searched) != 0)
                        {
                            matches[i] = default(T);
                        }
                    }
                    return true;
                }
                // If searched greater, ignore left halfy
                if (GetSort().list[m].CompareTo(searched) < 0)
                {
                    min = m + 1;
                }// If searched is smaller, ignore right half
                else if ((GetSort().list[m].CompareTo(searched) > 0))
                {
                    max = m - 1;
                }
            }
            return false;
        }

    }
}
