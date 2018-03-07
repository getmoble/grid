using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;
using Newtonsoft.Json;
using Grid.Api.Models.CRM;

namespace Grid.Api.Controllers
{
    public class CRMAccountsController : GridApiBaseController
    {
        private readonly ICRMAccountRepository _crmAccountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CRMAccountsController(ICRMAccountRepository crmAccountRepository,
                                   IUnitOfWork unitOfWork)
        {
            _crmAccountRepository = crmAccountRepository;
            _unitOfWork = unitOfWork;
        }

        // To avoid Cyclic issue of the user.
        public ActionResult Index()
        {
            var accounts = _crmAccountRepository.GetAll();
            var result = new ApiResult<List<CRMAccount>>
            {
                Status = true,
                Result = accounts.ToList()
            };
            var json = JsonConvert.SerializeObject(result, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Content(json, "application/json");
        }

        //[HttpGet]
        //public ActionResult Get(int id)
        //{
        //    var apiResult = TryExecute(() => _crmAccountRepository.Get(id, "AssignedToEmployee.User.Person,Parent"), "CRM Account fetched sucessfully");
        //    var json = JsonConvert.SerializeObject(apiResult, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        //    return Content(json, "application/json");
        //}
        //[HttpGet]
        //public ActionResult Get(int id)
        //{
        //    var apiResult = TryExecute(() => _crmAccountRepository.Get(id, "AssignedToEmployee.User.Person,Parent"), "CRM Account Fetched sucessfully");
        //    return Json(apiResult, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]

        public JsonResult Get(int id)
        {
            var apiResult = TryExecute(() =>
            {
                var account = _crmAccountRepository.Get(id, "AssignedToEmployee.User.Person,Parent");
                var accountVm = new CRMAccountModel(account);
                return accountVm;
            }, "CRM Account Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(CRMAccountModel cRMAccount)
        {
            ApiResult<CRMAccount> apiResult;

            if (ModelState.IsValid)
            {
                if (cRMAccount.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var selectedAccount = _crmAccountRepository.Get(cRMAccount.Id);

                        if (selectedAccount != null)
                        {
                            selectedAccount.Title = cRMAccount.Title;
                            selectedAccount.Industry = cRMAccount.Industry;
                            selectedAccount.EmployeeCount = cRMAccount.EmployeeCount;
                            selectedAccount.FoundedOn = cRMAccount.FoundedOn;
                            selectedAccount.Email = cRMAccount.Email;
                            selectedAccount.PhoneNo = cRMAccount.PhoneNo;
                            selectedAccount.SecondaryEmail = cRMAccount.SecondaryEmail;
                            selectedAccount.OfficePhone = cRMAccount.OfficePhone;
                            selectedAccount.Website = cRMAccount.Website;
                            selectedAccount.Facebook = cRMAccount.Facebook;
                            selectedAccount.Twitter = cRMAccount.Twitter;
                            selectedAccount.GooglePlus = cRMAccount.GooglePlus;
                            selectedAccount.LinkedIn = cRMAccount.LinkedIn;
                            selectedAccount.City = cRMAccount.City;
                            selectedAccount.Country = cRMAccount.Country;
                            selectedAccount.Address = cRMAccount.Address;
                            selectedAccount.CommunicationAddress = cRMAccount.CommunicationAddress;
                            selectedAccount.Expertise = cRMAccount.Expertise;
                            selectedAccount.Description = cRMAccount.Description;
                            selectedAccount.AssignedToEmployeeId = cRMAccount.AssignedToEmployeeId;
                            selectedAccount.ParentId = cRMAccount.ParentId;
                            selectedAccount.UpdatedByUserId = WebUser.Id;

                            _crmAccountRepository.Update(selectedAccount);
                            _unitOfWork.Commit();
                        }

                        return selectedAccount;
                    }, "CRM Account updated sucessfully");
                }

                else
                {
                    apiResult = TryExecute(() =>
                    {

                        var crmAccount = new CRMAccount
                        {
                            Title = cRMAccount.Title,
                            Industry = cRMAccount.Industry,
                            EmployeeCount = cRMAccount.EmployeeCount,
                            FoundedOn = cRMAccount.FoundedOn,
                            Email = cRMAccount.Email,
                            PhoneNo = cRMAccount.PhoneNo,
                            SecondaryEmail = cRMAccount.SecondaryEmail,
                            OfficePhone = cRMAccount.OfficePhone,
                            Website = cRMAccount.Website,
                            Facebook = cRMAccount.Facebook,
                            Twitter = cRMAccount.Twitter,
                            GooglePlus = cRMAccount.GooglePlus,
                            LinkedIn = cRMAccount.LinkedIn,
                            City = cRMAccount.City,
                            Country = cRMAccount.Country,
                            Address = cRMAccount.Address,
                            CommunicationAddress = cRMAccount.CommunicationAddress,
                            Expertise = cRMAccount.Expertise,
                            Description = cRMAccount.Description,
                            AssignedToEmployeeId = cRMAccount.AssignedToEmployeeId,
                            ParentId = cRMAccount.ParentId,
                            CreatedByUserId = WebUser.Id,

                        };
                        _crmAccountRepository.Create(crmAccount);
                        _unitOfWork.Commit();
                        return crmAccount;
                    }, "CRM Account created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<CRMAccount>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _crmAccountRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "CRM Account deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
