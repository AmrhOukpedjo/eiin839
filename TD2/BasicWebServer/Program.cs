using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Reflection;
using System.Diagnostics;

namespace BasicServerHTTPlistener
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            //if HttpListener is not supported by the Framework
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("A more recent Windows version is required to use the HttpListener class.");
                return;
            }
 
 
            // Create a listener.
            HttpListener listener = new HttpListener();

            // Add the prefixes.
            if (args.Length != 0)
            {
                foreach (string s in args)
                {
                    listener.Prefixes.Add(s);
                    // don't forget to authorize access to the TCP/IP addresses localhost:xxxx and localhost:yyyy 
                    // with netsh http add urlacl url=http://localhost:xxxx/ user="Tout le monde"
                    // and netsh http add urlacl url=http://localhost:yyyy/ user="Tout le monde"
                    // user="Tout le monde" is language dependent, use user=Everyone in english 

                }
            }
            else
            {
                Console.WriteLine("Syntax error: the call must contain at least one web server url as argument");
            }
            listener.Start();

            // get args 
            foreach (string s in args)
            {
                Console.WriteLine("Listening for connections on " + s);
            }

            // Trap Ctrl-C on console to exit 
            Console.CancelKeyPress += delegate {
                // call methods to close socket and exit
                listener.Stop();
                listener.Close();
                Environment.Exit(0);
            };


            while (true)
            {
                // Note: The GetContext method blocks while waiting for a request.
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;

                string documentContents;
                using (Stream receiveStream = request.InputStream)
                {
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                    {
                        documentContents = readStream.ReadToEnd();
                    }
                }
                
                // get url 
                Console.WriteLine($"Received request for {request.Url}");


                Console.WriteLine(request.Url.Query);

                /***My methods***/

                Type type = typeof(Mymethods);
                string methodeName = request.Url.Segments[2];
                MethodInfo method = type.GetMethod(methodeName);
                Mymethods c = new Mymethods();
                Console.WriteLine(methodeName);
                string param1 = HttpUtility.ParseQueryString(request.Url.Query).Get("param1");
                string param2 = HttpUtility.ParseQueryString(request.Url.Query).Get("param2");
                Object[] paramts = {};
                if (methodeName.Equals("incr"))
                {
                    int val = int.Parse(param1);
                    Object[] paramaters = {val};
                    paramts = paramaters;
                }
                else
                {
                    Object[] paramaters = { param1, param2 };
                    paramts = paramaters;
                }
 
                string result = (string)method.Invoke(c, paramts);
                Console.WriteLine(result);
                Console.ReadLine();

                Console.WriteLine(result);

                /***END**/

               

                // Obtain a response object.
                //HttpListenerResponse response = context.Response;

                //// Construct a response.
                //string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
                //byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                //// Get a response stream and write the response to it.
                //response.ContentLength64 = buffer.Length;
                //System.IO.Stream output = response.OutputStream;
                //output.Write(buffer, 0, buffer.Length);
                //// You must close the output stream.
                //output.Close();
            }
            // Httplistener neither stop ... But Ctrl-C do that ...
            // listener.Stop();
        }
    }
}

class Mymethods
{
    public static string MyMethod(string param1, string param2)
    {
        Console.WriteLine("Call MyMethod 1");
        return "<html><body> Hello " + param1 + " et " + param2 + "</body></html>";
    }

    public static string MyMethod2(string param1, string param2)
    {
        Console.WriteLine("Call MyMethod 2");
        return "<html><body> Hello " + param1 + " et " + param2 + "=> MyMethod2</body></html>";
    }

    public static string MyMethod3(string param1, string param2)
    {
        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = @"C:\S8\SOC\eiin839\TD2\ExecTest\bin\Debug\ExecTest.exe"; // Specify exe name.
        start.Arguments = param1;// Specify arguments.
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;

        using (Process process = Process.Start(start))
        {
            //
            // Read in all the text from the process with the StreamReader.
            //
            using (StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                Console.WriteLine(result);
                Console.ReadLine();
            }
        }
        return "";
    }

   /** http://localhost:8080/ceque/incr?param1=3 **/
    public static string incr(int param1_val)
    {
        string result = "{\n \t param : " + param1_val + ",\n\t param_incr : " + (param1_val + 1) +"\n}";

        return result;
    }
}