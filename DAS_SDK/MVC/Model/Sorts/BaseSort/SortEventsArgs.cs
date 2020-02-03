using System;
using DAS_SDK.MVC.Enums;

namespace DAS_SDK.MVC.Model.Sorts.BaseSort
{
    class FileSortedEventArgs<T>: EventArgs
    {
        public DateTime StartDateTime = new DateTime();
        public DateTime CreatedDateTime = new DateTime();

        public FileSortedEventArgs(DateTime startDateTime, BaseSort<T> baseSort)
        {
            this.StartDateTime = startDateTime;
            this.CreatedDateTime = DateTime.Now;
            baseSort.SortState = SortState.Sorted;
        }
        public TimeSpan GetDeltaTime()
        {
            return CreatedDateTime.Subtract(StartDateTime);
        }
    }

    class SortingStartedEventArgs<T> : EventArgs
    {
        public DateTime StartDateTime = new DateTime();

        public SortingStartedEventArgs(BaseSort<T> baseSort)
        {
            StartDateTime = DateTime.Now;
            baseSort.SortState = SortState.Sorting;

        }
    }

}
