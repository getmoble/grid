using System;
using System.Web.Mvc;
using Grid.Features.EmailService;
using Grid.Features.TicketDesk.DAL.Interfaces;
using Grid.Features.TicketDesk.Entities.Enums;

namespace Grid.Areas.Scheduler.Controllers
{
    public class ExpiredTicketsController : Controller
    {
        private readonly ITicketRepository _ticketRepository;

        private readonly EmailComposerService _emailComposerService;

        public ExpiredTicketsController(ITicketRepository ticketRepository,
                                        EmailComposerService emailComposerService)
        {
            _ticketRepository = ticketRepository;
            _emailComposerService = emailComposerService;
        }

        public ActionResult Index()
        {
            var today = DateTime.UtcNow.Date;
            var expiredTickets = _ticketRepository.GetAllBy(t => t.Status != TicketStatus.Closed || t.Status == TicketStatus.Cancelled && t.DueDate < today);
            foreach (var expiredTicket in expiredTickets)
            {
                var ticketId = expiredTicket.Id;

                #if !DEBUG
                    _emailComposerService.TicketMissed(ticketId);
                #endif
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}