using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Features.HRMS.Entities;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Features.HRMS.ViewModels;
using Grid.Filters;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.HRMS.Controllers
{
    [GridPermission(PermissionCode = 300)]
    public class EmployeeDirectoryController : GridBaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserSkillRepository _userSkillRepository;
        private readonly IUserHobbyRepository _userHobbyRepository;
        private readonly IUserCertificationRepository _userCertificationRepository;
        private readonly IUserTechnologyMapRepository _userTechnologyMapRepository;
        private readonly IUserAwardRepository _userAwardRepository;

        public EmployeeDirectoryController(IUserRepository userRepository,
                                           IUserSkillRepository userSkillRepository,
                                           IUserHobbyRepository userHobbyRepository,
                                           IUserCertificationRepository userCertificationRepository,
                                           IUserTechnologyMapRepository userTechnologyMapRepository,
                                           IUserAwardRepository userAwardRepository)
        {
            _userRepository = userRepository;
            _userSkillRepository = userSkillRepository;
            _userHobbyRepository = userHobbyRepository;
            _userCertificationRepository = userCertificationRepository;
            _userTechnologyMapRepository = userTechnologyMapRepository;
            _userAwardRepository = userAwardRepository;
        }

        [HttpGet]
        [SelectList("Department", "DepartmentId")]
        [SelectList("Designation", "DesignationId")]
        [SelectList("Location", "LocationId")]
        [SelectList("Shift", "ShiftId")]
        public ActionResult Directory(UserSearchViewModel vm)
        {
            vm.Total = _userRepository.Count();

            Func<IQueryable<User>, IQueryable<User>> userFilter = q =>
            {
                q = q.Include(u => u.AccessRule)
                        .Include(u => u.Department)
                        .Include(u => u.Designation)
                        .Include(u => u.Location)
                        .Include(u => u.Person)
                        .Include(u => u.ReportingPerson)
                        .Include(u => u.Shift);

                if (!vm.ShowExEmployees)
                {
                    q = q.Where(r => r.EmployeeStatus != EmployeeStatus.Ex);
                }

                if (vm.DepartmentId.HasValue)
                {
                    q = q.Where(r => r.DepartmentId == vm.DepartmentId.Value);
                }

                if (vm.LocationId.HasValue)
                {
                    q = q.Where(r => r.LocationId == vm.LocationId.Value);
                }

                if (vm.DesignationId.HasValue)
                {
                    q = q.Where(r => r.DesignationId == vm.DesignationId.Value);
                }

                if (vm.ShiftId.HasValue)
                {
                    q = q.Where(r => r.ShiftId == vm.ShiftId.Value);
                }

                if (!string.IsNullOrEmpty(vm.Code))
                {
                    q = q.Where(r => r.EmployeeCode.Contains(vm.Code));
                }

                if (vm.Status.HasValue)
                {
                    q = q.Where(r => r.EmployeeStatus == vm.Status.Value);
                }

                // Skip admin User
                q = q.Where(e => e.Id != 1);

                return q;
            };

            // Need to sort by name
            vm.Users = _userRepository.SearchPage(userFilter, o => o.OrderByDescending(u => u.DateOfJoin), vm.GetPageNo(), vm.PageSize);

            return vm.Mode == ViewMode.List ? View(vm) : View("Directory_Grid", vm);

            // Else return Grid View
        }

        [HttpGet]
        public ActionResult UserProfile(int id)
        {
            var user = _userRepository.Get(id, "Person,AccessRule,ReportingPerson.Person,Location,Department,Designation,Shift");

            if (user == null)
            {
                return HttpNotFound();
            }

            var userSkills = _userSkillRepository.GetAllBy(m => m.UserId == user.Id, "Skill");
            var userHobbies = _userHobbyRepository.GetAllBy(m => m.UserId == user.Id, "Hobby");
            var userCertifications = _userCertificationRepository.GetAllBy(m => m.UserId == user.Id, "Certification");
            var technologies = _userTechnologyMapRepository.GetAllBy(r => r.UserId == user.Id, "Technology").Select(t => t.Technology).ToList();
            var awards = _userAwardRepository.GetAllBy(u => u.UserId == user.Id, "Award").ToList();

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                EmployeeCode = user.EmployeeCode,
                AccessRuleId = user.AccessRuleId,
                AccessRule = user.AccessRule,
                PersonId = user.PersonId,
                Person = user.Person,
                DepartmentId = user.DepartmentId,
                Department = user.Department,
                LocationId = user.LocationId,
                Location = user.Location,
                DesignationId = user.DesignationId,
                Designation = user.Designation,
                ShiftId = user.ShiftId,
                Shift = user.Shift,
                ReportingPersonId = user.ReportingPersonId,
                ReportingPerson = user.ReportingPerson,
                DateOfJoin = user.DateOfJoin,
                OfficialEmail = user.OfficialEmail,
                UserSkills = userSkills.ToList(),
                UserCertifications = userCertifications.ToList(),
                Technologies = technologies,
                UserHobbies = userHobbies.ToList(),
                UserAwards = awards
            };

            return View(userViewModel);
        }
    }
}