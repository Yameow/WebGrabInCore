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
        public static void WriteToJsonFile<T>(List<T> inputList, string filepath) where T : BaseFile
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
                serializer.Serialize(writer, inputList.OrderBy(m => m.OrderField).ToList());
                sw.Flush();
                writer.Close();
            }
        }

        public static List<K> ReadFromJsonFile<K>(string filepath) where K : BaseFile
        {
            using (var stream = new FileStream(filepath, FileMode.Open))
            {
                StreamReader sr = new StreamReader(stream);
                using (var reader = new JsonTextReader(sr))
                {
                    JsonSerializer serializer = new JsonSerializer
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        Converters = { new JavaScriptDateTimeConverter() }
                    };
                    var list = serializer.Deserialize<List<K>>(reader);
                    return list;
                }
            }
        }
    }
}
