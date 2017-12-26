using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using WebGrabDemo.Common;

namespace WebGrabDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()                
                .Build();

            host.Run();
        }

        public static MovieInfoHelper hotMovieList = new MovieInfoHelper(Path.Combine(GlobalConfig.WWWRootPath, "hotMovie.json"));
    }
}
