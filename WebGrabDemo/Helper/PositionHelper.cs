using AngleSharp.Parser.Html;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WebGrabDemo.Common;
using WebGrabDemo.Models;

namespace WebGrabDemo.Helper
{
    public class PositionHelper
    {
        private ConcurrentDictionary<string, PositionInfo> _dicPositionInfo = new ConcurrentDictionary<string, PositionInfo>();
        private static HtmlParser htmlParser = new HtmlParser();
        private string _positionFilePath = "";

        /// <summary>
        /// 初始化职位列表
        /// </summary>
        /// <param name="jsonFilePath">Json文件存放位置</param>
        public PositionHelper(string jsonFilePath)
        {
            //先验证json文件是否存在
            _positionFilePath = jsonFilePath;
            try
            {
                if (!File.Exists(jsonFilePath))
                {
                    var pvFile = File.Create(jsonFilePath);
                    pvFile.Flush();
                    pvFile.Dispose();
                    return;
                }
                var positionList = FileHelper.ReadFromJsonFile<PositionInfo>(jsonFilePath);

                foreach (var position in positionList.GroupBy(m => m.PositionURL))
                {
                    if (!_dicPositionInfo.ContainsKey(position.Key))
                        _dicPositionInfo.TryAdd(position.Key, position.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("MovieInfoHelper Exception", ex);

            }
        }

        /// <summary>
        /// 获取当前的职位列表
        /// </summary>
        /// <returns></returns>
        public List<PositionInfo> GetListPositionInfo()
        {
            return _dicPositionInfo.Values.OrderByDescending(m => m.OrderField).ToList();
        }

        /// <summary>
        /// 添加到职位字典
        /// </summary>
        /// <param name="movieInfo"></param>
        /// <returns></returns>
        public bool AddToPositionDic(PositionInfo positionInfo)
        {
            if (positionInfo != null && !_dicPositionInfo.ContainsKey(positionInfo.PositionURL) && _dicPositionInfo.Count % 10 == 0)
            {
                FileHelper.WriteToJsonFile(_dicPositionInfo.Values.ToList(), _positionFilePath);
                LogHelper.Info("Add Movie Success!");
                return _dicPositionInfo.TryAdd(positionInfo.PositionURL, positionInfo);
            }
            return true;
        }

        /// <summary>
        /// 是否包含此职位
        /// </summary>
        /// <param name="onlieURL"></param>
        /// <returns></returns>
        public bool IsContainsPosition(string onlieURL)
        {
            return _dicPositionInfo.ContainsKey(onlieURL);
        }

        /// <summary>
        /// 通过Key获取内存中的职位数据
        /// </summary>
        /// <param name="key">OnlineURL</param>
        /// <returns></returns>
        public PositionInfo GetPositionInfo(String key)
        {
            if (_dicPositionInfo.ContainsKey(key))
                return _dicPositionInfo[key];
            else

                return null;
        }

        /// <summary>
        /// 从在线网页提取职位详细数据
        /// </summary>
        /// <param name="onlineURL"></param>
        /// <returns></returns>
        public static PositionInfo GetPositionInfoFromOnlineURL(string onlineURL, bool isContainIntro = false)
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
                var positionTags = detail.QuerySelector("p.intro-icon").GetElementsByTagName("span").Select(o=>o.InnerHtml);
                var positionTag = string.Join(",", positionTags);
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
