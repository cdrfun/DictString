using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictString
{
    class EntryGenerator
    {
        int maxStringLength = 10;
        Random _rand = new Random((int)DateTime.Now.Ticks);
        List<string> _assignedKeys = new List<string>();
        string _validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        ~EntryGenerator()
        {
            Console.WriteLine("~EntryGenerator");
            Console.WriteLine(Environment.NewLine);
        }

        public string GetKeyValueEnries(int numberOfEntries)
        {
            PerformanceMeasure perf = new PerformanceMeasure(@"measure_StringBuilder.csv", true);

            StringBuilder result = new StringBuilder();
            //string result = String.Empty;;

            for (int i = 0; i < numberOfEntries; i++)
            {
                result.Append(GetKeyValueEnry());
                result.Append(";");

                Console.Write("\r{0}", perf.WriteMeasure());
            }

            Console.Write(Environment.NewLine);
            return result.ToString();
        }

        public string GetKeyValueEnriesSlow(int numberOfEntries)
        {
            PerformanceMeasure perf = new PerformanceMeasure(@"measure_StringAppend.csv", true);

            //StringBuilder result = new StringBuilder();
            string result = String.Empty; ;

            for (int i = 0; i < numberOfEntries; i++)
            {
                result += GetKeyValueEnry() + ";";
                Console.Write("\r{0}", perf.WriteMeasure());
            }

            Console.Write(Environment.NewLine);
            return result.ToString();
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
            int length = _rand.Next(maxLength - 1) + 1;
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
}
