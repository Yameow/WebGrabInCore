using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebGrabDemo.Models
{
    public class PositionInfo : BaseFile
    {
        private DateTime pubDate { get; set; }
        public string PositionId { get; set; }
        public string PositionName { get; set; }
        public string PositionURL { get; set; }
        public string PositionDescription { get; set; }
        public string PositionDegree { get; set; }
        public string PositionCity { get; set; }
        public string PositionLevel { get; set; }
        /// <summary>
        /// 全职or实习
        /// </summary>
        public string PositoinType { get; set; }
        public string PositionTag { get; set; }
        public string PositionSalary { get; set; }
        public DateTime PubDate
        {
            get { return pubDate; }
            set
            {
                PubDate = pubDate;
                base.OrderField = pubDate.ToString();
            }
        }
    }
}
