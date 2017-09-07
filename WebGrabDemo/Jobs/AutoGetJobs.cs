using WebGrabDemo.Common;
using WebGrabDemo.Models;

namespace WebGrabDemo.Jobs
{
    public class AutoGetJobs
    {
        public static void Run()
        {
            LogHelper.Info("Start crawling");
            LatestMovieInfo.CrawlLatestMovieInfo(10);
            HotMovieInfo.CrawlHotMovie();
            Btdytt520HotClickHelper.CrawlHotClickMovieInfo();
            LogHelper.Info("Finish crawling");
        }
    }
}
