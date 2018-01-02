using System.IO;
using WebGrabDemo.Common;
using WebGrabDemo.Models;
using WebGrabDemo.Service;

namespace WebGrabDemo.Jobs
{
    public class AutoGetJobs
    {
        public static void Run()
        {
            LogHelper.Info("Start crawling");
            //MovieService.GrabHotMovie();
            PositionService.GrabPositionInfo();
            LogHelper.Info("Finish crawling");
        }
    }
}
