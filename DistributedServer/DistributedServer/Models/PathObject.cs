using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributedServer.Models
{
    public class PathObject
    {
        public int id { get; set; }
        public string dirName { get; set; }
        public string path { get; set; }
        public bool isAvail { get; set; }
        public bool isCompleted { get; set; }
        public int LastIndex { get; set; }
    }
}