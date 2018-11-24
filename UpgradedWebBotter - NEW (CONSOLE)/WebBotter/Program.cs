using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;
using System.Threading;

namespace WebBotter
{
    class Program
    {
        public static string[] proxyListGlobal; // proxy list global array
        public static string URL; // URL that will get viewbotted
        public static string referer = "http://www.reddit.com/"; // The referer header that will be sent. By default it is reddit.
        static void Main(string[] args)
        {
           Console.ForegroundColor = ConsoleColor.Green; // Hacker colours, a must have in this world
           Console.WriteLine("Enter URL you wish to bot.");
           try
           {
               URL = Console.ReadLine(); // gets URL which the user wants to bot.
               Uri URLURI = new Uri(URL); // makes a URI compatible for below.
           }
            catch(Exception e)
           {
               Console.Clear();
               Console.WriteLine(e.Message);
               Main(null);
               return;
           }

           Console.WriteLine("Enter referer URL. Leave blank for reddit.com");
           string refer = Console.ReadLine(); // gets user referer
            if(!String.IsNullOrEmpty(refer))
            {
                referer = refer; // sets user referer
            }

           Console.WriteLine("Enter location of proxy list. Leave Null for 10,000 hits with YOUR ip.");
           string proxyList = Console.ReadLine(); // gets location of proxylist
            if(String.IsNullOrEmpty(proxyList))
            {
                startWithoutProxy(); // if no proxy list, start without proxy. "Leave Null for 10,000 hits with YOUR ip"
            }
            else if(!File.Exists(proxyList))
            {
                Console.Clear();
                Console.WriteLine("File not found.");
                Main(null);
            }
            else
            {
                proxyListGlobal = File.ReadAllLines(@proxyList); // reads all proxies
                multi.start1000PerIP(proxyListGlobal,URL,refer); // refer to multi class
                //startWithProxy(); currently no longer in use since it's outdated and slow.
            }
        }
        public static void startWithProxy() // not commenting this because it's outdated
        {
            int counter = 0;
            foreach (var item in proxyListGlobal)
            {
                try
                {
                    counter++;
                    Console.WriteLine(item + " ---- " + counter);
                    WebProxy WP = new WebProxy(item);
                    HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(URL);
                    myRequest.Referer = referer;
                    myRequest.Proxy = WP;
                    myRequest.Timeout = 10000;
                    HttpWebResponse WR = (HttpWebResponse)myRequest.GetResponse();
                    if (new StreamReader(WR.GetResponseStream()).ReadToEnd().Contains('<'))
                    {
                        Console.WriteLine("Successfully sent packet!");
                    }
                    WR.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        public static void startWithoutProxy()
        {
            for(int counter = 0; counter < 10000; counter++ ) // runs a foreach for 10000 hits
                try
                {
                    Console.WriteLine(counter); // writes the current hit number
                    HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(URL); // creates the web request
                    myRequest.Referer = referer; // sets referer
                    HttpWebResponse WR = (HttpWebResponse)myRequest.GetResponse(); // gets the response
                    if (new StreamReader(WR.GetResponseStream()).ReadToEnd().Contains('<')) // checks if response is a website (because of tags and stuff :)
                    {
                        Console.WriteLine("Successfully sent packet!"); // tells them the packet was successfully sent
                    }
                    WR.Close(); // closes the response
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
        }
        }

    }
