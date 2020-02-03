using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAS_SDK.MVC.Enums
{
    public enum DasEnum
    {
        Sort,
        Custom
    }

    public enum DasEnumCustomArray
    {
        Front,
        List
    }

    public enum DasEnumSortType
    {
        Select,
        //Insert,
        Bubble,
        // Merge,
        Radix,
        Quick,
        Heap,
        None
        //Bogo        
    }


    enum DasFormatEnum
    {
        Binary,
        TxtRow1Val,
        Txt1RowNvalsCommaSepartor,
        TxtNoFormat
    }

}
