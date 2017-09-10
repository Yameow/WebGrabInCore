using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebGrabDemo.Models
{
    public class MovieInfo : BaseFile
    {
        private DateTime pubDate;
        public string MovieName { get; set; }

        public string Dy2018OnlineUrl { get; set; }

        [JsonIgnoreAttribute]
        public string MovieIntro { get; set; }

        public DateTime PubDate {
            get { return pubDate; }
            set {
                PubDate = pubDate;
                base.OrderField = pubDate.ToString();
            }
        }

        public List<string> XunLeiDownLoadURLList { get; set; }

    }
}
