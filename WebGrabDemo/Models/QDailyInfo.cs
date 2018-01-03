using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebGrabDemo.Models
{
    public class QDailyInfo:BaseFile
    {
        private DateTime pubDate { get; set; }
        public string ArticleName { get; set; }

        public string OriginalUrl { get; set; }
        public int QDailyId { get; set; }

        public string ArticleContent { get; set; }

        public DateTime PubDate
        {
            get { return pubDate; }
            set
            {
                pubDate = value;
                base.OrderField = pubDate.ToString();
            }
        }
    }
}
