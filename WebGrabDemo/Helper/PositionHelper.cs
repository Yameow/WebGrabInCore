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

        private string _positionJsonFilePath = "";

        /// <summary>
        /// 初始化职位列表
        /// </summary>
        /// <param name="jsonFilePath">Json文件存放位置</param>
        public PositionHelper(string jsonFilePath)
        {
            //先验证json文件是否存在
            _positionJsonFilePath = jsonFilePath;
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
            if (positionInfo != null && !_dicPositionInfo.ContainsKey(positionInfo.PositionURL))
            {
                FileHelper.WriteToJsonFile(_dicPositionInfo.Values.ToList(), _positionJsonFilePath);
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
        public bool IsContainsPosition(string onlieUrl)
        {
            return _dicPositionInfo.ContainsKey(onlieUrl);
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

    }
}
