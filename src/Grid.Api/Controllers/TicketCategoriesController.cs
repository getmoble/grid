using System.Linq;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.TicketDesk.DAL.Interfaces;
using Grid.Features.TicketDesk.Entities;

namespace Grid.Api.Controllers
{
    public class TicketCategoriesController : GridApiBaseController
    {
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TicketCategoriesController(ITicketCategoryRepository ticketCategoryRepository,
                                   IUnitOfWork unitOfWork)
        {
            _ticketCategoryRepository = ticketCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() =>
            {
                return _ticketCategoryRepository.GetAll(o => o.OrderByDescending(t => t.CreatedOn));
            }, "Ticket Categories Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _ticketCategoryRepository.Get(id), "Ticket Category fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(TicketCategory ticketCategory)
        {
            ApiResult<TicketCategory> apiResult;

            if (ModelState.IsValid)
            {
                if (ticketCategory.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _ticketCategoryRepository.Update(ticketCategory);
                        _unitOfWork.Commit();
                        return ticketCategory;
                    }, "Ticket Category updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _ticketCategoryRepository.Create(ticketCategory);
                        _unitOfWork.Commit();
                        return ticketCategory;
                    }, "Ticket Category created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<TicketCategory>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _ticketCategoryRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Ticket Category deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
