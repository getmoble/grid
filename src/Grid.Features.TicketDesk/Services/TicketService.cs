using System.Collections.Generic;
using System.Net.Mail;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.Settings.Services.Interfaces;
using Grid.Features.TicketDesk.DAL.Interfaces;
using Grid.Features.TicketDesk.Entities.Enums;
using Grid.Features.TicketDesk.Services.Interfaces;
using Grid.Providers.Email;

namespace Grid.Features.TicketDesk.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketActivityRepository _ticketActivityRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISettingsService _settingsService;
        private readonly IUserRepository _userRepository;

        public TicketService(ITicketRepository ticketRepository,
                             ITicketActivityRepository ticketActivityRepository,
                             IUnitOfWork unitOfWork,
                             ISettingsService settingsService,
                             IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _ticketActivityRepository = ticketActivityRepository;
            _unitOfWork = unitOfWork;
            _settingsService = settingsService;
            _userRepository = userRepository;
        }

        public OperationResult<bool> Delete(int id)
        {
            var ticket = _ticketRepository.Get(id);
            if (ticket.Status != TicketStatus.Open)
            {
                return new OperationResult<bool> { Status = false, Message = "We can delete a ticket only when it is open state" };
            }

            var activities = _ticketActivityRepository.Any(a => a.TicketId == id);
            if (activities)
            {
                return new OperationResult<bool> { Status = false, Message = "We cannot delete a ticket once we have added notes against it" };
            }

            _ticketRepository.Delete(ticket);
            _unitOfWork.Commit();
            return new OperationResult<bool> { Status = true };
        }

        public int? GetPointOfContact(string category)
        {
            int? pointOfContact = null;
            var settings = _settingsService.GetSiteSettings();
            if (settings.POCSettings != null)
            {
                switch (category)
                {
                    case "IT":
                        pointOfContact = settings.POCSettings.ITDepartmentLevel1;
                        break;
                    case "HR":
                        pointOfContact = settings.POCSettings.HRDepartmentLevel1;
                        break;
                }
            }

            return pointOfContact;
        }

        public EmailContext ComposeEmailContextForTicketCreated(int ticketId)
        {
            var emailContext = new EmailContext();

            var selectedTicket = _ticketRepository.Get(ticketId, "TicketCategory,TicketSubCategory,CreatedByUser.Person");
            if (selectedTicket != null)
            {
                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[Category]", selectedTicket.TicketCategory.Title),
                    new PlaceHolder("[SubCategory]", selectedTicket.TicketSubCategory.Title),
                    new PlaceHolder("[Title]", selectedTicket.Title),
                    new PlaceHolder("[Description]", selectedTicket.Description),
                    new PlaceHolder("[CreatedBy]", selectedTicket.CreatedByUser.Person.Name),
                    new PlaceHolder("[Id]", selectedTicket.Id.ToString())
                };

                emailContext.Subject = "Ticket Created";

                emailContext.ToAddress.Add(new MailAddress(selectedTicket.CreatedByUser.OfficialEmail, selectedTicket.CreatedByUser.Person.Name));

                var settings = _settingsService.GetSiteSettings();
                if (settings.POCSettings != null)
                {
                    int? pointOfContact = null;
                    switch (selectedTicket.TicketCategory.Title)
                    {
                        case "IT":
                            pointOfContact = settings.POCSettings.ITDepartmentLevel1;
                            break;
                        case "HR":
                            pointOfContact = settings.POCSettings.HRDepartmentLevel1;
                            break;
                    }

                    if (pointOfContact.HasValue)
                    {
                        var selectedPOC = _userRepository.Get(pointOfContact.Value, "Person");
                        if (selectedPOC != null)
                        {
                            var pocAddress = new MailAddress(selectedPOC.OfficialEmail, selectedPOC.Person.Name);
                            emailContext.CcAddress.Add(pocAddress);
                        }
                    }
                }
            }

            return emailContext;
        }

        public EmailContext ComposeEmailContextForTicketUpdated(int ticketId)
        {
            var emailContext = new EmailContext();

            var selectedTicket = _ticketRepository.Get(ticketId, "TicketCategory,TicketSubCategory,CreatedByUser.Person,AssignedToUser.Person");
            if (selectedTicket != null)
            {
                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[Category]", selectedTicket.TicketCategory.Title),
                    new PlaceHolder("[SubCategory]", selectedTicket.TicketSubCategory.Title),
                    new PlaceHolder("[Title]", selectedTicket.Title),
                    new PlaceHolder("[Description]", selectedTicket.Description),
                    new PlaceHolder("[CreatedBy]", selectedTicket.CreatedByUser.Person.Name),
                    new PlaceHolder("[Id]", selectedTicket.Id.ToString()),
                    new PlaceHolder("[AssignedTo]", selectedTicket.AssignedToUser.Person.Name),
                    new PlaceHolder("[Status]", selectedTicket.Status.ToString())
                };

                emailContext.Subject = "Ticket Status Updated";

                emailContext.ToAddress.Add(new MailAddress(selectedTicket.CreatedByUser.OfficialEmail, selectedTicket.CreatedByUser.Person.Name));

                var settings = _settingsService.GetSiteSettings();
                if (settings.POCSettings != null)
                {
                    int? pointOfContact = null;
                    switch (selectedTicket.TicketCategory.Title)
                    {
                        case "IT":
                            pointOfContact = settings.POCSettings.ITDepartmentLevel1;
                            break;
                        case "HR":
                            pointOfContact = settings.POCSettings.HRDepartmentLevel1;
                            break;
                    }

                    if (pointOfContact.HasValue)
                    {
                        var selectedPOC = _userRepository.Get(pointOfContact.Value, "Person");
                        if (selectedPOC != null)
                        {
                            var pocAddress = new MailAddress(selectedPOC.OfficialEmail, selectedPOC.Person.Name);
                            emailContext.CcAddress.Add(pocAddress);
                        }
                    }
                }
            }

            return emailContext;
        }

        public EmailContext ComposeEmailContextForTicketMissed(int ticketId)
        {
            var emailContext = new EmailContext();

            var selectedTicket = _ticketRepository.Get(ticketId, "TicketCategory,TicketSubCategory,CreatedByUser.Person,AssignedToUser.Person");
            if (selectedTicket != null)
            {
                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[Category]", selectedTicket.TicketCategory.Title),
                    new PlaceHolder("[SubCategory]", selectedTicket.TicketSubCategory.Title),
                    new PlaceHolder("[Title]", selectedTicket.Title),
                    new PlaceHolder("[Description]", selectedTicket.Description),
                    new PlaceHolder("[CreatedBy]", selectedTicket.CreatedByUser.Person.Name),
                    new PlaceHolder("[Id]", selectedTicket.Id.ToString()),
                    new PlaceHolder("[Status]", selectedTicket.Status.ToString())
                };

                // We may not assign it always
                if (selectedTicket.AssignedToUser != null)
                {
                    emailContext.PlaceHolders.Add(new PlaceHolder("[AssignedTo]", selectedTicket.AssignedToUser.Person.Name));
                }

                emailContext.Subject = "Ticket Due Date Missed";

                emailContext.ToAddress.Add(new MailAddress(selectedTicket.CreatedByUser.OfficialEmail, selectedTicket.CreatedByUser.Person.Name));

                var settings = _settingsService.GetSiteSettings();
                if (settings.POCSettings != null)
                {
                    int? pointOfContact = null;
                    int? pointOfContactLevel2 = null;
                    switch (selectedTicket.TicketCategory.Title)
                    {

                        case "IT":
                            pointOfContact = settings.POCSettings.ITDepartmentLevel1;
                            pointOfContactLevel2 = settings.POCSettings.ITDepartmentLevel2;
                            break;
                        case "HR":
                            pointOfContact = settings.POCSettings.HRDepartmentLevel1;
                            pointOfContactLevel2 = settings.POCSettings.HRDepartmentLevel2;
                            break;
                    }

                    if (pointOfContact.HasValue)
                    {
                        var selectedPOC = _userRepository.Get(pointOfContact.Value, "Person");
                        if (selectedPOC != null)
                        {
                            var pocAddress = new MailAddress(selectedPOC.OfficialEmail, selectedPOC.Person.Name);
                            emailContext.CcAddress.Add(pocAddress);
                        }
                    }

                    if (pointOfContactLevel2.HasValue)
                    {
                        var selectedPOCLevel2 = _userRepository.Get(pointOfContactLevel2.Value, "Person");
                        if (selectedPOCLevel2 != null)
                        {
                            var pocAddress = new MailAddress(selectedPOCLevel2.OfficialEmail, selectedPOCLevel2.Person.Name);
                            emailContext.CcAddress.Add(pocAddress);
                        }
                    }
                }
            }

            return emailContext;
        }
    }
}
