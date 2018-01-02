using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using WebGrabDemo.Common;
using WebGrabDemo.Helper;
using WebGrabDemo.Models;

namespace WebGrabDemo.Service
{
    public class PositionService
    {
        private static HtmlParser htmlParser = new HtmlParser();

        private static PositionHelper positionNewList = new PositionHelper(Path.Combine(GlobalConfig.WWWRootPath, "position_New.json"));

        /// <summary>
        /// 抓取数据
        /// </summary>
        /// <param name="indexPageCount"></param>
        public static void GrabPositionInfo(int indexPageCount = 0)
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
                            var indexURL = $"http://www.zheyibu.com/sou/0-0-0-0-0-0-0-1-0-{i}-1003";
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
                        PositionInfo positionInfo = GetPositionInfoFromOnlineURL(onlineURL);
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

        /// <summary>
        /// 从在线网页提取职位详细数据
        /// </summary>
        /// <param name="onlineURL"></param>
        /// <returns></returns>
        private static PositionInfo GetPositionInfoFromOnlineURL(string onlineURL, bool isContainIntro = false)
        {
            try
            {
                var positionHTML = RequestHelper.HttpGet(onlineURL, Encoding.UTF8);
                if (string.IsNullOrEmpty(positionHTML))
                    return null;
                var positionDoc = htmlParser.Parse(positionHTML);
                var detail = positionDoc.GetElementsByClassName("details-left").FirstOrDefault();

                var updatetime = detail.QuerySelector("div.job-intro").GetElementsByTagName("span").FirstOrDefault().InnerHtml.Split(' ').FirstOrDefault();
                DateTime pubDate = default(DateTime);
                if (updatetime != null && !string.IsNullOrEmpty(updatetime))
                {
                    DateTime.TryParse(updatetime, out pubDate);
                }
                var positionName = detail.QuerySelector("div.job-intro").GetElementsByTagName("a").FirstOrDefault().InnerHtml;
                var positionId = onlineURL.Split('/').LastOrDefault().Split('.').FirstOrDefault();
                var positionDescription = detail.QuerySelector("div.intro-position").GetElementsByTagName("p").LastOrDefault().InnerHtml;
                var positionDegree = detail.QuerySelector("div.intro-demond").GetElementsByTagName("p")[1].InnerHtml;
                var positionCity = detail.QuerySelector("p.intro-divide").GetElementsByTagName("span")[1].InnerHtml;
                var positionLevel = detail.QuerySelector("");
                var positionType = detail.QuerySelector("p.intro-divide").GetElementsByTagName("span")[0].InnerHtml;
                var positionTags = detail.QuerySelector("p.intro-icon") != null? detail.QuerySelector("p.intro-icon").GetElementsByTagName("span").Select(o => o.InnerHtml):null;
                var positionTag = positionTags == null? null : string.Join(",", positionTags);
                var positionSalary = detail.QuerySelector("p.intro-divide").GetElementsByTagName("span")[2].InnerHtml;
                var positionInfo = new PositionInfo()
                {
                    PubDate = pubDate,
                    PositionId = positionId,
                    PositionName = positionName,
                    PositionURL = onlineURL,
                    PositionDescription = positionDescription,
                    PositionDegree = positionDegree,
                    PositionCity = positionCity,
                    PositoinType = positionType,
                    PositionTag = positionTag,
                    PositionSalary = positionSalary
                };
                return positionInfo;
            }
            catch (Exception ex)
            {
                LogHelper.Error("GetPositionInfoFromOnlineURL Exception", ex, new { OnloneURL = onlineURL });
                return null;
            }

        }
    }
}
