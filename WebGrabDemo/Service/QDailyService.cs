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
    public class QDailyService
    {
        private static HtmlParser htmlParser = new HtmlParser();

        private static QDailyHelper qDailyList = new QDailyHelper(Path.Combine(GlobalConfig.WWWRootPath, "qDaily.json"));

        /// <summary>
        /// 抓取数据
        /// </summary>
        /// <param name="indexPageCount"></param>
        public static void GrabIQDailynfo(int indexPageCount = 0)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    LogHelper.Info("Grab QDaily Start...");
                    //todo 这里可以获取id only
                    var lastId = qDailyList.GetListQDailyInfo().Count != 0
                        ? qDailyList.GetListQDailyInfo().Max(o => o.QDailyId)
                        : 1;

                    //取前10页
                    for (var i = lastId; i < lastId + 100; i++)
                    {
                        try
                        {
                            var indexURL = $"http://www.qdaily.com/articles/{i}.html";
                            GrabQDailyElement(indexURL);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("Grab QDailyInfo Exception", ex);
                        }
                    }
                    LogHelper.Info("Grab QDailyInfo Finish!");
                }
                catch (Exception ex)
                {
                    LogHelper.Error("Grab QDailyInfo Exception", ex);
                }
            });
        }

        /// <summary>
        /// 从在线网页提取数据
        /// </summary>
        /// <param name="i"></param>
        private static void GrabQDailyElement(string indexUrl)
        {
            QDailyInfo qDailyInfo = GetQDailyInfoFromOnlineURL(indexUrl);
            if (qDailyInfo != null)
                qDailyList.AddToQDailyDic(qDailyInfo);
        }

        /// <summary>
        /// 获取全部的数据
        /// </summary>
        /// <returns></returns>
        public static List<QDailyInfo> GetAllQDailyInfo()
        {
            return qDailyList.GetListQDailyInfo(); ;
        }

        public static QDailyInfo GetQDailyInfoByOnlineUrl(string onlineUrl)
        {
            return qDailyList.GetDailyInfo(onlineUrl);
        }

        /// <summary>
        /// 从在线网页提取职位详细数据
        /// </summary>
        /// <param name="onlineURL"></param>
        /// <returns></returns>
        private static QDailyInfo GetQDailyInfoFromOnlineURL(string onlineURL, bool isContainIntro = false)
        {
            try
            {
                var qDailyHTML = RequestHelper.HttpGet(onlineURL, Encoding.UTF8);
                if (string.IsNullOrEmpty(qDailyHTML) || qDailyHTML.Contains("error404"))
                    return null;
                var qDailyDoc = htmlParser.Parse(qDailyHTML);
                var detail = qDailyDoc.GetElementsByClassName("com-article-detail").FirstOrDefault();

                var updatetime = detail.QuerySelector("span.smart-date").GetAttribute("data-origindate").ToString();
                DateTime pubDate = default(DateTime);
                if (updatetime != null && !string.IsNullOrEmpty(updatetime))
                {
                    DateTime.TryParse(updatetime, out pubDate);
                }
                var articleName = detail.QuerySelector("h2.title").InnerHtml;
                var articleContent = detail.QuerySelector("div.detail").InnerHtml;
                var qDailyInfo = new QDailyInfo()
                {
                    ArticleName = articleName,
                    OriginalUrl = onlineURL,
                    QDailyId = Int32.Parse(onlineURL.Split('/').LastOrDefault().Split('.').FirstOrDefault()),
                    ArticleContent = articleContent,
                    
                    PubDate = pubDate,
                };
                return qDailyInfo;
            }
            catch (Exception ex)
            {
                LogHelper.Error("GetPositionInfoFromOnlineURL Exception", ex, new { OnloneURL = onlineURL });
                return null;
            }

        }
    }
}
