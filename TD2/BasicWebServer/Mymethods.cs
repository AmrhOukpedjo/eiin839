using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicWebServer
{
    internal class Mymethods
    {
        public string MyMethod(string param1, string param2)
        {
            Console.WriteLine("Call MyMethod 1");
            return "<html><body> Hello " + param1 + " et " + param2 + "</body></html>";
        }
    }
}
