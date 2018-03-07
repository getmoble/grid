using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.EmailService;
using Grid.Features.HRMS.DAL.Interfaces;
using Newtonsoft.Json;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Features.Settings.Services.Interfaces;
using Grid.Features.TicketDesk.DAL.Interfaces;
using Grid.Features.TicketDesk.Entities;
using Grid.Features.TicketDesk.Entities.Enums;
using Grid.Features.TicketDesk.Services.Interfaces;
using Grid.Features.TicketDesk.ViewModels;
using Grid.Filters;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.TicketDesk.Controllers
{

    [GridPermission(PermissionCode = 300)]
    public class TicketsController : TicketDeskBaseController
    {

        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly ITicketSubCategoryRepository _ticketSubCategoryRepository;
        private readonly ITicketActivityRepository _ticketActivityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISettingsService _settingsService;
        private readonly ITicketService _ticketService;
        private readonly EmailComposerService _emailComposerService;

        public TicketsController(ITicketRepository ticketRepository,
                                 ITicketCategoryRepository ticketCategoryRepository,
                                 ITicketSubCategoryRepository ticketSubCategoryRepository,
                                 ITicketActivityRepository ticketActivityRepository,
                                 IUserRepository userRepository,
                                 ITicketService ticketService,
                                 EmailComposerService emailComposerService,
                                 ISettingsService settingsService,
                                 IUnitOfWork unitOfWork)
        {
            _ticketRepository = ticketRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _ticketSubCategoryRepository = ticketSubCategoryRepository;
            _ticketActivityRepository = ticketActivityRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _ticketService = ticketService;
            _emailComposerService = emailComposerService;
            _settingsService = settingsService;
        }

        #region Ajax Calls
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

        #endregion

        [SelectList("TicketCategory", "TicketCategoryId")]
        [SelectList("TicketSubCategory", "TicketSubCategoryId")]
        [SelectList("User", "AssignedToUserId")]
        public ActionResult Index(TicketSearchViewModel vm)
        {
            Func<IQueryable<Ticket>, IQueryable<Ticket>> ticketFilter = q =>
            {
                q = q.Include("TicketCategory").Include("TicketSubCategory").Include("CreatedByUser.Person").Include("AssignedToUser.Person");

                if (vm.TicketCategoryId.HasValue)
                {
                    q = q.Where(r => r.TicketCategoryId == vm.TicketCategoryId.Value);
                }

                if (vm.TicketSubCategoryId.HasValue)
                {
                    q = q.Where(r => r.TicketSubCategoryId == vm.TicketSubCategoryId.Value);
                }

                if (vm.HideCompleted)
                {
                    q = q.Where(r => r.Status != TicketStatus.Closed && r.Status != TicketStatus.Cancelled);
                }

                if (vm.TicketStatus.HasValue)
                {
                    q = q.Where(r => r.Status == vm.TicketStatus.Value);
                }

                if (vm.AssignedToUserId.HasValue)
                {
                    q = q.Where(r => r.AssignedToUserId == vm.AssignedToUserId.Value);
                }

                if (!string.IsNullOrEmpty(vm.Title))
                {
                    q = q.Where(r => r.Title.Contains(vm.Title));
                }

                if (vm.StartDate.HasValue)
                {
                    q = q.Where(r => r.CreatedOn >= vm.StartDate);
                }

                if (vm.EndDate.HasValue)
                {
                    q = q.Where(r => r.CreatedOn <= vm.EndDate);
                }

                if (WebUser.IsAdmin || WebUser.Permissions.Contains(1100))
                {
                    // Do Nothing;
                }
                else
                {
                    q = q.Where(t => t.CreatedByUserId == WebUser.Id);
                }

                return q;
            };


            if (WebUser.IsAdmin || WebUser.Permissions.Contains(1100))
            {
                ViewBag.CanManage = true;
                vm.Tickets = _ticketRepository.SearchPage(ticketFilter, o => o.OrderByDescending(t => t.CreatedOn),
                    vm.GetPageNo(), vm.PageSize);
                return View(vm);
            }
            else
            {
                ViewBag.CanManage = false;
                vm.Tickets = _ticketRepository.SearchPage(ticketFilter, o => o.OrderByDescending(t => t.CreatedOn), vm.GetPageNo(), vm.PageSize);
                return View(vm);
            }
        }

        public ActionResult MyTickets(TicketSearchViewModel vm)
        {
            Func<IQueryable<Ticket>, IQueryable<Ticket>> ticketFilter = q =>
            {
                q = q.Include("TicketCategory").Include("TicketSubCategory").Include("CreatedByUser.Person").Include("AssignedToUser.Person");

                if (vm.HideCompleted)
                {
                    q = q.Where(r => r.Status != TicketStatus.Closed && r.Status != TicketStatus.Cancelled);
                }

                if (vm.TicketStatus.HasValue)
                {
                    q = q.Where(r => r.Status == vm.TicketStatus.Value);
                }

                if (!string.IsNullOrEmpty(vm.Title))
                {
                    q = q.Where(r => r.Title.Contains(vm.Title));
                }

                if (vm.StartDate.HasValue)
                {
                    q = q.Where(r => r.CreatedOn >= vm.StartDate);
                }

                if (vm.EndDate.HasValue)
                {
                    q = q.Where(r => r.CreatedOn <= vm.EndDate);
                }

                q = q.Where(t => t.CreatedByUserId == WebUser.Id);

                return q;
            };


            ViewBag.CanManage = false;
            vm.Tickets = _ticketRepository.SearchPage(ticketFilter, o => o.OrderByDescending(t => t.CreatedOn), vm.GetPageNo(), vm.PageSize);
            return View(vm);
        }

        public ActionResult Details(int id)
        {
            var ticket = _ticketRepository.GetBy(t => t.Id == id, "TicketCategory,TicketSubCategory,CreatedByUser.Person");
            var vm = new TicketDetailsViewModel(ticket);
            return View(vm);
        }

        public ActionResult Create()
        {
            // Special Case where the sub categories should be based on the category
            var categories = _ticketCategoryRepository.GetAll().ToList();
            var firstCategory = categories.First();
            var subCategories = _ticketSubCategoryRepository.GetAllBy(s => s.TicketCategoryId == firstCategory.Id);

            ViewBag.TicketCategoryId = new SelectList(categories, "Id", "Title");
            ViewBag.TicketSubCategoryId = new SelectList(subCategories, "Id", "Title");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateTicketViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newTicket = new Ticket
                {
                    TicketCategoryId = vm.TicketCategoryId,
                    TicketSubCategoryId = vm.TicketSubCategoryId,
                    Title = vm.Title,
                    Description = vm.Description,
                    DueDate = vm.DueDate,
                    Status = vm.Status,
                    AssignedToUserId = vm.AssignedToUserId,
                    CreatedByUserId = WebUser.Id
                };

                // Assign to Default point of contact.
                var selectedCategory = _ticketCategoryRepository.Get(vm.TicketCategoryId);
                var pointOfContact = _ticketService.GetPointOfContact(selectedCategory.Title);
                newTicket.AssignedToUserId = pointOfContact;

                _ticketRepository.Create(newTicket);
                _unitOfWork.Commit();

                #if !DEBUG
                    _emailComposerService.TicketCreated(newTicket.Id);
                #endif

                return RedirectToAction("Index");
            }

            // Special Case where the sub categories should be based on the category
            var categories = _ticketCategoryRepository.GetAll().ToList();
            var subCategories = _ticketSubCategoryRepository.GetAllBy(s => s.TicketCategoryId == vm.TicketCategoryId);

            ViewBag.TicketCategoryId = new SelectList(categories, "Id", "Title", vm.TicketCategoryId);
            ViewBag.TicketSubCategoryId = new SelectList(subCategories, "Id", "Title", vm.TicketSubCategoryId);
            return View();
        }

        public ActionResult Edit(int id)
        {
            var ticket = _ticketRepository.Get(id);

            var vm = new EditTicketViewModel(ticket);

            ViewBag.TicketCategoryId = new SelectList(_ticketCategoryRepository.GetAll(), "Id", "Title", ticket.TicketCategoryId);
            ViewBag.TicketSubCategoryId = new SelectList(_ticketSubCategoryRepository.GetAll(), "Id", "Title", ticket.TicketSubCategoryId);
            ViewBag.AssignedToUserId = new SelectList(_userRepository.GetAllBy(u => u.Id != 1 && u.EmployeeStatus != EmployeeStatus.Ex, "Person"), "Id", "Person.Name", ticket.AssignedToUserId);

            return CheckForNullAndExecute(ticket, () => View(vm));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditTicketViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var selectedTicket = _ticketRepository.Get(vm.Id);
                if (selectedTicket != null)
                {
                    selectedTicket.TicketCategoryId = vm.TicketCategoryId;
                    selectedTicket.TicketSubCategoryId = vm.TicketSubCategoryId;
                    selectedTicket.Title = vm.Title;
                    selectedTicket.Description = vm.Description;
                    selectedTicket.DueDate = vm.DueDate;
                    selectedTicket.UpdatedByUserId = WebUser.Id;
                    
                    _ticketRepository.Update(selectedTicket);
                    _unitOfWork.Commit();

                    return RedirectToAction("Index");
                }
            }

            ViewBag.TicketCategoryId = new SelectList(_ticketCategoryRepository.GetAll(), "Id", "Title", vm.TicketCategoryId);
            ViewBag.TicketSubCategoryId = new SelectList(_ticketSubCategoryRepository.GetAll(), "Id", "Title", vm.TicketSubCategoryId);

            return View(vm);
        }

        public ActionResult ChangeStatus(int id)
        {
            var ticket = _ticketRepository.Get(id);

            var vm = new ChangeTicketStatusViewModel
            {
                Status = ticket.Status,
                AssignedToUserId = ticket.AssignedToUserId,
                DueDate = ticket.DueDate
            };

            ViewBag.TicketCategoryId = new SelectList(_ticketCategoryRepository.GetAll(), "Id", "Title", ticket.TicketCategoryId);
            ViewBag.TicketSubCategoryId = new SelectList(_ticketSubCategoryRepository.GetAll(), "Id", "Title", ticket.TicketSubCategoryId);
            ViewBag.AssignedToUserId = new SelectList(_userRepository.GetAllBy(u => u.Id != 1 && u.EmployeeStatus != EmployeeStatus.Ex, "Person"), "Id", "Person.Name", WebUser.Id);

            return CheckForNullAndExecute(ticket, () => View(vm));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeStatus(ChangeTicketStatusViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var selectedTicket = _ticketRepository.Get(vm.Id);
                if (selectedTicket != null)
                {
                    selectedTicket.Status = vm.Status;
                    selectedTicket.AssignedToUserId = vm.AssignedToUserId;
                    selectedTicket.DueDate = vm.DueDate;

                    selectedTicket.UpdatedByUserId = WebUser.Id;

                    _ticketRepository.Update(selectedTicket);
                    _unitOfWork.Commit();


                    // Add it as an Activity
                    var newActivity = new TicketActivity
                    {
                        Title = Enum.GetName(vm.Status.GetType(), vm.Status),
                        Comment = vm.Comments,
                        TicketId = selectedTicket.Id,
                        CreatedByUserId = WebUser.Id
                    };

                    _ticketActivityRepository.Create(newActivity);
                    _unitOfWork.Commit();

                    #if !DEBUG
                        _emailComposerService.TicketUpdated(selectedTicket.Id);
                    #endif

                    return RedirectToAction("Index");
                }
            }

            ViewBag.AssignedToUserId = new SelectList(_userRepository.GetAllBy(u => u.Id != 1 && u.EmployeeStatus != EmployeeStatus.Ex, "Person"), "Id", "Person.Name");

            return View(vm);
        }

        public ActionResult Delete(int id)
        {
            var ticket = _ticketRepository.Get(id);
            return CheckForNullAndExecute(ticket, () => View(ticket));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var operationResult = _ticketService.Delete(id);
            if (operationResult.Status)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, operationResult.Message);
            return View();
        }
    }
}

