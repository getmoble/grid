using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Features.HRMS.Services.Interfaces;
using Grid.Features.Settings.Services.Interfaces;
using Grid.Providers.Email;

namespace Grid.Features.HRMS.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISettingsService _settingsService;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository,
                           ISettingsService settingsService,
                           IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _settingsService = settingsService;
            _unitOfWork = unitOfWork;
        }

        public EmailContext ComposeEmailContextForBirthdayReminder()
        {
            var emailContext = new EmailContext();

            var users = _userRepository.GetAllBy(x => x.EmployeeStatus != EmployeeStatus.Ex && x.Id != 1, "Person").ToList();
            var today = DateTime.Today;

            var hasBirthdayToday = users.Where(u => u.Person.DateOfBirth.HasValue && u.Person.DateOfBirth.Value.Month == today.Month && u.Person.DateOfBirth.Value.Day == today.Day).ToList();
            var hasWeddingAnniversaryToday = users.Where(u => u.Person.MarriageAnniversary.HasValue && u.Person.MarriageAnniversary.Value.Month == today.Month && u.Person.MarriageAnniversary.Value.Day == today.Day).ToList();
            var hasJoiningAnniversaryToday = users.Where(u => u.DateOfJoin.HasValue && u.DateOfJoin.Value.Month == today.Month && u.DateOfJoin.Value.Day == today.Day).ToList();

            if (hasBirthdayToday.Any() || hasWeddingAnniversaryToday.Any() || hasJoiningAnniversaryToday.Any())
            {
                var birthdayDetails = new StringBuilder();
                if (hasBirthdayToday.Any())
                {
                    birthdayDetails.AppendLine("<table width='1024px' bgcolor='#999999'>");
                    birthdayDetails.AppendLine("<tr bgcolor='#ffffff'><td>Type</td><td>Name</td><td>Date</td>");
                    foreach (var birthday in hasBirthdayToday)
                    {
                        birthdayDetails.AppendLine("<tr bgcolor='#ffffff'>");
                        birthdayDetails.AppendLine($"<td>{"Birthday"}</td><td>{birthday.Person.Name}</td><td>{birthday.Person.DateOfBirth.Value.ToShortDateString()}</td>");
                        birthdayDetails.AppendLine("</tr>");
                    }
                    birthdayDetails.AppendLine("</table>");
                }
                else
                {
                    birthdayDetails.AppendLine("No Birthdays for Today");
                }

                var weddingAnniversaryDetails = new StringBuilder();
                if (hasWeddingAnniversaryToday.Any())
                {
                    weddingAnniversaryDetails.AppendLine("<table width='1024px' bgcolor='#999999'>");
                    weddingAnniversaryDetails.AppendLine("<tr bgcolor='#ffffff'><td>Type</td><td>Name</td><td>Date</td>");
                    foreach (var weddingAnniversary in hasWeddingAnniversaryToday)
                    {
                        weddingAnniversaryDetails.AppendLine("<tr bgcolor='#ffffff'>");
                        weddingAnniversaryDetails.AppendLine($"<td>{"Wedding Anniversary"}</td><td>{weddingAnniversary.Person.Name}</td><td>{weddingAnniversary.Person.MarriageAnniversary.Value.ToShortDateString()}</td>");
                        weddingAnniversaryDetails.AppendLine("</tr>");
                    }
                    weddingAnniversaryDetails.AppendLine("</table>");
                }
                else
                {
                    weddingAnniversaryDetails.AppendLine("No Birthdays for Today");
                }

                var joiningAnniversaryDetails = new StringBuilder();
                if (hasJoiningAnniversaryToday.Any())
                {
                    joiningAnniversaryDetails.AppendLine("<table width='1024px' bgcolor='#999999'>");
                    joiningAnniversaryDetails.AppendLine("<tr bgcolor='#ffffff'><td>Type</td><td>Name</td><td>Date</td>");
                    foreach (var joiningAnniversary in hasJoiningAnniversaryToday)
                    {
                        joiningAnniversaryDetails.AppendLine("<tr bgcolor='#ffffff'>");
                        joiningAnniversaryDetails.AppendLine($"<td>{"Joining Anniversary"}</td><td>{joiningAnniversary.Person.Name}</td><td>{joiningAnniversary.DateOfJoin.Value.ToShortDateString()}</td>");
                        joiningAnniversaryDetails.AppendLine("</tr>");
                    }
                    joiningAnniversaryDetails.AppendLine("</table>");
                }
                else
                {
                    joiningAnniversaryDetails.AppendLine("No Joining Anniversary for Today");
                }

                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[Birthdays]", birthdayDetails.ToString()),
                    new PlaceHolder("[WeddingAnniversaries]", weddingAnniversaryDetails.ToString()),
                    new PlaceHolder("[JoiningAnniversaries]", joiningAnniversaryDetails.ToString())
                };

                emailContext.Subject = "Birthday and Anniversary Reminder";

                var settings = _settingsService.GetSiteSettings();
                if (settings.POCSettings != null)
                {
                    var selectedPOC = _userRepository.Get(settings.POCSettings.HRDepartmentLevel1, "Person");
                    if (selectedPOC != null)
                    {
                        var pocAddress = new MailAddress(selectedPOC.OfficialEmail, selectedPOC.Person.Name);
                        emailContext.ToAddress.Add(pocAddress);
                    }

                    var selectedPOC2 = _userRepository.Get(settings.POCSettings.HRDepartmentLevel2, "Person");
                    if (selectedPOC2 != null)
                    {
                        var pocAddress = new MailAddress(selectedPOC.OfficialEmail, selectedPOC.Person.Name);
                        emailContext.CcAddress.Add(pocAddress);
                    }
                }
            }
            else
            {
                emailContext.DropEmail = true;
            }

            return emailContext;
        }
    }
}
