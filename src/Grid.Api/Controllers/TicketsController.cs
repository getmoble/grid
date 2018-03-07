using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Api.Models.TicketDesk;
using Grid.Features.Common;
using Grid.Features.TicketDesk.DAL.Interfaces;
using Grid.Features.TicketDesk.Entities;
using Grid.Features.TicketDesk.ViewModels;
using Newtonsoft.Json;

namespace Grid.Api.Controllers
{
    public class TicketsController : GridApiBaseController
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketActivityRepository _ticketActivityRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TicketsController(ITicketRepository ticketRepository,
                                 ITicketActivityRepository ticketActivityRepository,
                                 IUnitOfWork unitOfWork)
        {
            _ticketRepository = ticketRepository;
            _ticketActivityRepository = ticketActivityRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(TicketSearchViewModel vm)
        {
            var apiResult = TryExecute(() =>
            {
                Func<IQueryable<Ticket>, IQueryable<Ticket>> ticketFilter = q =>
                {
                    q = q.Include("AssignedToUser.Person").Include("CreatedByUser.Person");

                    if (vm.Mine.HasValue)
                    {
                        q = q.Where(r => r.CreatedByUserId == WebUser.Id);
                    }

                    q = q.OrderByDescending(t => t.DueDate);

                    return q;
                };

                var tickets = _ticketRepository.Search(ticketFilter).Select(t => new TicketModel(t)).ToList();
                return tickets;
            }, "Tickets fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() =>
            {
                var ticket = _ticketRepository.GetBy(t => t.Id == id, "TicketCategory,TicketSubCategory,CreatedByUser.Person");
                return new TicketModel(ticket);
            }, "Ticket fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(Ticket vm)
        {
            ApiResult<Ticket> apiResult;

            if (ModelState.IsValid)
            {
                if (vm.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var ticket = _ticketRepository.Get(vm.Id);
                        ticket.TicketCategoryId = vm.TicketCategoryId;
                        ticket.TicketSubCategoryId = vm.TicketSubCategoryId;
                        ticket.Title = vm.Title;
                        ticket.Description = vm.Description;
                        ticket.DueDate = vm.DueDate;
                        ticket.UpdatedByUserId = WebUser.Id;
                        _unitOfWork.Commit();
                        return ticket;
                    }, "Ticket updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        vm.CreatedByUserId = WebUser.Id;
                        _ticketRepository.Create(vm);
                        _unitOfWork.Commit();
                        return vm;
                    }, "Ticket created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Ticket>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _ticketRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Ticket deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public string GetAllActivitiesForTicket(int id)
        {
            var activities = _ticketActivityRepository.GetAllBy(r => r.TicketId == id, o => o.OrderByDescending(f => f.CreatedOn));
            var list = JsonConvert.SerializeObject(activities, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return list;
        }

        [HttpPost]
        public JsonResult AddNote(TicketActivityViewModel vm)
        {
            var selectedTicket = _ticketRepository.Get(vm.TicketId);
            if (selectedTicket != null)
            {
                // Add it as an Activity
                var newActivity = new TicketActivity
                {
                    Title = vm.Title,
                    Comment = vm.Comment,
                    TicketId = selectedTicket.Id,
                    CreatedByUserId = WebUser.Id
                };

                _ticketActivityRepository.Create(newActivity);
                _unitOfWork.Commit();
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult DeleteNote(int id)
        {
            _ticketActivityRepository.Delete(id);
            _unitOfWork.Commit();
            return Json(true);
        }
    }

}
