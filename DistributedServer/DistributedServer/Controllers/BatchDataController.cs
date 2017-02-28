using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using DistributedServer.Models;
using DistributedServer.App_Code;

namespace DistributedServer.Controllers
{
    public class BatchDataController : ApiController
    {
        MasterDictionary master = MasterDictionary.Instance;
        BatchData[] batches = new BatchData[1];

        public IEnumerable<BatchData> GetAllBatches()
        {
            batches[1] = GetNext();
            return batches;
        }

        public IHttpActionResult GetBatchData(int id)
        {
            id = 0;
            if (batches == null)
                batches[0] = GetNext();
            if (batches[0] == null)
                batches[0] = GetNext();

            var batch = batches[0];
            if (batch == null)
            {
                return NotFound();
            }
            return Ok(batch);
        }

        private BatchData GetNext()
        {
            return MasterDictionary.GetRandomBatch(0);
        }
    }
}
