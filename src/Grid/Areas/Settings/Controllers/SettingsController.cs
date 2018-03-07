using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Dropbox.Api;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.Settings.Services.Interfaces;
using Grid.Infrastructure;
using Grid.Features.Settings.ViewModels;

namespace Grid.Areas.Settings.Controllers
{
    public class SettingsController : GridBaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISettingsService _settingsService;

        public SettingsController(IUserRepository userRepository, IEmployeeRepository employeeRepository,
                                  ISettingsService settingsService)
        {
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
            _settingsService = settingsService;
        }

        public ActionResult Index()
        {
            var settings = _settingsService.GetSiteSettings();
            var employees = _employeeRepository.GetAll("User,User.Person").ToList();           

            ViewBag.ITDepartmentLevel1 = new SelectList(employees, "Id", "User.Person.Name", settings.POCSettings.ITDepartmentLevel1);
            ViewBag.ITDepartmentLevel2 = new SelectList(employees, "Id", "User.Person.Name", settings.POCSettings.ITDepartmentLevel2);
            ViewBag.HRDepartmentLevel1 = new SelectList(employees, "Id", "User.Person.Name", settings.POCSettings.HRDepartmentLevel1);
            ViewBag.HRDepartmentLevel2 = new SelectList(employees, "Id", "User.Person.Name", settings.POCSettings.HRDepartmentLevel2);
            ViewBag.SalesDepartmentLevel1 = new SelectList(employees, "Id", "User.Person.Name", settings.POCSettings.SalesDepartmentLevel1);
            ViewBag.SalesDepartmentLevel2 = new SelectList(employees, "Id", "User.Person.Name", settings.POCSettings.SalesDepartmentLevel2);

            var vm = new SettingsViewModel
            {
                ApplicationName = settings.ApplicationName,
                DropBoxApplicationKey = settings.DropBoxSettings.ApplicationKey,
                DropBoxApplicationSecret = settings.DropBoxSettings.ApplicationSecret,
                BlobAccountKey = settings.BlobSettings.AccountKey,
                BlobAccountName = settings.BlobSettings.AccountName,
                SlackConsumerKey = settings.SlackSettings.ConsumerKey,
                SlackConsumerSecret = settings.SlackSettings.ConsumerSecret,
                SlackScopes = settings.SlackSettings.Scopes,
                Server = settings.EmailSettings.Server,
                Port = settings.EmailSettings.Port,
                Username = settings.EmailSettings.Username,
                Password = settings.EmailSettings.Password,
                FromEmail = settings.EmailSettings.FromEmail,
                FromName = settings.EmailSettings.FromName,
                AdminEmail = settings.EmailSettings.AdminEmail,
                ITDepartmentLevel1 = settings.POCSettings.ITDepartmentLevel1,
                ITDepartmentLevel2 = settings.POCSettings.ITDepartmentLevel2,
                HRDepartmentLevel1 = settings.POCSettings.HRDepartmentLevel1,
                HRDepartmentLevel2 = settings.POCSettings.HRDepartmentLevel2,
                SalesDepartmentLevel1 = settings.POCSettings.SalesDepartmentLevel1,
                SalesDepartmentLevel2 = settings.POCSettings.SalesDepartmentLevel2,
                MaxTimeSheetMisses = settings.TimeSheetSettings.MaxTimeSheetMisses,
                MaxTimeSheetApprovalMisses = settings.TimeSheetSettings.MaxTimeSheetApprovalMisses
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(SettingsViewModel vm)
        {
            var settings = _settingsService.GetSiteSettings();

            settings.ApplicationName = vm.ApplicationName;

            settings.DropBoxSettings.ApplicationKey = vm.DropBoxApplicationKey;
            settings.DropBoxSettings.ApplicationSecret = vm.DropBoxApplicationSecret;

            settings.BlobSettings.AccountKey = vm.BlobAccountKey;
            settings.BlobSettings.AccountName = vm.BlobAccountName;

            settings.SlackSettings.ConsumerKey = vm.SlackConsumerKey;
            settings.SlackSettings.ConsumerSecret = vm.SlackConsumerSecret;
            settings.SlackSettings.Scopes = vm.SlackScopes;

            settings.EmailSettings.Server = vm.Server;
            settings.EmailSettings.Port = vm.Port;
            settings.EmailSettings.Username = vm.Username;
            settings.EmailSettings.Password = vm.Password;
            settings.EmailSettings.FromEmail = vm.FromEmail;
            settings.EmailSettings.FromName = vm.FromName;
            settings.EmailSettings.AdminEmail = vm.AdminEmail;

            settings.POCSettings.ITDepartmentLevel1 = vm.ITDepartmentLevel1;
            settings.POCSettings.ITDepartmentLevel2 = vm.ITDepartmentLevel2;
            settings.POCSettings.HRDepartmentLevel1 = vm.HRDepartmentLevel1;
            settings.POCSettings.HRDepartmentLevel2 = vm.HRDepartmentLevel2;
            settings.POCSettings.SalesDepartmentLevel1 = vm.SalesDepartmentLevel1;
            settings.POCSettings.SalesDepartmentLevel2 = vm.SalesDepartmentLevel2;

            settings.TimeSheetSettings.MaxTimeSheetMisses = vm.MaxTimeSheetMisses;
            settings.TimeSheetSettings.MaxTimeSheetApprovalMisses = vm.MaxTimeSheetApprovalMisses;

            _settingsService.UpdateSiteSettings(settings);

            return RedirectToAction("Index", "Home", new { Area = ""});
        }

        [HttpGet]
        public ActionResult ConnectDropBox()
        {
            var settings = _settingsService.GetSiteSettings();
            var oauth2State = Guid.NewGuid().ToString("N");
            var redirect = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Code, settings.DropBoxSettings.ApplicationKey, "http://localhost:43729/Settings/Settings/DropboxCallback", oauth2State);
            return Redirect(redirect.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> DropboxCallback(string code, string state)
        {
            var settings = _settingsService.GetSiteSettings();
            var response = await DropboxOAuth2Helper.ProcessCodeFlowAsync(code, settings.DropBoxSettings.ApplicationKey, settings.DropBoxSettings.ApplicationSecret, "http://localhost:43729/Settings/Settings/DropboxCallback");
            settings.DropBoxSettings.AccessToken = response.AccessToken;
            _settingsService.UpdateSiteSettings(settings);

            return RedirectToAction("Index", "Home", new { Area = "" });
        }
    }
}
