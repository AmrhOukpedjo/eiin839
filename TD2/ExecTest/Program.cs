using System;

namespace ExeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(args.Length);
            string result = "<html><body> Hello ";
            foreach (var arg in args)
            {
              result+= arg +" et ";
            }

            result += "</body></html>";
            
            /* if (args.Length == 1)
                 Console.WriteLine(args[0]);
             else
                 Console.WriteLine("ExeTest <string parameter>");*/
        }
    }
}
