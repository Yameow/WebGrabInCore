using Pomelo.AspNetCore.TimedJob;
using WebGrabDemo.Common;
using WebGrabDemo.Models;

namespace WebGrabDemo.Jobs
{
    public class AutoGetJobs:Job
    {
        [Invoke(Begin = "2017-09-01 00:30", Interval = 1000 * 3600 * 3, SkipWhileExecuting = true)]
        public void Run()
        {
            LogHelper.Info("Start crawling");
            LatestMovieInfo.CrawlLatestMovieInfo(100);
            HotMovieInfo.CrawlHotMovie();
            Btdytt520HotClickHelper.CrawlHotClickMovieInfo();
            LogHelper.Info("Finish crawling");
        }
    }
}
