using System.Linq;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Api.Models.HRMS;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;
using Grid.Features.HRMS.ViewModels;

namespace Grid.Api.Controllers
{
    public class DesignationsController: GridApiBaseController
    {
        private readonly IDesignationRepository _designationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DesignationsController(IDesignationRepository designationRepository,
                                   IUnitOfWork unitOfWork)
        {
            _designationRepository = designationRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() =>
            {
                return _designationRepository.GetAll().Select(d => new DesignationModel(d)).ToList();
            }, "Designations Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _designationRepository.Get(id), "Designation Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _designationRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Designation deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(DesignationViewModel designationVm)
        {
            ApiResult<Designation> apiResult;

            if (ModelState.IsValid)
            {
                if (designationVm.Id > 0)
                {
                    apiResult = TryExecute(() => 
                    {
                        var designation = new Designation
                        {
                            Title = designationVm.Title,
                            DepartmentId = designationVm.DepartmentId,
                            Band = designationVm.Band,
                            Description = designationVm.Description,
                            MailAlias = designationVm.MailAlias,
                            Id = designationVm.Id
                        };
                        _designationRepository.Update(designation);
                        _unitOfWork.Commit();
                        return designation;
                    }, "Designation updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        var designation = new Designation
                        {
                            Title = designationVm.Title,
                            DepartmentId = designationVm.DepartmentId,
                            Band = designationVm.Band,
                            Description = designationVm.Description,
                            MailAlias = designationVm.MailAlias,
                            Id = designationVm.Id
                        };
                        _designationRepository.Create(designation);
                        _unitOfWork.Commit();
                        return designation;
                    }, "Designation created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Designation>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
