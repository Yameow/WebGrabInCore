using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebGrabDemo.Common;
using WebGrabDemo.Helper;
using WebGrabDemo.Models;

namespace WebGrabDemo.Grab
{
    public class Position
    {
        private static PositionHelper positionNewList = new PositionHelper(Path.Combine(GlobalConfig.WWWRootPath, "position_New.json"));

        private static HtmlParser htmlParser = new HtmlParser();

        /// <summary>
        /// 抓取数据
        /// </summary>
        /// <param name="indexPageCount"></param>
        public static void CrawlLatestMovieInfo(int indexPageCount = 0)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    LogHelper.Info("Grab Position_New Start...");
                    indexPageCount = indexPageCount == 0 ? 3 : indexPageCount;
                    //取前五页
                    for (var i = 1; i < indexPageCount; i++)
                    {
                        try
                        {
                            var index = i == 1 ? "" : "_" + i;
                            var indexURL = $"http://www.zheyibu.com/sou/0-0-0-0-0-0-0-1-0-{index}-0";
                            GrabPositionElement(indexURL);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("Grab Position_New Exception", ex);
                        }
                    }

                    LogHelper.Info("Grab Position_New Finish!");
                }
                catch (Exception ex)
                {
                    LogHelper.Error("Grab Position_New Exception", ex);
                }
            });
        }

        /// <summary>
        /// 从在线网页提取数据
        /// </summary>
        /// <param name="i"></param>
        private static void GrabPositionElement(string indexUrl)
        {

            var htmlDoc = RequestHelper.HttpGet(indexUrl, Encoding.UTF8);
            var dom = htmlParser.Parse(htmlDoc);
            var lstDivInfo = dom.QuerySelectorAll("ul.left-result");
            if (lstDivInfo != null)
            {
                lstDivInfo.FirstOrDefault().QuerySelectorAll("a").Where(a => a.GetAttribute("href").Contains("job/")).ToList()
                .ForEach(a =>
                {
                    var onlineURL = a.GetAttribute("href");
                    if (!positionNewList.IsContainsPosition(onlineURL))
                    {
                        PositionInfo positionInfo = PositionHelper.GetPositionInfoFromOnlineURL(onlineURL);
                        if (positionInfo != null)
                            positionNewList.AddToPositionDic(positionInfo);
                    }
                });
            }
        }

        /// <summary>
        /// 获取全部的电影数据
        /// </summary>
        /// <returns></returns>
        public static List<PositionInfo> GetAllPositionInfo()
        {
            return positionNewList.GetListPositionInfo(); ;
        }

        public static PositionInfo GetPositionInfoByOnlineURL(string onlineURL)
        {
            return positionNewList.GetPositionInfo(onlineURL);
        }
    }
}
