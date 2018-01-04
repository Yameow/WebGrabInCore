using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebGrabDemo.Common;

namespace WebGrabDemo.Service
{
    public class PingService
    {
        public void PingWebsite(int times)
        {
            Task.Factory.StartNew(async () =>
            {
                //Random rId = new Random();
                while (true)
                {
                    //int v = rId.Next(8000000);
                    //var indexUrl = $"http://www.zheyibu.com/job/{v}.html";
                    //Console.WriteLine(indexUrl);
                    //if (!string.IsNullOrEmpty(RequestHelper.HttpGet(indexUrl, Encoding.UTF8)))
                    //{
                    //    Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " : Success : " + indexUrl);
                    //}
                    //RequestHelper.HttpGet(indexUrl);
                    //Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " : Success : " + indexUrl);
                    var indexUrl = "https://www.zhihu.com/question/265079560";
                    SendMessage(indexUrl).ConfigureAwait(false);              
                    Console.WriteLine(times++);
                }                         
            });
        }

        public async Task<HttpResponseMessage> SendMessage(string url)
        {
            using (var client = new HttpClient())
            {
                return await client.GetAsync(url);
            }
        }
    }
}
