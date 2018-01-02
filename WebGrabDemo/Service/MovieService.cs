using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using WebGrabDemo.Common;
using System.Text;
using WebGrabDemo.Helper;
using WebGrabDemo.Models;

namespace WebGrabDemo.Service
{
    /// <summary>
    /// 热门电影数据
    /// </summary>
    public class MovieService
    {
        private static readonly HtmlParser HtmlParser = new HtmlParser();

        public static MovieInfoHelper HotMovieHelper = new MovieInfoHelper(Path.Combine(GlobalConfig.WWWRootPath, "hotMovie.json"));

        /// <summary>
        /// 爬取数据
        /// </summary>
        public static void GrabHotMovie()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    LogHelper.Info("CrawlHotMovie Start...");
                    var htmlDoc = RequestHelper.HttpGet("http://www.dy2018.com/", Encoding.GetEncoding("GB2312"));
                    HtmlParser parser = new HtmlParser();
                    var dom = parser.Parse(htmlDoc);
                    var lstDivInfo = dom.QuerySelectorAll("div.co_content222");
                    if (lstDivInfo != null)
                    {
                        //前三个DIV为新电影
                        foreach (var divInfo in lstDivInfo.Take(3))
                        {
                            divInfo.QuerySelectorAll("a").Where(a => a.GetAttribute("href").Contains("/i/")).ToList()
                                .ForEach(
                                    a =>
                                    {
                                        var onlineUrl = "http://www.dy2018.com" + a.GetAttribute("href");
                                        if (!HotMovieHelper.IsContainsMoive(onlineUrl))
                                        {
                                            MovieInfo movieInfo = GetMovieInfoFromOnlineUrl(onlineUrl);
                                            if (movieInfo != null && movieInfo.XunLeiDownLoadUrlList != null &&
                                                movieInfo.XunLeiDownLoadUrlList.Count != 0)
                                                HotMovieHelper.AddToMovieDic(movieInfo);
                                        }
                                    });
                        }
                    }

                    LogHelper.Info("CrawlHotMovie Finish...");
                }
                catch (Exception ex)
                {
                    LogHelper.Error("CrawlHotMovie Exception", ex);
                }
                finally
                {

                }
            });
        }

        /// <summary>
        /// 获取全部的电影数据
        /// </summary>
        /// <returns></returns>
        public static List<MovieInfo> GetAllMovieInfo()
        {
            return HotMovieHelper.GetListMoveInfo();
        }

        public static MovieInfo GetMovieInfoFromOnlineUrl(string onlineUrl, bool isContainIntro = false)
        {
            try
            {
                var movieHtml = RequestHelper.HttpGet(onlineUrl, Encoding.GetEncoding("GB2312"));
                if (string.IsNullOrEmpty(movieHtml))
                    return null;
                var movieDoc = HtmlParser.Parse(movieHtml);
                var zoom = movieDoc.GetElementById("Zoom");
                var lstDownLoadUrl = movieDoc.QuerySelectorAll("[bgcolor='#fdfddf']");
                var updatetime = movieDoc.QuerySelector("span.updatetime"); var pubDate = DateTime.Now;
                if (updatetime != null && !string.IsNullOrEmpty(updatetime.InnerHtml))
                {
                    DateTime.TryParse(updatetime.InnerHtml.Replace("发布时间：", ""), out pubDate);
                }
                var lstOnlineUrl = lstDownLoadUrl.Select(a => a.QuerySelector("a")).Where(item => item != null).Select(item => item.InnerHtml).ToList();

                var movieName = movieDoc.QuerySelector("div.title_all");

                var movieInfo = new WebGrabDemo.Models.MovieInfo()
                {
                    MovieName = movieName != null && movieName.QuerySelector("h1") != null ?
                        movieName.QuerySelector("h1").InnerHtml : "找不到影片信息...",
                    Dy2018OnlineUrl = onlineUrl,
                    MovieIntro = zoom != null && isContainIntro ? WebUtility.HtmlEncode(zoom.InnerHtml) : "暂无介绍...",
                    XunLeiDownLoadUrlList = lstOnlineUrl,
                    PubDate = pubDate,
                };
                return movieInfo;
            }
            catch (Exception ex)
            {
                LogHelper.Error("GetMovieInfoFromOnlineURL Exception", ex, new { OnloneURL = onlineUrl });
                return null;
            }

        }

    }
}
