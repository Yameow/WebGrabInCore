using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using WebGrabDemo.Common;
using System.Text;

namespace WebGrabDemo.Models
{
    public class Btdytt520HotClickHelper
    {

        private static MovieInfoHelper hotMoviceHelper = new MovieInfoHelper(Path.Combine(ConstsConf.WWWRootPath, "btdytt520HotClick.json"));

        private static HtmlParser htmlParser = new HtmlParser();

        public static void CrawlHotClickMovieInfo(int endIndex=10)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    LogHelper.Info("CrawlHotClickMovieInfo Start...");
                    for (int index = 1; index <= endIndex; index++)
                    {
                        var indexURL = $"http://www.btdytt520.com/hotclick/p_{index}.html";
                        var html = RequestHelper.HttpGet(indexURL, Encoding.UTF8);
                        if (string.IsNullOrEmpty(html))
                            return;
                        var htmlDom = htmlParser.Parse(html);
                        foreach (var li in htmlDom.QuerySelectorAll("li.newsli"))
                        {
                            var aDom = li.QuerySelectorAll("a").FirstOrDefault(a => !string.IsNullOrEmpty(a.GetAttribute("target")));
                            if (aDom == null)
                                continue;
                            var onlineURL = "http://www.btdytt520.com/" + aDom.GetAttribute("href");
                            if (hotMoviceHelper.IsContainsMoive(onlineURL) || li.QuerySelector("li.phlidate") == null)
                                continue;
                            var pubDate = DateTime.Now;
                            DateTime.TryParse(li.QuerySelector("li.phlidate").InnerHtml, out pubDate);
                            hotMoviceHelper.AddToMovieDic(new MovieInfo()
                            {
                                Dy2018OnlineUrl = onlineURL,
                                MovieName = aDom.InnerHtml,
                                PubDate = pubDate,
                            });
                        }

                    }

                    LogHelper.Info("CrawlHotClickMovieInfo Finish.");
                }
                catch (Exception ex)
                {
                    LogHelper.Error("CrawlHotClickMovieInfo Exception", ex);
                    LogHelper.Info("CrawlHotClickMovieInfo Finish.");
                }
            });

                

        }

        /// <summary>
        /// 获取全部的电影数据
        /// </summary>
        /// <returns></returns>
        public static List<MovieInfo> GetAllMovieInfo()
        {
            return hotMoviceHelper.GetListMoveInfo();
        }
        
    }
}
