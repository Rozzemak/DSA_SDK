using DAS_SDK.MVC.Enums;
using DAS_SDK.MVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DAS_SDK;
using System.Threading;
using DAS_SDK.MVC.Model.Debug;

namespace DAS_SDK.MVC.Model.Front_END
{
    class Base_Front_END
    {
        protected List<Window> WindowRefList;
        protected Grid Grid_Root_ControlRef;
        protected StackPanel StackPanel;
        protected Base_Debug Debug;

        public Thread UI_Thread;

        public Base_Front_END(List<Window> windowRefList, Thread _UI_Thread, Base_Debug debug)
        {
            UI_Thread = _UI_Thread;
            this.Debug = debug;
            this.WindowRefList = windowRefList;
            //progressBar = Grid_Root_ControlRef.FindName("Progress_Bar") as ProgressBar;
            //debug.AddMessage<object>(new Message<object>("Progress bar name: [" + progressBar.Name + "]", MessageType_ENUM.Indifferent));
            //debug.AddMessage<object>(new Message<object>("Grid name: [" + Grid_Root_ControlRef.Name +"]", MessageType_ENUM.Indifferent));
            //debug.AddMessage<object>(new Message<object>("InitController_Button name: [" + SortButton.Name + "]", MessageType_ENUM.Indifferent));
        }






    }
}
