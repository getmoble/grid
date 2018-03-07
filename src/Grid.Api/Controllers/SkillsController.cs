using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Api.Controllers
{
    public class SkillsController : GridApiBaseController
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SkillsController(ISkillRepository skillRepository,
                                   IUnitOfWork unitOfWork)
        {
            _skillRepository = skillRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _skillRepository.GetAll(), "Skills Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _skillRepository.Get(id), "Skill Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(Skill skill)
        {
            ApiResult<Skill> apiResult;

            if (ModelState.IsValid)
            {
                if (skill.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _skillRepository.Update(skill);
                        _unitOfWork.Commit();
                        return skill;
                    }, "Skill updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _skillRepository.Create(skill);
                        _unitOfWork.Commit();
                        return skill;
                    }, "Skill created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Skill>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _skillRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Skill deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
