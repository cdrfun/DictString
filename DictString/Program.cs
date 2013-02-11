using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Diagnostics;

namespace DictString
{
    class Program
    {
        static void Main(string[] args)
        {
            bool repeat = true;
            int numVal = 0;

            while (repeat)
            {
                repeat = false;
                Console.WriteLine("Number of key-value-pairs needed:");
                string input = Console.ReadLine();
                try
                {
                    numVal = Convert.ToInt32(input);
                }
                catch
                {
                    Console.WriteLine("Input string is not a valid number.");
                    repeat = true; ;
                }
            }

            EntryGenerator generator = new EntryGenerator();
            string result = generator.GetKeyValueEnries(numVal);

            Console.WriteLine(result);
            System.IO.File.WriteAllText(@"out.txt", result);
        }
    }

    class EntryGenerator
    {
        int maxStringLength = 10;
        Random _rand = new Random((int)DateTime.Now.Ticks);
        List<string> _assignedKeys = new List<string>();
        string _validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public string GetKeyValueEnries(int numberOfEntries)
        {
            long time = 0;
            long itemTime = 0;

            //StringBuilder result = new StringBuilder();
            string result = String.Empty;;
            for (int i = 0; i < numberOfEntries - 1; i++)
            {
                //result.Append(GetKeyValueEnry());
                //result.Append(";");
                result += GetKeyValueEnry() + ";";
                time = _stopWatch.ElapsedMilliseconds;
                itemTime = 0;
                if (time > 0)
                    itemTime = (time*10000) / i;
                Console.Write("\r Current: {0} | Time per 10k Pairs: {1} ms | Total Time: {2} ms", i, itemTime, time);
            }
            
            //return result.ToString();
            return result;
        }

        private string GetKeyValueEnry()
        {
            string key = GetUniqueKey(maxStringLength);
            string value = RandLengthString(maxStringLength);
            return key + "=" + value;
        }

        private string GetUniqueKey(int maxLength)
        {
            string key = RandLengthString(maxStringLength);
            while (_assignedKeys.Contains(key))
            {
                key = RandLengthString(maxStringLength);
            }
            return key;
        }

        private string RandLengthString(int maxLength)
        {
            int length = _rand.Next(maxLength-1)+1;
            return RandomString(length);
        }

        private string RandomString(int size)
        {
            var stringChars = new char[size];
            for (int i = 0; i < size; i++)
            {
                stringChars[i] = _validChars[_rand.Next(_validChars.Length)];
            }

            return new String(stringChars);
        }
    }

    class PerformanceMeasure : IDisposable
    {
        private Process _currentProcess;
        private Stopwatch _stopWatch;
        private string _perfLog;
        private int _noMeasure;

        public PerformanceMeasure(string perfLog)
        {
            this._currentProcess = Process.GetCurrentProcess();
            this._stopWatch = new Stopwatch();
            this._noMeasure = 0;

            string header = "NoMeasure;TimePassed;RamUsage;CpuUsage";
            System.IO.File.WriteAllText(@_perfLog, header);
        }

        public void swStart()
        {
            _stopWatch.Start();
        }

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _currentProcess.Dispose();

            _disposed = true;
        }    

        ~PerformanceMeasure()
        {
            Dispose(false);
        }       
    }
}
