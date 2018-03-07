using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Grid.Clients.ITSync.Models;
using Grid.Features.Auth.DAL.Interfaces;
using Grid.Features.Common;
using Newtonsoft.Json;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Providers.Blob;
using Grid.Features.HRMS.Entities;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Features.HRMS.ViewModels;
using Grid.Features.HRMS.ViewModels.User;
using Grid.Features.IMS.DAL.Interfaces;
using Grid.Features.IT.DAL.Interfaces;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.Settings.Services.Interfaces;
using Grid.Filters;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.HRMS.Controllers
{
    [GridPermission(PermissionCode = 210)]
    public class UsersController : GridBaseController
    {
        private readonly IAccessRuleRepository _accessRuleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserSkillRepository _userSkillRepository;
        private readonly IUserCertificationRepository _userCertificationRepository;

        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDesignationRepository _designationRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IShiftRepository _shiftRepository;

        private readonly IRoleRepository _roleRepository;
        private readonly IRoleMemberRepository _roleMemberRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly ILeaveEntitlementRepository _leaveEntitlementRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IUserTechnologyMapRepository _userTechnologyMapRepository;
        private readonly IEmergencyContactRepository _emergencyContactRepository;
        private readonly IEmployeeDependentRepository _employeeDependentRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IAwardRepository _awardRepository;
        private readonly ISystemSnapshotRepository _systemSnapshotRepository;
        private readonly IUserAwardRepository _userAwardRepository;
        private readonly IUserHobbyRepository _userHobbyRepository;


        private readonly IUserDocumentRepository _userDocumentRepository;

        private readonly ITechnologyRepository _technologyRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IHobbyRepository _hobbyRepository;
        private readonly ICertificationRepository _certificationRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly ISettingsService _settingsService;
        public UsersController(ISettingsService settingsService,
                                IAccessRuleRepository accessRuleRepository,
                               IUserRepository userRepository,
                               IUserSkillRepository userSkillRepository,
                               IUserCertificationRepository userCertificationRepository,
                               IDepartmentRepository departmentRepository,
                               IDesignationRepository designationRepository,
                               ILocationRepository locationRepository,
                               IShiftRepository shiftRepository,
                               IRoleRepository roleRepository,
                               IRoleMemberRepository roleMemberRepository,
                                 IAssetRepository assetRepository,
                                 ILeaveEntitlementRepository leaveEntitlementRepository,
                                 IProjectMemberRepository projectMemberRepository,
                                 IUserTechnologyMapRepository userTechnologyMapRepository,
                                 IEmergencyContactRepository emergencyContactRepository,
                                 IEmployeeDependentRepository employeeDependentRepository,
                                 IUserAwardRepository userAwardRepository,
                                 ITokenRepository tokenRepository,
                                 IAwardRepository awardRepository,
                                 ITechnologyRepository technologyRepository,
                                 ISkillRepository skillRepository,
                                 IHobbyRepository hobbyRepository,
                                 IUserHobbyRepository userHobbyRepository,
                                 ICertificationRepository certificationRepository,
                                 ISystemSnapshotRepository systemSnapshotRepository,

                                 IUserDocumentRepository userDocumentRepository,
                               IUnitOfWork unitOfWork)
        {
            _settingsService = settingsService;
            _accessRuleRepository = accessRuleRepository;
            _userRepository = userRepository;
            _userSkillRepository = userSkillRepository;
            _userCertificationRepository = userCertificationRepository;
            _departmentRepository = departmentRepository;
            _designationRepository = designationRepository;
            _locationRepository = locationRepository;
            _shiftRepository = shiftRepository;

            _roleRepository = roleRepository;
            _roleMemberRepository = roleMemberRepository;
            _leaveEntitlementRepository = leaveEntitlementRepository;
            _assetRepository = assetRepository;
            _projectMemberRepository = projectMemberRepository;
            _userTechnologyMapRepository = userTechnologyMapRepository;
            _emergencyContactRepository = emergencyContactRepository;
            _employeeDependentRepository = employeeDependentRepository;
            _tokenRepository = tokenRepository;
            _awardRepository = awardRepository;
            _systemSnapshotRepository = systemSnapshotRepository;
            _userAwardRepository = userAwardRepository;

            _userHobbyRepository = userHobbyRepository;

            _technologyRepository = technologyRepository;
            _skillRepository = skillRepository;
            _hobbyRepository = hobbyRepository;
            _certificationRepository = certificationRepository;


            _userDocumentRepository = userDocumentRepository;

            _unitOfWork = unitOfWork;
        }

        #region Ajax Calls
        [HttpPost]
        public JsonResult AddSkill(AddSkillViewModel vm)
        {
            var selectedUser = _userRepository.Get(vm.UserId);
            if (selectedUser != null)
            {
                var userSkill = new UserSkill
                {
                    UserId = selectedUser.Id,
                    SkillId = vm.SkillId
                };

                _userSkillRepository.Create(userSkill);
                _unitOfWork.Commit();

                return Json(true);
            }
            
            return Json(false);
        }

        public JsonResult AddCertification(AddCertificationViewModel vm)
        {
            var selectedUser = _userRepository.GetBy(r => r.Id == vm.UserId);
            if (selectedUser != null)
            {
                var userCertification = new UserCertification
                {
                    UserId = selectedUser.Id,
                    CertificationId = vm.CertificationId
                };

                _userCertificationRepository.Create(userCertification);
                _unitOfWork.Commit();

                return Json(true);
            }
            
            return Json(false);
        }
        #endregion

        public ActionResult Chart()
        {
            var users = _userRepository.GetAllBy(u => u.Id != 1 && u.EmployeeStatus != EmployeeStatus.Ex, "Designation,Person,ReportingPerson.Person").ToList();
            return View(users.ToList());
        }

        [SelectList("Department", "DepartmentId")]
        [SelectList("Designation", "DesignationId")]
        [SelectList("Location", "LocationId")]
        [SelectList("Shift", "ShiftId")]
        public ActionResult Index(UserSearchViewModel vm)
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

            vm.Users = _userRepository.SearchPage(userFilter, o => o.OrderByDescending(u => u.DateOfJoin), vm.GetPageNo(), vm.PageSize);
            return View(vm);
        }

        [HttpGet]
        public ActionResult ResetPassword(int id)
        {
            var selectedUser = _userRepository.Get(id, "Person");
            if (selectedUser != null)
            {
                var vm = new ResetPasswordViewModel
                {
                    UserId = id,
                    Name = selectedUser.Person.Name
                };

                return View(vm);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _userRepository.GetBy(u => u.Id == vm.UserId, "AccessRule,Person");
                    if (user != null)
                    {
                        user.Password = HashHelper.Hash(vm.NewPassword);

                        _userRepository.Update(user);
                        _unitOfWork.Commit();

                        user.AccessRule.LastPasswordResetDate = DateTime.UtcNow;
                        _accessRuleRepository.Update(user.AccessRule);
                        _unitOfWork.Commit();
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View(vm);
                }

                // Send Email once password is reset by admin

            }
            
            return View(vm);
        }

        [SelectList("LeaveType", "LeaveTypeId")]
        [SelectList("LeavePeriod", "LeaveTimePeriodId")]
        [SelectList("Skill", "SkillId")]
        [SelectList("Certification", "CertificationId")]
        public ActionResult Details(int id)
        {
            var user = _userRepository.Get(id, "Person,AccessRule,ReportingPerson.Person, Manager.Person, Location,Department,Designation,Shift");

            if (user == null)
            {
                return HttpNotFound();
            }
            
            var userDocs = _userDocumentRepository.GetAllBy(m => m.UserId == user.Id);
            var userSkills = _userSkillRepository.GetAllBy(m => m.UserId == user.Id, "Skill");
            var userHobbies = _userHobbyRepository.GetAllBy(m => m.UserId == user.Id, "Hobby");
            var userCertifications = _userCertificationRepository.GetAllBy(m => m.UserId == user.Id, "Certification");
            var roleMembers = _roleMemberRepository.GetAllBy(m => m.UserId == user.Id, "Role");
            var assets = _assetRepository.GetAllBy(u => u.AllocatedEmployeeId == user.Id, "AssetCategory").ToList();
            //var leaveEntitlements = _leaveEntitlementRepository.GetAllBy(u => u.AllocatedUserId == user.Id, "LeaveType").ToList();
            var projects = _projectMemberRepository.GetAllBy(p => p.EmployeeId == user.Id, "Project,Member").ToList();
            var technologies = _userTechnologyMapRepository.GetAllBy(r => r.UserId == user.Id, "Technology").Select(t => t.Technology).ToList();
            //var emergencyContacts = _emergencyContactRepository.GetAllBy(c => c.UserId == user.Id);
            //var dependants = _employeeDependentRepository.GetAllBy(d => d.UserId == user.Id);
            var reportees = _userRepository.GetAllBy(u => u.EmployeeStatus != EmployeeStatus.Ex && u.ReportingPersonId == user.Id, "Person,Designation,AccessRule").ToList();
            var tokens = _tokenRepository.GetAllBy(t => t.AllocatedToUserId == user.Id).ToList();
            var awards = _userAwardRepository.GetAllBy(u => u.UserId == user.Id, "Award").ToList();

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                EmployeeCode = user.EmployeeCode,
                Username = user.Username,
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
                ManagerId = user.ManagerId,
                Manager = user.Manager,
                Experience = user.Experience,
                DateOfJoin = user.DateOfJoin,
                ConfirmationDate = user.ConfirmationDate,
                LastDate = user.LastDate,
                DateOfResignation = user.DateOfResignation,
                OfficialEmail = user.OfficialEmail,
                EmployeeStatus = user.EmployeeStatus,
                RequiresTimeSheet = user.RequiresTimeSheet,
                Salary = user.Salary,
                Bank = user.Bank,
                BankAccountNumber = user.BankAccountNumber,
                PANCard = user.PANCard,
                PaymentMode = user.PaymentMode,
                UserDocuments = userDocs.ToList(),
                UserSkills = userSkills.ToList(),
                UserCertifications = userCertifications.ToList(),
                RoleMembers = roleMembers.ToList(),
                //Assets = assets.ToList(),
                //LeaveEntitlements = leaveEntitlements.ToList(),
                //Projects = projects.ToList(),
                Technologies = technologies,
                //EmergencyContacts = emergencyContacts.ToList(),
                //EmployeeDependents = dependants.ToList(),
                Reportees = reportees,
                //Tokens = tokens,
                UserHobbies = userHobbies.ToList(),
                UserAwards = awards
            };

            // If we have snapshots, set snapshot variable
            var snapshot = _systemSnapshotRepository.GetBy(s => s.UserId == user.Id);
            if (snapshot != null)
            {
                userViewModel.HasSoftwareInfo = true;
                userViewModel.Softwares = JsonConvert.DeserializeObject<List<SoftwareModel>>(snapshot.Softwares);

                userViewModel.HasHardwareInfo = true;
                userViewModel.Hardware = JsonConvert.DeserializeObject<HardwareModel>(snapshot.Hardwares);
            }

            return View(userViewModel);
        }

        [MultiSelectList("Role", "Roles")]
        [MultiSelectList("Technology", "Technologies")]
        [SelectList("User", "ReportingPersonId")]
        [SelectList("User", "ManagerId")]
        [SelectList("Department", "DepartmentId")]
        [SelectList("Designation", "DesignationId")]
        [SelectList("Location", "LocationId")]
        [SelectList("Shift", "ShiftId")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewUserViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newUser = new User
                {
                    EmployeeCode = vm.EmployeeCode,
                    Username = vm.Username,
                    Password = HashHelper.Hash(vm.Password),
                    AccessRule = AccessRule.CreateNewUserAccessRule(true),
                    Person = vm.Person,
                    DepartmentId = vm.DepartmentId,
                    LocationId = vm.LocationId,
                    DesignationId = vm.DesignationId,
                    ShiftId = vm.ShiftId,
                    ReportingPersonId = vm.ReportingPersonId,
                    ManagerId = vm.ManagerId,
                    Experience = vm.Experience,
                    DateOfJoin = vm.DateOfJoin,
                    ConfirmationDate = vm.ConfirmationDate,
                    DateOfResignation = vm.DateOfResignation,
                    LastDate = vm.LastDate,
                    OfficialEmail = vm.OfficialEmail,
                    OfficialPhone = vm.OfficialPhone,
                    OfficialMessengerId = vm.OfficialMessengerId,
                    EmployeeStatus = vm.EmployeeStatus,
                    RequiresTimeSheet = vm.RequiresTimeSheet,
                    Salary = vm.Salary,
                    Bank = vm.Bank,
                    BankAccountNumber = vm.BankAccountNumber,
                    PANCard = vm.PANCard,
                    PaymentMode = vm.PaymentMode
                };

                _userRepository.Create(newUser);
                _unitOfWork.Commit();

                // Map the Technologies
                if (vm.TechnologyIds != null)
                {
                    foreach (var technologyId in vm.TechnologyIds)
                    {
                        var newMap = new UserTechnologyMap
                        {
                            UserId = newUser.Id,
                            TechnologyId = technologyId
                        };
                        _userTechnologyMapRepository.Create(newMap);
                    }

                    _unitOfWork.Commit();
                }

                return RedirectToAction("Index");
            }

            ViewBag.Roles = new MultiSelectList(_roleRepository.GetAll(), "Id", "Name", vm.RoleIds);
            ViewBag.Technologies = new MultiSelectList(_technologyRepository.GetAll(), "Id", "Title", vm.TechnologyIds);
            ViewBag.DepartmentId = new SelectList(_departmentRepository.GetAll(), "Id", "Title", vm.DepartmentId);
            ViewBag.DesignationId = new SelectList(_designationRepository.GetAll(), "Id", "Title", vm.DesignationId);
            ViewBag.LocationId = new SelectList(_locationRepository.GetAll(), "Id", "Title", vm.LocationId);
            ViewBag.ReportingPersonId = new SelectList(_userRepository.GetAll("Person"), "Id", "Person.Name", vm.ReportingPersonId);
            ViewBag.ManagerId = new SelectList(_userRepository.GetAll("Person"), "Id", "Person.Name", vm.ManagerId);
            ViewBag.ShiftId = new SelectList(_shiftRepository.GetAll(), "Id", "Title", vm.ShiftId);
            
            return View(vm);
        }

        public ActionResult Edit(int id)
        {
            var user = _userRepository.Get(id, "Person");

            if (user == null)
            {
                return HttpNotFound();
            }

            var mappedRoles = _roleMemberRepository.GetAllBy(m => m.UserId == user.Id)
                                       .Select(m => m.RoleId)
                                       .ToList();

            var mappedTechnologies = _userTechnologyMapRepository.GetAllBy(m => m.UserId == user.Id)
                                       .Select(m => m.TechnologyId)
                                       .ToList();

            var mappedSkills = _userSkillRepository.GetAllBy(m => m.UserId == user.Id)
                                       .Select(m => m.SkillId)
                                       .ToList();

            var mappedHobbies = _userHobbyRepository.GetAllBy(m => m.UserId == user.Id)
                                       .Select(m => m.HobbyId)
                                       .ToList();

            var mappedCertifications = _userCertificationRepository.GetAllBy(m => m.UserId == user.Id)
                                       .Select(m => m.CertificationId)
                                       .ToList();

            ViewBag.Roles = new MultiSelectList(_roleRepository.GetAll(), "Id", "Name", mappedRoles);
            ViewBag.Technologies = new MultiSelectList(_technologyRepository.GetAll(), "Id", "Title", mappedTechnologies);
            ViewBag.Skills = new MultiSelectList(_skillRepository.GetAll(), "Id", "Title", mappedSkills);
            ViewBag.Hobbies = new MultiSelectList(_hobbyRepository.GetAll(), "Id", "Title", mappedHobbies);
            ViewBag.Certifications = new MultiSelectList(_certificationRepository.GetAll(), "Id", "Title", mappedCertifications);
            ViewBag.DepartmentId = new SelectList(_departmentRepository.GetAll(), "Id", "Title", user.DepartmentId);
            ViewBag.DesignationId = new SelectList(_designationRepository.GetAllBy(d => d.DepartmentId == user.DepartmentId), "Id", "Title", user.DesignationId);
            ViewBag.LocationId = new SelectList(_locationRepository.GetAll(), "Id", "Title", user.LocationId);
            ViewBag.ReportingPersonId = new SelectList(_userRepository.GetAllBy(u => u.EmployeeStatus != EmployeeStatus.Ex && u.Id != 1, "Person"), "Id", "Person.Name", user.ReportingPersonId);
            ViewBag.ManagerId = new SelectList(_userRepository.GetAllBy(u => u.EmployeeStatus != EmployeeStatus.Ex && u.Id != 1, "Person"), "Id", "Person.Name", user.ManagerId);
            ViewBag.ShiftId = new SelectList(_shiftRepository.GetAll(), "Id", "Title", user.ShiftId);

            var vm = new EditUserViewModel(user);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditUserViewModel vm)
        {
            var selectedUser = _userRepository.Get(vm.Id, "Person");

            if (selectedUser != null)
            {
                selectedUser.EmployeeCode = vm.EmployeeCode;
                selectedUser.Person.FirstName = vm.Person.FirstName;
                selectedUser.Person.MiddleName = vm.Person.MiddleName;
                selectedUser.Person.LastName = vm.Person.LastName;
                selectedUser.Person.Gender = vm.Person.Gender;
                selectedUser.Person.Email = vm.Person.Email;
                selectedUser.Person.PhoneNo = vm.Person.PhoneNo;
                selectedUser.Person.SecondaryEmail = vm.Person.SecondaryEmail;
                selectedUser.Person.Address = vm.Person.Address;
                selectedUser.Person.CommunicationAddress = vm.Person.CommunicationAddress;
                selectedUser.Person.PassportNo = vm.Person.PassportNo;
                selectedUser.Person.DateOfBirth = vm.Person.DateOfBirth;
                selectedUser.Person.BloodGroup = vm.Person.BloodGroup;
                selectedUser.Person.MaritalStatus = vm.Person.MaritalStatus;
                selectedUser.Person.MarriageAnniversary = vm.Person.MarriageAnniversary;
                selectedUser.DepartmentId = vm.DepartmentId;
                selectedUser.LocationId = vm.LocationId;
                selectedUser.DesignationId = vm.DesignationId;
                selectedUser.ShiftId = vm.ShiftId;
                selectedUser.ReportingPersonId = vm.ReportingPersonId;
                selectedUser.ManagerId = vm.ManagerId;
                selectedUser.Experience = vm.Experience;
                selectedUser.DateOfJoin = vm.DateOfJoin;
                selectedUser.ConfirmationDate = vm.ConfirmationDate;
                selectedUser.DateOfResignation = vm.DateOfResignation;
                selectedUser.LastDate = vm.LastDate;
                selectedUser.OfficialEmail = vm.OfficialEmail;
                selectedUser.OfficialPhone = vm.OfficialPhone;
                selectedUser.OfficialMessengerId = vm.OfficialMessengerId;
                selectedUser.EmployeeStatus = vm.EmployeeStatus;
                selectedUser.RequiresTimeSheet = vm.RequiresTimeSheet;
                selectedUser.Salary = vm.Salary;
                selectedUser.Bank = vm.Bank;
                selectedUser.BankAccountNumber = vm.BankAccountNumber;
                selectedUser.PANCard = vm.PANCard;
                selectedUser.PaymentMode = vm.PaymentMode;

                _userRepository.Update(selectedUser);
                _unitOfWork.Commit();

                // Remove the existing mapped Roles 
                var existingRoles = _roleMemberRepository.GetAllBy(m => m.UserId == selectedUser.Id);
                foreach (var map in existingRoles)
                {
                    _roleMemberRepository.Delete(map);
                }

                if (vm.RoleIds != null)
                {
                  //  Map the New Technologies
                    foreach (var roleId in vm.RoleIds)
                    {
                        var newMap = new RoleMember
                        {
                            UserId = vm.Id,
                            RoleId = roleId
                        };

                        _roleMemberRepository.Create(newMap);
                    }

                    _unitOfWork.Commit();
                }

                // Remove the existing mapped Technologies 
                var existingMaps = _userTechnologyMapRepository.GetAllBy(m => m.UserId == selectedUser.Id);
                foreach (var map in existingMaps)
                {
                    _userTechnologyMapRepository.Delete(map);
                }

                _unitOfWork.Commit();

                if (vm.TechnologyIds != null)
                {
                    // Map the New Technologies
                    foreach (var technologyId in vm.TechnologyIds)
                    {
                        var newMap = new UserTechnologyMap
                        {
                            UserId = vm.Id,
                            TechnologyId = technologyId
                        };

                        _userTechnologyMapRepository.Create(newMap);
                    }

                    _unitOfWork.Commit();
                }


                // Remove the existing mapped Skills 
                var existingSkillMaps = _userSkillRepository.GetAllBy(m => m.UserId == selectedUser.Id);
                foreach (var map in existingSkillMaps)
                {
                    _userSkillRepository.Delete(map);
                }

                _unitOfWork.Commit();

                if (vm.SkillIds != null)
                {
                    // Map the New Technologies
                    foreach (var skillId in vm.SkillIds)
                    {
                        var newMap = new UserSkill
                        {
                            UserId = vm.Id,
                            SkillId = skillId
                        };

                        _userSkillRepository.Create(newMap);
                    }

                    _unitOfWork.Commit();
                }

                // Remove the existing mapped Skills 
                var existingHobbyMaps = _userHobbyRepository.GetAllBy(m => m.UserId == selectedUser.Id);
                foreach (var map in existingHobbyMaps)
                {
                    _userHobbyRepository.Delete(map);
                }

                _unitOfWork.Commit();

                if (vm.HobbiesId != null)
                {
                    // Map the New Technologies
                    foreach (var hobbyId in vm.HobbiesId)
                    {
                        var newMap = new UserHobby
                        {
                            UserId = vm.Id,
                            HobbyId = hobbyId
                        };

                        _userHobbyRepository.Create(newMap);
                    }

                    _unitOfWork.Commit();
                }

                // Remove the existing mapped Certifications 
                var existingCertificationMaps = _userCertificationRepository.GetAllBy(m => m.UserId == selectedUser.Id);
                foreach (var map in existingCertificationMaps)
                {
                    _userCertificationRepository.Delete(map);
                }

                _unitOfWork.Commit();

                if (vm.CertificationIds != null)
                {
                    // Map the New Technologies
                    foreach (var certificateId in vm.CertificationIds)
                    {
                        var newMap = new UserCertification
                        {
                            UserId = vm.Id,
                            CertificationId = certificateId
                        };

                        _userCertificationRepository.Create(newMap);
                    }

                    _unitOfWork.Commit();
                }

                return RedirectToAction("Index");
            }

            var allErrors = ModelState.Values.SelectMany(v => v.Errors);

            ViewBag.Roles = new MultiSelectList(_roleRepository.GetAll(), "Id", "Name", vm.RoleIds);
            ViewBag.DepartmentId = new SelectList(_departmentRepository.GetAll(), "Id", "Title", vm.DepartmentId);
            ViewBag.DesignationId = new SelectList(_designationRepository.GetAllBy(d => d.DepartmentId == vm.DepartmentId), "Id", "Title", vm.DesignationId);
            ViewBag.LocationId = new SelectList(_locationRepository.GetAll(), "Id", "Title", vm.LocationId);
            ViewBag.ReportingPersonId = new SelectList(_userRepository.GetAllBy(u => u.EmployeeStatus != EmployeeStatus.Ex && u.Id != 1, "Person"), "Id", "Person.Name", vm.ReportingPersonId);
            ViewBag.ManagerId = new SelectList(_userRepository.GetAllBy(u => u.EmployeeStatus != EmployeeStatus.Ex && u.Id != 1, "Person"), "Id", "Person.Name", vm.ManagerId);
            ViewBag.ShiftId = new SelectList(_shiftRepository.GetAll(), "Id", "Title", vm.ShiftId);
            return View(vm);
        }

        [HttpGet]
        public ActionResult EditImage(int id)
        {
            var selectedUser = _userRepository.Get(id, "Person");
            var vm = new EditImageViewModel
            {
                UserId = id
            };
            if (selectedUser != null)
            {
                vm.PhotoPath = selectedUser.Person.PhotoPath;
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult EditImage(EditImageViewModel vm)
        {
            if (vm.Photo != null)
            {
                var selectedUser = _userRepository.Get(vm.UserId, "Person");
                if (selectedUser != null)
                {
                    var siteSettings = _settingsService.GetSiteSettings();
                    var blobUploadService = new BlobUploadService(siteSettings.BlobSettings);
                    var blobPath = blobUploadService.UploadProfileImage(vm.Photo);
                    selectedUser.Person.PhotoPath = blobPath;

                    _userRepository.Update(selectedUser);
                    _unitOfWork.Commit();
                }
            }

            return RedirectToAction("Details", "Users", new { id = vm.UserId });
        }

        public ActionResult Delete(int id)
        {
            var user = _userRepository.Get(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _userRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
