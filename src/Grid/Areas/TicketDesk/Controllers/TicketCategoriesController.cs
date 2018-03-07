using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.TicketDesk.DAL.Interfaces;
using Grid.Features.TicketDesk.Entities;

namespace Grid.Areas.TicketDesk.Controllers
{
    public class TicketCategoriesController : TicketDeskBaseController
    {
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TicketCategoriesController(ITicketCategoryRepository ticketCategoryRepository, 
                                          IUnitOfWork unitOfWork)
        {
            _ticketCategoryRepository = ticketCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var categories = _ticketCategoryRepository.GetAll();
            return View(categories);
        }
      
    }
}
