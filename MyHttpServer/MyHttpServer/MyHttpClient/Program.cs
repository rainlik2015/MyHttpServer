using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyHttpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 1000; i++)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(obj =>
                {
                    System.Net.WebClient w = new System.Net.WebClient();
                    var buf = w.DownloadData(@" http://localhost:8880/myHome/myGetUser/my3");
                    //Console.WriteLine(buf.Length.ToString());
                });
            }
            Console.ReadKey();
        }
    }
}
