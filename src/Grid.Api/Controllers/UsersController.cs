using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Api.Models.HRMS;
using Grid.Features.Auth.DAL.Interfaces;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Newtonsoft.Json;
using Grid.Features.HRMS.Entities;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Features.HRMS.ViewModels;
using Grid.Features.HRMS.ViewModels.User;
using Grid.Features.IMS.DAL.Interfaces;
using Grid.Features.IT.DAL.Interfaces;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.Settings.Services.Interfaces;


namespace Grid.Api.Controllers
{
    public class UsersController : GridApiBaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserSkillRepository _userSkillRepository;
        private readonly IUserCertificationRepository _userCertificationRepository;
        private readonly IRoleMemberRepository _roleMemberRepository;
        private readonly IUserTechnologyMapRepository _userTechnologyMapRepository;
        private readonly IUserHobbyRepository _userHobbyRepository;
        private readonly IUnitOfWork _unitOfWork;

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
            _userRepository = userRepository;
            _userSkillRepository = userSkillRepository;
            _userCertificationRepository = userCertificationRepository;
            _roleMemberRepository = roleMemberRepository;
            _userTechnologyMapRepository = userTechnologyMapRepository;
            _userHobbyRepository = userHobbyRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index(UserSearchViewModel vm)
        {
            Func<IQueryable<User>, IQueryable<User>> userFilter = q =>
            {
                q = q.Include(u => u.AccessRule)
                     .Include(u => u.Department)
                     .Include(u => u.Designation)
                     .Include(u => u.Location)
                     .Include(u => u.Person);

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

            var users = _userRepository.Search(userFilter);

            var result = new ApiResult<List<UserModel>>
            {
                Status = true,
                Result = users.Select(u => new UserModel(u)).ToList(),
                Message = "Users Fetched sucessfully"
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var selectedUser = _userRepository.Get(id, "Person,AccessRule,ReportingPerson.Person,Location,Department,Designation,Shift");
            var result = new ApiResult<User>
            {
                Status = true,
                Result = selectedUser,
                Message = "User Fetched sucessfully"
            };
            var json = JsonConvert.SerializeObject(result, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Content(json, "application/json");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _userRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "User deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(NewUserViewModel vm)
        {
            ApiResult<User> apiResult;

            if (ModelState.IsValid)
            {
                if (vm.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var selectedUser = _userRepository.Get(vm.Id, "Person");
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
                            // Map the New Technologies
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
                        var existingCertificationMaps =
                            _userCertificationRepository.GetAllBy(m => m.UserId == selectedUser.Id);
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

                        return selectedUser;
                    }, "User updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
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


                        // Map the Technologies
                        if (vm.RoleIds != null)
                        {
                            foreach (var roleId in vm.RoleIds)
                            {
                                var newMap = new RoleMember
                                {
                                    UserId = newUser.Id,
                                    RoleId = roleId
                                };

                                _roleMemberRepository.Create(newMap);
                            }

                            _unitOfWork.Commit();
                        }

                        return newUser;

                    }, "User created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<User>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
