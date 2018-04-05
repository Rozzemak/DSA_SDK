using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAS_SDK.MVC.Model.Debug
{
    class Base_Debug
    {
        public Thread DebugThread;
        private List<Message<object>> messages = new List<Message<object>>();
        protected delegate void Worker(MessageType_ENUM messageTypeOnly = MessageType_ENUM.DEFAULT_WRITE_ALL);
        protected Worker _Worker;

        private uint jobs = 0;

        public Base_Debug(bool threadStart = true)
        {
            DebugThread = new Thread(delegate ()
            {

                while (true)
                {
                    Thread.Sleep(10);
                    while (jobs > 0)
                    {
                        Work();
                        jobs--;
                        Thread.Sleep(1);
                    }
                }
            });
            if (threadStart)
                DebugThread.Start();
            //  _Worker += Work_2;

        }

        public void AddMessage<T>(Message<object> msg)
        {
                messages.Add(msg);
                PrintAllPendingMessages();
        }

        public async Task AddMessage_Assync<T>(Message<object> msg)
        {
            await Task.Run(delegate (){
                messages.Add(msg);
                PrintAllPendingMessages();
            });          
        }

        private void PrintAllPendingMsg(MessageType_ENUM messageTypeOnly = MessageType_ENUM.DEFAULT_WRITE_ALL)
        {
            lock (Console.Out)
            {
                if (messageTypeOnly == MessageType_ENUM.DEFAULT_WRITE_ALL)
                {
                    foreach (var item in messages)
                    {

                        // Console.ResetColor();
                        switch (item._MessageType)
                        {
                            case MessageType_ENUM.Standard:
                                break;
                            case MessageType_ENUM.Warning:
                                Console.BackgroundColor = ConsoleColor.Red;
                                break;
                            case MessageType_ENUM.Error:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case MessageType_ENUM.Exception:
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.ForegroundColor = ConsoleColor.Black;
                                break;
                            case MessageType_ENUM.Indifferent:
                                Console.BackgroundColor = ConsoleColor.Yellow;
                                Console.ForegroundColor = ConsoleColor.Black;
                                break;
                            case MessageType_ENUM.Event:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case MessageType_ENUM.DEFAULT_WRITE_ALL:
                                break;
                        }
                        Console.WriteLine("[" + Enum.GetName(typeof(MessageType_ENUM), item._MessageType) + "]" + item.MessageContent);
                        Console.ResetColor();
                    }
                }
                else
                {
                    foreach (var item in messages)
                    {
                        if (item._MessageType == messageTypeOnly)
                            Console.WriteLine("[" + Enum.GetName(typeof(MessageType_ENUM), item._MessageType) + "]" + item.MessageContent);
                    }
                }
                messages.Clear();
            }
        }

        public void PrintAllPendingMessages(MessageType_ENUM messageTypeOnly = MessageType_ENUM.DEFAULT_WRITE_ALL)
        {
            jobs++;
            _Worker += PrintAllPendingMsg;
        }

        private void Work_2(MessageType_ENUM messageType_ENUM = MessageType_ENUM.Standard)
        {
            Console.WriteLine("Test_Work2");
        }

        public void Work()
        {
            _Worker();
        }
    }
}
