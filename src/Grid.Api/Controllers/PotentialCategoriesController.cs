using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;

namespace Grid.Api.Controllers
{
    public class PotentialCategoriesController : GridApiBaseController
    {
        private readonly ICRMPotentialCategoryRepository _crmPotentialCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PotentialCategoriesController(ICRMPotentialCategoryRepository crmPotentialCategoryRepository,
                                   IUnitOfWork unitOfWork)
        {
            _crmPotentialCategoryRepository = crmPotentialCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _crmPotentialCategoryRepository.GetAll(), "Potential Categories Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _crmPotentialCategoryRepository.Get(id), "Potential Category fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(CRMPotentialCategory potentialCategory)
        {
            ApiResult<CRMPotentialCategory> apiResult;

            if (ModelState.IsValid)
            {
                if (potentialCategory.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var selectedPotentialCategory = _crmPotentialCategoryRepository.Get(potentialCategory.Id);
                        selectedPotentialCategory.Title = potentialCategory.Title;
                        selectedPotentialCategory.Description = potentialCategory.Description;
                        selectedPotentialCategory.UpdatedByUserId = WebUser.Id;
                        _crmPotentialCategoryRepository.Update(selectedPotentialCategory);
                        _unitOfWork.Commit();
                        return selectedPotentialCategory;
                    }, "Potential Category updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        var newPotentialCategory = new CRMPotentialCategory
                        {
                            Title = potentialCategory.Title,
                            Description = potentialCategory.Description,
                            CreatedByUserId = WebUser.Id
                        };
                        _crmPotentialCategoryRepository.Create(newPotentialCategory);
                        _unitOfWork.Commit();
                        return newPotentialCategory;
                    }, "Potential Category created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<CRMPotentialCategory>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _crmPotentialCategoryRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Potential Category deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
