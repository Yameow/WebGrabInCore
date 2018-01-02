using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebGrabDemo.Models
{
    public class MovieInfo : BaseFile
    {
        private DateTime pubDate { get; set; }
        public string MovieName { get; set; }

        public string Dy2018OnlineUrl { get; set; }

        [JsonIgnoreAttribute]
        public string MovieIntro { get; set; }

        public DateTime PubDate
        {
            get { return pubDate; }
            set
            {
                pubDate = value;
                base.OrderField = pubDate.ToString();
            }
        }    

    public List<string> XunLeiDownLoadUrlList { get; set; }

    }
}
