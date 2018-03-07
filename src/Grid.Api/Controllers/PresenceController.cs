using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Grid.AccessLogs;
using Grid.Clients.ITSync.Models;
using Grid.Infrastructure;
using Grid.Infrastructure.Filters;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace Grid.Api.Controllers
{
    public class PresenceController : GridBaseController
    {
        [HttpPost]
        [APIIdentityInjector]
        public ActionResult Create(PresenceModel vm)
        {
            if (WebUser != null)
            {
                // Create the batch operation.
                var batchOperation = new TableBatchOperation();

                var userActivities = JsonConvert.DeserializeObject<List<UserActivity>>(vm.Payload);
                var service = new CloudTableService();

                foreach (var userActivity in userActivities)
                {
                    userActivity.PartitionKey = WebUser.Id.ToString();
                    userActivity.RowKey = Guid.NewGuid().ToString("N");
                    userActivity.AssetId = vm.AssetId;
                    userActivity.Name = WebUser.Name;
                    batchOperation.Insert(userActivity);
                }

                if(userActivities.Count > 0)
                    service.AddBatch(batchOperation);

                return Json(true);
            }

            return Json(false);
        }
    }
}