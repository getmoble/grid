using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;

namespace Grid.Api.Controllers
{
    public class LeadCategoriesController : GridApiBaseController
    {
        private readonly ICRMLeadCategoryRepository _crmLeadCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LeadCategoriesController(ICRMLeadCategoryRepository crmLeadCategoryRepository,
                                   IUnitOfWork unitOfWork)
        {
            _crmLeadCategoryRepository = crmLeadCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _crmLeadCategoryRepository.GetAll(), "Lead Categories Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _crmLeadCategoryRepository.Get(id), "Lead Category fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(CRMLeadCategory vm)
        {
            ApiResult<CRMLeadCategory> apiResult;

            if (ModelState.IsValid)
            {
                if (vm.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var selectedLeadCategory = _crmLeadCategoryRepository.Get(vm.Id);
                        selectedLeadCategory.Title = vm.Title;
                        selectedLeadCategory.Description = vm.Description;
                        selectedLeadCategory.UpdatedByUserId = WebUser.Id;
                        _crmLeadCategoryRepository.Update(selectedLeadCategory);
                        _unitOfWork.Commit();
                        return selectedLeadCategory;
                    }, "Lead Category updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        var newLeadCategory = new CRMLeadCategory
                        {
                            Title = vm.Title,
                            Description = vm.Description,
                            CreatedByUserId = WebUser.Id
                        };
                        _crmLeadCategoryRepository.Create(newLeadCategory);
                        _unitOfWork.Commit();
                        return newLeadCategory;
                    }, "Lead Category created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<CRMLeadCategory>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _crmLeadCategoryRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Lead Category deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
