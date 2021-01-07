using IntegratedModule;
using System;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> arr = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>();
            arr.Add(new System.Collections.Generic.KeyValuePair<string, string>("Hi", "Hello"));
            Console.WriteLine("{Hi} World!".format(arr.ToArray()));
        }
    }
}
