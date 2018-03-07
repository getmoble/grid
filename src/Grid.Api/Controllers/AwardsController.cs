using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Api.Controllers
{
    public class AwardsController: GridApiBaseController
    {
        private readonly IAwardRepository _awardRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AwardsController(IAwardRepository awardRepository,
                                   IUnitOfWork unitOfWork)
        {
            _awardRepository = awardRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _awardRepository.GetAll(), "Awards Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _awardRepository.Get(id), "Award Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(Award role)
        {
            ApiResult<Award> apiResult;

            if (ModelState.IsValid)
            {
                if (role.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _awardRepository.Update(role);
                        _unitOfWork.Commit();
                        return role;
                    }, "Award updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _awardRepository.Create(role);
                        _unitOfWork.Commit();
                        return role;
                    }, "Award created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Award>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _awardRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Award deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
