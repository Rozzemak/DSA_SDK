using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAS_SDK.MVC.Model.Debug;

namespace DAS_SDK.MVC.Model.Sorts.Base_Sort
{
    class FileSortedEventArgs: EventArgs
    {
        public DateTime startDateTime = new DateTime();
        public DateTime createdDateTime = new DateTime();

        public FileSortedEventArgs(DateTime startDateTime)
        {
            this.startDateTime = startDateTime;
            this.createdDateTime = DateTime.Now;
        }

        public TimeSpan GetDeltaTime()
        {
            return createdDateTime.Subtract(startDateTime);
        }

    }

    class SortingStartedEventArgs : EventArgs
    {
        public DateTime startDateTime = new DateTime();

        public SortingStartedEventArgs()
        {
            startDateTime = DateTime.Now;
        }
    }

}
