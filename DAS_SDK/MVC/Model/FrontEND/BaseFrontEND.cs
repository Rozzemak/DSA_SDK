using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using DAS_SDK.MVC.Model.Debug;

namespace DAS_SDK.MVC.Model.FrontEND
{
    class BaseFrontEnd
    {
        public List<Window> WindowRefList;
        protected Grid GridRootControlRef;
        public StackPanel StackPanel;
        protected BaseDebug Debug;

        public Thread UiThread;

        public BaseFrontEnd(List<Window> windowRefList, Thread uiThread, BaseDebug debug)
        {
            UiThread = uiThread;
            this.Debug = debug;
            this.WindowRefList = windowRefList;
            //progressBar = Grid_Root_ControlRef.FindName("Progress_Bar") as ProgressBar;
            //debug.AddMessage<object>(new Message<object>("Progress bar name: [" + progressBar.Name + "]", MessageType_ENUM.Indifferent));
            //debug.AddMessage<object>(new Message<object>("Grid name: [" + Grid_Root_ControlRef.Name +"]", MessageType_ENUM.Indifferent));
            //debug.AddMessage<object>(new Message<object>("InitController_Button name: [" + SortButton.Name + "]", MessageType_ENUM.Indifferent));
        }






    }
}
