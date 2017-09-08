using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Concurrent;
using WebGrabDemo.Models;

namespace WebGrabDemo.Common
{
    public class FileHelper
    {
        /// <summary>
        /// 写入json文件
        /// </summary>
        public static void WriteToJsonFile(ConcurrentDictionary<string, BaseFile> inputDic ,string filepath, bool isWriteNow = false)
        {
            if (inputDic.Count % 10 == 0 || isWriteNow)
            {
                using (var stream = new FileStream(filepath, FileMode.OpenOrCreate))
                {
                    StreamWriter sw = new StreamWriter(stream);
                    JsonSerializer serializer = new JsonSerializer
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        Converters = { new JavaScriptDateTimeConverter() }
                    };
                    //构建Json.net的写入流  
                    JsonWriter writer = new JsonTextWriter(sw);
                    //把模型数据序列化并写入Json.net的JsonWriter流中  
                    serializer.Serialize(writer, inputDic.Values.OrderBy(m => m.OrderDate).ToList());
                    sw.Flush();
                    writer.Close();
                }
            }
        }

        public static ConcurrentDictionary<string, BaseFile> ReadFromJsonFile(string filepath)
        {
            using (var stream = new FileStream(filepath, FileMode.Open))
            {
                StreamReader sw = new StreamReader(stream);
                JsonSerializer serializer = new JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = { new JavaScriptDateTimeConverter() }
                };
                serializer.Deserialize();
            }
        }
    }
}
