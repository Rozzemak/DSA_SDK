using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using DAS_SDK.MVC.Enums;

namespace DAS_SDK.MVC.Model.FileGenerator
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

    class FileGeneratorBase<TYpe>
    {
        protected ulong MaxSizeInBytes;
        public string Path;
        private Random rnd;
        private DasFormatEnum formatEnum;
        private FileInfo fileInfo;
        protected uint MaxValSize;
        protected uint MaxValLenght;
        protected int Seed;

        public FileGeneratorBase(ulong maxSizeInBytes = 100 * 100 * 100 * 10,
            string path = "unsorted.txt",
            int seed = 0, DasFormatEnum format = DasFormatEnum.TxtRow1Val,
            uint maxValSize = Int32.MaxValue, uint maxValLenght = 10)
        {
            this.Path = path;
            this.Seed = seed;
            this.MaxSizeInBytes = maxSizeInBytes;
            this.formatEnum = format;
            this.MaxValSize = maxValSize;
            this.MaxValLenght = maxValLenght;
            fileInfo = new FileInfo(this.Path);
            if (seed == 0)
                rnd = new Random();
            else
                rnd = new Random(seed);
        }

        public bool FileExists()
        {
            if (File.Exists(Path))
            {
                return true;
            }
            else
            {
                MessageBox.Show("File: \n [" + Directory.GetCurrentDirectory() + @"\" + Path + "] \n does not exists. \n And will be created.");
                return false;
            }
        }

        public bool CreateAndFill<T>()
        {
            var fileRefreshFreqInWrites = 100;
            var incrementRefresh = 0;
            var filled = new List<object>();
            switch (formatEnum)
            {
                case DasFormatEnum.Binary:

                    break;
                case DasFormatEnum.TxtRow1Val:
                    var valToWrite = default(T);
                    uint rows = 0;
                    //object valToWrite = ((object)(Guid.NewGuid().ToString().Substring(0, Math.Abs(rnd.Next(1,(int)maxValSize))))).ToString();
                    using (var sr = new StreamWriter(Path, FileExists(), Encoding.UTF8))
                    {
                        while ((ulong)fileInfo.Length < (MaxSizeInBytes))
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
                case DasFormatEnum.Txt1RowNvalsCommaSepartor:
                    var valToWrite2 = default(T);
                    //object valToWrite = ((object)(Guid.NewGuid().ToString().Substring(0, Math.Abs(rnd.Next(1,(int)maxValSize))))).ToString();
                    using (var sr = new StreamWriter(Path, FileExists(), Encoding.UTF8))
                    {
                        while ((ulong)fileInfo.Length < (MaxSizeInBytes))
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
                case DasFormatEnum.TxtNoFormat:
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
                val = (Convert.ChangeType((Guid.NewGuid().ToString().Substring(0, Math.Abs(rnd.Next(1, (int)MaxValLenght)))), typeof(T)));
                return (T)val;
            }
        }
    }
}
