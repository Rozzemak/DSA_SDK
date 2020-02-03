using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAS_SDK.MVC.Model.Debug
{
    class BaseDebug
    {
        public Thread DebugThread;
        private List<Message<object>> messages = new List<Message<object>>();
        protected delegate void Worker(MessageTypeEnum messageTypeOnly = MessageTypeEnum.DefaultWriteAll);
        protected Worker Worker2;

        private uint jobs = 0;

        public BaseDebug(bool threadStart = true)
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
            //  _Worker2 += Work_2;

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

        private void PrintAllPendingMsg(MessageTypeEnum messageTypeOnly = MessageTypeEnum.DefaultWriteAll)
        {
            lock (Console.Out)
            {
                if (messageTypeOnly == MessageTypeEnum.DefaultWriteAll)
                {
                    for (var i = 0; i < messages.Count; i++)
                    {
                        // Console.ResetColor();
                        switch (messages[i].MessageType)
                        {
                            case MessageTypeEnum.Standard:
                                break;
                            case MessageTypeEnum.Warning:
                                Console.BackgroundColor = ConsoleColor.Red;
                                break;
                            case MessageTypeEnum.Error:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case MessageTypeEnum.Exception:
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.ForegroundColor = ConsoleColor.Black;
                                break;
                            case MessageTypeEnum.Indifferent:
                                Console.BackgroundColor = ConsoleColor.Yellow;
                                Console.ForegroundColor = ConsoleColor.Black;
                                break;
                            case MessageTypeEnum.Event:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case MessageTypeEnum.DefaultWriteAll:
                                break;
                        }
                        Console.WriteLine("[" + Enum.GetName(typeof(MessageTypeEnum), messages[i].MessageType) + "]" + messages[i].MessageContent);
                        Console.ResetColor();
                    }
                }
                else
                {
                    for (var i = 0; i < messages.Count; i++)
                    {
                        if (messages[i].MessageType == messageTypeOnly)
                            Console.WriteLine("[" + Enum.GetName(typeof(MessageTypeEnum), messages[i].MessageType) + "]" + messages[i].MessageContent);
                    }
                }
                messages.Clear();
            }
        }

        public void PrintAllPendingMessages(MessageTypeEnum messageTypeOnly = MessageTypeEnum.DefaultWriteAll)
        {
            jobs++;
            Worker2 += PrintAllPendingMsg;
        }

        private void Work_2(MessageTypeEnum messageTypeEnum = MessageTypeEnum.Standard)
        {
            Console.WriteLine("Test_Work2");
        }

        public void Work()
        {
            Worker2();
        }
    }
}
