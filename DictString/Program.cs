using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace DictString
{
    class Program
    {
        [STAThread]
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

            StartStringBuilder(numVal);

            GC.Collect();
            GC.WaitForPendingFinalizers();  // Tried to do some RAM measures here, but didn't work out. Will leave it as a gravestone

            StartStringAppend(numVal);
        }

        static private void StartStringBuilder(int numVal)
        {
            Console.WriteLine("StringBuilder:");
            EntryGenerator generator = new EntryGenerator();

            System.IO.File.WriteAllText(@"out_StringBuilder.txt", generator.GetKeyValueEnries(numVal));
        }

        static private void StartStringAppend(int numVal)
        {
            Console.WriteLine("StringAppend:");
            EntryGenerator generator = new EntryGenerator();

            System.IO.File.WriteAllText(@"out_StringAppend.txt", generator.GetKeyValueEnriesSlow(numVal));
        }
    }
}
