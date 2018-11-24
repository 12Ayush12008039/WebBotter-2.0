using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;
using System.Threading;

namespace Botter.Core
{
    class multi
    {
        public static int running = 0; // int for running proxies
        public static int blocked = 0; // int for blocked proxies (used for my purposes)
        public static int dead = 0; // int for dead proxies (no response, 404 ect)
        public static int total = 0; // total hits sent
        public static int totalProxies = 0; // total proxies used
        public static Timer t; // object for timer :)
        public static string URLGlobal; // global URL

        public static void start1000PerIP(string[] proxyListGlobal, string URL, string referer)
        {
           t = new Timer(TimerCallback, null, 0, 2000); // assigns timer object
           URLGlobal = URL; // sets global URL string to the URL
            totalProxies = proxyListGlobal.Length; // sets the totalProxies int to total proxies
            foreach (var item in proxyListGlobal)
            {
                new Thread(() => runInThread(item, URL, referer)).Start(); // runs viewbot on a selected ip in a new thread, makes things quicker
                running++; // increments running by 1.
            }
        }

        public static void runInThread(string item, string URL, string referer)
        {
            for (int counter = 0; counter < 1000; counter++)
                try
                {

                    //Console.WriteLine(counter);
                    HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(URL); // creates request
                    myRequest.Timeout = 10000; // sets timeout for 10 seconds
                    myRequest.Referer = referer; // sets referer
                    myRequest.Proxy = new WebProxy(item); // sets web proxy
                    HttpWebResponse WR = (HttpWebResponse)myRequest.GetResponse(); // gets response
                    if (new StreamReader(WR.GetResponseStream()).ReadToEnd().Contains("<")) // checks response
                    {
                        total++; // increases total hits by 1.
                        /*blocked++;
                        running--;
                        break;
                        //Console.WriteLine("Successfully sent packet! - " + counter); */
                    }
                    WR.Close(); // closes response
                    myRequest.Abort(); // aborts request now its finished
                }
                catch (Exception e)
                {
                    dead++;
                    running--;
                    break;
                }
        }

        private static void TimerCallback(Object o) // used to update screen infomation
        {
            Console.Clear();
            Console.WriteLine("Website currently botting: " + URLGlobal);
            Console.WriteLine("Total proxies: " + totalProxies);
            Console.WriteLine("Proxies running: " + running);
            Console.WriteLine("Blocked proxies: " + blocked);
            Console.WriteLine("Dead proxies: " + dead);
            Console.WriteLine("Total packets sent: " + total);
        }
    }
}
