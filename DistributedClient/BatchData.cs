using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedClient
{
    class BatchData
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Data { get; set; }
        public int LastComplete { get; set; }
    }
}
