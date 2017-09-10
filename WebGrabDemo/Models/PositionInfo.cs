using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebGrabDemo.Models
{
    public class PositionInfo : BaseFile
    {
        public string PositionId { get; set; }
        public string PositionName { get; set; }
        public string PositionURL { get; set; }
    }
}
