using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.TicketDesk.DAL.Interfaces;
using Grid.Features.TicketDesk.Entities;
using Grid.Filters;

namespace Grid.Areas.TicketDesk.Controllers
{
    public class TicketSubCategoriesController : TicketDeskBaseController
    {
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly ITicketSubCategoryRepository _ticketSubCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TicketSubCategoriesController(ITicketSubCategoryRepository ticketSubCategoryRepository,
                                             ITicketCategoryRepository ticketCategoryRepository,
                                             IUnitOfWork unitOfWork)
        {
            _ticketCategoryRepository = ticketCategoryRepository;
            _ticketSubCategoryRepository = ticketSubCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        #region Ajax Calls
        [HttpPost]
        public ActionResult GetSubCategoriesByCategoryId(int id)
        {
            var subCategories = _ticketSubCategoryRepository.GetAllBy(d => d.TicketCategoryId == id).ToList();
            return Json(subCategories);
        }
        #endregion

        public ActionResult Index()
        {
            var ticketSubCategories = _ticketSubCategoryRepository.GetAll("TicketCategory");
            return View(ticketSubCategories.ToList());
        }
    }
}
