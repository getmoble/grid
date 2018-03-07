using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Auth.OAuth2.Mvc.Controllers;
using Google.Apis.Drive.v2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;

namespace Grid.Controllers
{
    public class ExportController : AuthCallbackController
    {
        public static GoogleClientSecrets GetClientConfiguration()
        {
            var path = HostingEnvironment.MapPath("~/client_secret.json");
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return GoogleClientSecrets.Load(stream);
            }
        }

        public async Task<ActionResult> Index()
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
               AuthorizeAsync(new CancellationToken());

            if (result.Credential != null)
            {
                var service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = "Grid Intranet"
                });

                var list = await service.Files.List().ExecuteAsync();
                ViewBag.Message = "FILE COUNT IS: " + list.Items.Count();
            }

            return RedirectToAction("Index", "Home");
        }

        protected override Google.Apis.Auth.OAuth2.Mvc.FlowMetadata FlowData
        {
            get { return new AppFlowMetadata(); }
        }
    }
}