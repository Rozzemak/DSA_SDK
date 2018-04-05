using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAS_SDK.MVC.Enums;
using System.IO;
using System.Threading;
using System.Windows;

namespace DAS_SDK.MVC.Model.File_Generator
{
    static class Ext
    {
        public static bool IsNumericType(this object o)
        {
            switch (System.Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }


        public static T MinValue<T>(this Type self)
        {
            return (T)self.GetField(nameof(MinValue)).GetRawConstantValue();
        }

        public static T MaxValue<T>(this Type self)
        {
            return (T)self.GetField(nameof(MaxValue)).GetRawConstantValue();
        }


    }

    class File_Generator_Base<Type>
    {
        protected ulong maxSizeInBytes;
        protected string path;
        private Random rnd;
        private DAS_FORMAT_ENUM format_enum;
        private FileInfo fileInfo;
        protected uint maxValSize;
        protected uint maxValLenght;
        protected int seed;

        public File_Generator_Base(ulong maxSizeInBytes = 100 * 100 * 100 * 10,
            string path = "unsorted.txt",
            int seed = 0, DAS_FORMAT_ENUM format = DAS_FORMAT_ENUM.TXT_ROW_1VAL,
            uint maxValSize = Int32.MaxValue, uint maxValLenght = 10)
        {
            this.path = path;
            this.seed = seed;
            this.maxSizeInBytes = maxSizeInBytes;
            this.format_enum = format;
            this.maxValSize = maxValSize;
            this.maxValLenght = maxValLenght;
            fileInfo = new FileInfo(this.path);
            if (seed == 0)
                rnd = new Random();
            else
                rnd = new Random(seed);
        }

        public bool FileExists()
        {
            if (File.Exists(path))
            {
                return true;
            }
            else
            {
                MessageBox.Show("File: \n [" + Directory.GetCurrentDirectory() + @"\" + path + "] \n does not exists. \n And will be created.");
                return false;
            }
        }

        public bool CreateAndFill<T>()
        {
            int fileRefreshFreqInWrites = 100;
            int incrementRefresh = 0;
            List<object> filled = new List<object>();
            switch (format_enum)
            {
                case DAS_FORMAT_ENUM.BINARY:

                    break;
                case DAS_FORMAT_ENUM.TXT_ROW_1VAL:
                    T valToWrite = default(T);
                    uint rows = 0;
                    //object valToWrite = ((object)(Guid.NewGuid().ToString().Substring(0, Math.Abs(rnd.Next(1,(int)maxValSize))))).ToString();
                    using (StreamWriter sr = new StreamWriter(path, FileExists(), Encoding.UTF8))
                    {
                        while ((ulong)fileInfo.Length < (maxSizeInBytes))
                        {
                            incrementRefresh++;
                            valToWrite = GetValToWrite<T>();
                            sr.WriteLine((valToWrite).ToString());
                            if (incrementRefresh >= fileRefreshFreqInWrites)
                            {
                                incrementRefresh = 0;
                                fileInfo.Refresh();
                            }
                            rows++;
                            // Adding every written value to list, will result in out of mem. Ex., stack refresh will not happen.
                        }
                        System.Console.WriteLine("Lines in file: [" + rows + "]");
                    }
                    break;
                case DAS_FORMAT_ENUM.TXT_1ROW_NVALS_COMMA_SEPARTOR:
                    T valToWrite2 = default(T);
                    //object valToWrite = ((object)(Guid.NewGuid().ToString().Substring(0, Math.Abs(rnd.Next(1,(int)maxValSize))))).ToString();
                    using (StreamWriter sr = new StreamWriter(path, FileExists(), Encoding.UTF8))
                    {
                        while ((ulong)fileInfo.Length < (maxSizeInBytes))
                        {
                            incrementRefresh++;
                            valToWrite2 = GetValToWrite<T>();
                            sr.Write((valToWrite2).ToString()+";");
                            if (incrementRefresh >= fileRefreshFreqInWrites)
                            {
                                incrementRefresh = 0;
                                fileInfo.Refresh();
                            }
                            // Adding every written value to list, will result in out of mem. Ex., stack refresh will not happen.
                            // filled.Add(valToWrite);
                        }
                        System.Console.WriteLine("Lines in file: [" + 1 + "]");
                    }
                    break;
                case DAS_FORMAT_ENUM.TXT_NO_FORMAT:
                    break;
            }
            return true;
        }

        private T GetValToWrite<T>()
        {
            object val = rnd.Next(int.MinValue, int.MaxValue)*rnd.NextDouble();
            if (default(T) != null && default(T).IsNumericType())
            {
                return (T)Convert.ChangeType(val, typeof(T));
            }
            else
            {
                val = (Convert.ChangeType((Guid.NewGuid().ToString().Substring(0, Math.Abs(rnd.Next(1, (int)maxValLenght)))), typeof(T)));
                return (T)val;
            }
        }
    }
}
