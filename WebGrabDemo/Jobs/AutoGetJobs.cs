using Pomelo.AspNetCore.TimedJob;
using WebGrabDemo.Common;
using WebGrabDemo.Models;

namespace WebGrabDemo.Jobs
{
    public class AutoGetJobs:Job
    {
        [Invoke(Begin = "2017-09-06 23:09", Interval = 1000 *60 *60, SkipWhileExecuting = true)]
        public void Run()
        {
            LogHelper.Info("Start crawling");
            LatestMovieInfo.CrawlLatestMovieInfo(10);
            //HotMovieInfo.CrawlHotMovie();
            //Btdytt520HotClickHelper.CrawlHotClickMovieInfo();
            LogHelper.Info("Finish crawling");
        }
    }
}
