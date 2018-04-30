using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAS_SDK.MVC.Model.Debug;

namespace DAS_SDK.MVC.Model.Sorts.Base_Sort
{
    class FileSortedEventArgs<T>: EventArgs
    {
        public DateTime startDateTime = new DateTime();
        public DateTime createdDateTime = new DateTime();

        public FileSortedEventArgs(DateTime startDateTime, Base_Sort<T> base_Sort)
        {
            this.startDateTime = startDateTime;
            this.createdDateTime = DateTime.Now;
            base_Sort.sortState = SortState.Sorted;
        }
        public TimeSpan GetDeltaTime()
        {
            return createdDateTime.Subtract(startDateTime);
        }
    }

    class SortingStartedEventArgs<T> : EventArgs
    {
        public DateTime startDateTime = new DateTime();

        public SortingStartedEventArgs(Base_Sort<T> base_Sort)
        {
            startDateTime = DateTime.Now;
            base_Sort.sortState = SortState.Sorting;

        }
    }

}
