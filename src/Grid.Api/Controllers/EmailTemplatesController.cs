using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Api.Controllers
{
    public class EmailTemplatesController: GridApiBaseController
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmailTemplatesController(IEmailTemplateRepository emailTemplateRepository,
                                        IUnitOfWork unitOfWork)
        {
            _emailTemplateRepository = emailTemplateRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _emailTemplateRepository.GetAll(), "Email Templates Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _emailTemplateRepository.Get(id), "Email Template fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(EmailTemplate vm)
        {
            ApiResult<EmailTemplate> apiResult;

            if (ModelState.IsValid)
            {
                if (vm.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var emailTemplate = _emailTemplateRepository.Get(vm.Id);
                        emailTemplate.Name = vm.Name;
                        emailTemplate.Content = vm.Content;
                        emailTemplate.UpdatedByUserId = WebUser.Id;                     
                        _emailTemplateRepository.Update(emailTemplate);
                        _unitOfWork.Commit();
                        return vm;
                    }, "Email Template updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        vm.CreatedByUserId = WebUser.Id;
                        _emailTemplateRepository.Create(vm);
                        _unitOfWork.Commit();
                        return vm;
                    }, "Email Template created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<EmailTemplate>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _emailTemplateRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Email Template deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
