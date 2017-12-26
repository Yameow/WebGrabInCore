using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;
using WebGrabDemo.Common;
using System.IO;
using NLog.Web;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Http;
using WebGrabDemo.Jobs;

namespace WebGrabDemo
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddHangfire(x => x.UseStorage(new MemoryStorage()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            env.ConfigureNLog(Path.Combine(env.WebRootPath, "nlog.config"));

            app.UseHangfireServer();
            app.UseHangfireDashboard();
            RecurringJob.AddOrUpdate(() => AutoGetJobs.Run(), Cron.Minutely());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.Map("/index", r =>
            //{
            //    r.Run(context =>
            //    {
            //        //任务每分钟执行一次
            //        RecurringJob.AddOrUpdate(() => Console.WriteLine($"ASP.NET Core LineZero"), Cron.Minutely());
            //        return context.Response.WriteAsync("ok");
            //    });
            //});
            //app.Map("/one", r =>
            //{
            //    r.Run(context =>
            //    {
            //        //任务执行一次
            //        BackgroundJob.Enqueue(() => Console.WriteLine($"ASP.NET Core One Start LineZero{DateTime.Now}"));
            //        return context.Response.WriteAsync("ok");
            //    });
            //});
            //app.Map("/await", r =>
            //{
            //    r.Run(context =>
            //    {
            //        //任务延时两分钟执行
            //        BackgroundJob.Schedule(() => Console.WriteLine($"ASP.NET Core await LineZero{DateTime.Now}"), TimeSpan.FromMinutes(2));
            //        return context.Response.WriteAsync("ok");
            //    });
            //});
            
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            GlobalConfig.WWWRootPath = env.WebRootPath;

            //RecurringJob.AddOrUpdate(() => Console.WriteLine("test!  " + DateTime.Now), Cron.Minutely());

        }


        //public static async Task TestAsync()
        //{
        //    //Todo
        //}
    }
}
