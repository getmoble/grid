using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Api.Controllers
{
    public class HobbiesController : GridApiBaseController
    {
        private readonly IHobbyRepository _hobbyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HobbiesController(IHobbyRepository hobbyRepository,
                                   IUnitOfWork unitOfWork)
        {
            _hobbyRepository = hobbyRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _hobbyRepository.GetAll(), "Hobbies Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _hobbyRepository.Get(id), "Hobby Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(Hobby hobby)
        {
            ApiResult<Hobby> apiResult;

            if (ModelState.IsValid)
            {
                if (hobby.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _hobbyRepository.Update(hobby);
                        _unitOfWork.Commit();
                        return hobby;
                    }, "Hobby updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _hobbyRepository.Create(hobby);
                        _unitOfWork.Commit();
                        return hobby;
                    }, "Hobby created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Hobby>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _hobbyRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Hobby deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
