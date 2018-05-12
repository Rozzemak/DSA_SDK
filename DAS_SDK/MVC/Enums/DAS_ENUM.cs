using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAS_SDK.MVC.Enums
{
    public enum DAS_ENUM
    {
        Sort,
        Custom
    }

    public enum DAS_ENUM_CustomArray
    {
        Front,
        List
    }

    public enum DAS_ENUM_SORT_TYPE
    {
        Select,
        //Insert,
        Bubble,
        // Merge,
        Radix,
        Quick,
        //Bogo        
    }


    enum DAS_FORMAT_ENUM
    {
        BINARY,
        TXT_ROW_1VAL,
        TXT_1ROW_NVALS_COMMA_SEPARTOR,
        TXT_NO_FORMAT
    }

}
