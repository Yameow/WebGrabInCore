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
    public class QDailyHelper
    {
        private ConcurrentDictionary<string, QDailyInfo> _dicQDailyInfo = new ConcurrentDictionary<string, QDailyInfo>();

        private string _qDailyJsonFilePath = "";

        /// <summary>
        /// 初始化QDaily列表
        /// </summary>
        /// <param name="jsonFilePath">Json文件存放位置</param>
        public QDailyHelper(string jsonFilePath)
        {
            //先验证json文件是否存在
            _qDailyJsonFilePath = jsonFilePath;
            try
            {
                if (!File.Exists(jsonFilePath))
                {
                    var pvFile = File.Create(jsonFilePath);
                    pvFile.Flush();
                    pvFile.Dispose();
                    return;
                }
                var qDailyList = FileHelper.ReadFromJsonFile<QDailyInfo>(jsonFilePath);

                foreach (var qDaily in qDailyList.GroupBy(m => m.OriginalUrl))
                {
                    if (!_dicQDailyInfo.ContainsKey(qDaily.Key))
                        _dicQDailyInfo.TryAdd(qDaily.Key, qDaily.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("QDailynfoHelper Exception", ex);

            }
        }

        /// <summary>
        /// 获取当前的列表
        /// </summary>
        /// <returns></returns>
        public List<QDailyInfo> GetListQDailyInfo()
        {
            return _dicQDailyInfo.Values.OrderByDescending(m => m.OrderField).ToList();
        }

        /// <summary>
        /// 添加到字典
        /// </summary>
        /// <param name="movieInfo"></param>
        /// <returns></returns>
        public bool AddToQDailyDic(QDailyInfo qDailyInfo)
        {
            if (qDailyInfo != null && !_dicQDailyInfo.ContainsKey(qDailyInfo.OriginalUrl))
            {
                FileHelper.WriteToJsonFile(_dicQDailyInfo.Values.ToList(), _qDailyJsonFilePath);
                LogHelper.Info("Add QDaily Success!");
                return _dicQDailyInfo.TryAdd(qDailyInfo.OriginalUrl, qDailyInfo);
            }
            return true;
        }

        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="onlieURL"></param>
        /// <returns></returns>
        public bool IsContainsQDaily(string onlieUrl)
        {
            return _dicQDailyInfo.ContainsKey(onlieUrl);
        }

        /// <summary>
        /// 通过Key获取内存中的数据
        /// </summary>
        /// <param name="key">OnlineURL</param>
        /// <returns></returns>
        public QDailyInfo GetDailyInfo(String key)
        {
            if (_dicQDailyInfo.ContainsKey(key))
                return _dicQDailyInfo[key];
            else
                return null;
        }
    }
}
