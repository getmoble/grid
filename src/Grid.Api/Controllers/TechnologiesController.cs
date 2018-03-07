using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Api.Controllers
{
    public class TechnologiesController: GridApiBaseController
    {
        private readonly ITechnologyRepository _technologyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TechnologiesController(ITechnologyRepository technologyRepository,
                                   IUnitOfWork unitOfWork)
        {
            _technologyRepository = technologyRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _technologyRepository.GetAll(), "Technologies Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _technologyRepository.Get(id), "Technology Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(Technology technology)
        {
            ApiResult<Technology> apiResult;

            if (ModelState.IsValid)
            {
                if (technology.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _technologyRepository.Update(technology);
                        _unitOfWork.Commit();
                        return technology;
                    }, "Technology updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _technologyRepository.Create(technology);
                        _unitOfWork.Commit();
                        return technology;
                    }, "Technology created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Technology>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _technologyRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Technology deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
