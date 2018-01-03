using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebGrabDemo.Common;

namespace WebGrabDemo.Service
{
    public class PingService
    {
        public static void GrabIQDailynfo(int indexPageCount = 0)
        {
            Task.Factory.StartNew(() =>
            {
                Random rId = new Random();
                while (true)
                {
                    int v = rId.Next(8000000);
                    var indexUrl = $"http://www.zheyibu.com/job/{v}.html";
                    //Console.WriteLine(indexUrl);
                    if (!string.IsNullOrEmpty(RequestHelper.HttpGet(indexUrl, Encoding.UTF8)))
                    {
                        Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " : Success : " + indexUrl);
                    }
                    //RequestHelper.HttpGet(indexUrl);
                    //Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " : Success : " + indexUrl);
                }                         
            });
        }
    }
}
