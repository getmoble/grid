using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Dropbox.Api;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.Settings.Services.Interfaces;

namespace Grid.Areas.DMS.Controllers
{
    public class ManageController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ISettingsService _settingsService;

        public ManageController(IUserRepository userRepository,
                                  ISettingsService settingsService)
        {
            _userRepository = userRepository;
            _settingsService = settingsService;
        }

        // GET: DMS/Manage
        public async Task<ActionResult> Index()
        {
            var settings = _settingsService.GetSiteSettings();
            using (var dbx = new DropboxClient(settings.DropBoxSettings.AccessToken))
            {
                var list = await dbx.Files.ListFolderAsync(string.Empty);

                // show folders then files
                foreach (var item in list.Entries.Where(i => i.IsFolder))
                {
                    Console.WriteLine("D  {0}/", item.Name);
                }

                foreach (var item in list.Entries.Where(i => i.IsFile))
                {
                    Console.WriteLine("F{0,8} {1}", item.AsFile.Size, item.Name);
                }
            }

            return View();
        }
    }
}