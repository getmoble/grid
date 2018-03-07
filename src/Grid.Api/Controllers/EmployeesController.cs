using Grid.Api.Models;
using Grid.Api.Models.HRMS;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;
using Grid.Features.HRMS.ViewModels;
using Grid.Features.Settings.Services.Interfaces;
using Grid.Infrastructure;
using Grid.Infrastructure.Filters;
using Grid.Providers.Blob;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Grid.Api.Controllers
{
    [APIIdentityInjector]
    public class EmployeesController : GridApiBaseController
    {
        private readonly IPersonRepository _personRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserTechnologyMapRepository _userTechnologyMapRepository;
        private readonly IRoleMemberRepository _roleMemberRepository;
        private readonly IAccessRuleRepository _accessRuleRepository;      
        private readonly ISettingsService _settingsService;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesController(IPersonRepository personRepository,
                                   IUserRepository userRepository,
                                   IEmployeeRepository employeeRepository,
                                   IUserTechnologyMapRepository userTechnologyMapRepository,
                                   IRoleRepository roleRepository,
                                   IRoleMemberRepository roleMemberRepository,
                                   IAccessRuleRepository accessRuleRepository,
                                   ISettingsService settingsService,
                                   IUnitOfWork unitOfWork)
        {
            _personRepository = personRepository;
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
            _roleMemberRepository = roleMemberRepository;
            _userTechnologyMapRepository = userTechnologyMapRepository;
            _accessRuleRepository = accessRuleRepository;
            _settingsService = settingsService;

            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize]
        public JsonResult Index()
        {
            var apiResult = TryExecute(() =>
            {
                return _employeeRepository.GetAll().Select(h => new EmployeeModel(h)).ToList();
            }, "Employees Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]

        public JsonResult Get(int id)
        {
            var apiResult = TryExecute(() =>
            {
                var employee = _employeeRepository.Get(id, "User,User.Person,ReportingPerson.User.Person,Manager.User.Person,Location,Department,Designation,Shift");
                var employeeVm = new EmployeeModel(employee);
                var userTechnologyIds = _userTechnologyMapRepository.GetAllBy(i=>i.UserId == employeeVm.UserId).Select(i=>i.TechnologyId).ToList();
                var userRoleIds = _roleMemberRepository.GetAllBy(i => i.UserId == employeeVm.UserId).Select(i => i.RoleId).ToList();
                employeeVm.TechnologyIds = userTechnologyIds;
                employeeVm.RoleIds = userRoleIds;
                return employeeVm;
            }, "Employee Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetRoleAndTechnologyNames(int id)
        {
            var apiResult = TryExecute(() =>
            {
                var userTechnologyNames = _userTechnologyMapRepository.GetAllBy(i => i.UserId == id, "User,User.Person,Technology").Select(i => i.Technology.Title).ToList();
                var userRoleNames = _roleMemberRepository.GetAllBy(i => i.UserId == id, "User,User.Person,Role").Select(i => i.Role.Name).ToList();                
                return new { UserTechnologyNames =userTechnologyNames, UserRoleNames = userRoleNames };
            }, "Employee Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AllowCrossSiteJson]
        public JsonResult Create(EmployeeModel model)
        {
            ApiResult<Employee> apiResult;

            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var newPerson = new Person
                        {
                            Id = model.PersonId,
                            FirstName = model.FirstName,
                            MiddleName = model.MiddleName,
                            LastName = model.LastName,
                            Gender = model.Gender,
                            Email = model.Email,
                            SecondaryEmail = model.SecondaryEmail,
                            PhoneNo = model.PhoneNo,
                            Address = model.Address,
                            CommunicationAddress = model.CommunicationAddress,
                            PassportNo = model.PassportNo,
                            DateOfBirth = model.DateOfBirth,
                            BloodGroup = model.BloodGroup,
                            MaritalStatus = model.MaritalStatus,
                            MarriageAnniversary = model.MarriageAnniversary,
                            PhotoPath = model.PhotoPath
                        };

                        _personRepository.Update(newPerson);
                        var user = _userRepository.GetBy(i=>i.Id == model.UserId);

                        user.Id = model.UserId;
                        user.PersonId = model.PersonId;
                        user.Username = model.Username;                                         
                        user.EmployeeCode = model.EmployeeCode;
                        user.DepartmentId = model.DepartmentId;
                        user.LocationId = model.LocationId;
                        user.DesignationId = model.DesignationId;
                        user.ShiftId = model.ShiftId;
                        user.Salary = model.Salary;
                        user.Bank = model.Bank;
                        user.BankAccountNumber = model.BankAccountNumber;
                        user.PANCard = model.PANCard;
                        user.PaymentMode = model.PaymentMode;
                        user.Experience = model.Experience;
                        user.DateOfJoin = model.DateOfJoin;
                        user.ConfirmationDate = model.ConfirmationDate;
                        user.DateOfResignation = model.DateOfResignation;
                        user.LastDate = model.LastDate;
                        user.OfficialEmail = model.OfficialEmail;
                        user.OfficialPhone = model.OfficialPhone;
                        user.OfficialMessengerId = model.OfficialMessengerId;
                        user.EmployeeStatus = model.EmployeeStatus;
                        user.RequiresTimeSheet = model.RequiresTimeSheet;
                        user.SeatNo = model.SeatNo;
                        user.AccessRuleId = model.AccessRuleId;
                        

                        _userRepository.Update(user);                       

                        var newEmployee = new Employee
                        {

                            Id = model.EmployeeId,
                            UserId = model.UserId,
                            EmployeeCode = model.EmployeeCode,
                            DepartmentId = model.DepartmentId,
                            LocationId = model.LocationId,
                            DesignationId = model.DesignationId,
                            ShiftId = model.ShiftId,
                            Salary = model.Salary,
                            Bank = model.Bank,
                            BankAccountNumber = model.BankAccountNumber,
                            PANCard = model.PANCard,
                            PaymentMode = model.PaymentMode,
                            ReportingPersonId = model.ReportingPersonId,
                            ManagerId = model.ManagerId,
                            Experience = model.Experience,
                            DateOfJoin = model.DateOfJoin,
                            ConfirmationDate = model.ConfirmationDate,
                            DateOfResignation = model.DateOfResignation,
                            LastDate = model.LastDate,
                            OfficialEmail = model.OfficialEmail,
                            OfficialPhone = model.OfficialPhone,
                            OfficialMessengerId = model.OfficialMessengerId,
                            EmployeeStatus = model.EmployeeStatus,
                            RequiresTimeSheet = model.RequiresTimeSheet,
                            SeatNo = model.SeatNo

                        };
                        _employeeRepository.Update(newEmployee);
                        _unitOfWork.Commit();


                        var User = _userRepository.Get(model.UserId);
                        if (User != null)
                        {
                            var reportingPerson = _employeeRepository.Get(newEmployee.ReportingPersonId.Value);
                            var manager = _employeeRepository.Get(newEmployee.ManagerId.Value);

                            User.ReportingPersonId = reportingPerson.UserId;
                            User.ManagerId = manager.UserId;

                            _userRepository.Update(User);
                            _unitOfWork.Commit();
                        }

                        // Remove the existing mapped Roles 
                        var existingRoles = _roleMemberRepository.GetAllBy(m => m.UserId == user.Id);
                        foreach (var map in existingRoles)
                        {
                            _roleMemberRepository.Delete(map);
                        }

                        if (model.RoleIds != null)
                        {
                           // Map the New Technologies
                            foreach (var roleId in model.RoleIds)
                            {
                                var newMap = new RoleMember
                                {
                                    UserId = model.UserId,
                                    RoleId = roleId
                                };

                                _roleMemberRepository.Create(newMap);
                            }

                            _unitOfWork.Commit();
                        }

                        // Remove the existing mapped Technologies 
                        var existingMaps = _userTechnologyMapRepository.GetAllBy(m => m.UserId == model.UserId);
                        foreach (var map in existingMaps)
                        {
                            _userTechnologyMapRepository.Delete(map);
                        }

                        _unitOfWork.Commit();

                        if (model.TechnologyIds != null)
                        {
                            // Map the New Technologies
                            foreach (var technologyId in model.TechnologyIds)
                            {
                                var newMap = new UserTechnologyMap
                                {
                                    UserId = model.UserId,
                                    TechnologyId = technologyId
                                };

                                _userTechnologyMapRepository.Create(newMap);
                            }

                            _unitOfWork.Commit();
                        }

                        return newEmployee;
                    }, "Employee updated sucessfully");
                }
                else {
                    apiResult = TryExecute(() =>
                    {
                        var newPerson = new Person
                        {
                            FirstName = model.FirstName,
                            MiddleName = model.MiddleName,
                            LastName = model.LastName,
                            Gender = model.Gender,
                            Email = model.Email,
                            SecondaryEmail = model.SecondaryEmail,
                            PhoneNo = model.PhoneNo,
                            Address = model.Address,
                            CommunicationAddress = model.CommunicationAddress,
                            PassportNo = model.PassportNo,
                            DateOfBirth = model.DateOfBirth,
                            BloodGroup = model.BloodGroup,
                            MaritalStatus = model.MaritalStatus,
                            MarriageAnniversary = model.MarriageAnniversary,
                            PhotoPath = model.PhotoPath

                        };
                        var person = _personRepository.Create(newPerson);
                       
                        var newUser = new User
                        {
                            PersonId = person.Id,
                            Username = model.Username,
                            Password = HashHelper.Hash(model.Password),
                            AccessRule = AccessRule.CreateNewUserAccessRule(true),
                            EmployeeCode = model.EmployeeCode,
                            DepartmentId = model.DepartmentId,
                            LocationId = model.LocationId,
                            DesignationId = model.DesignationId,
                            ShiftId = model.ShiftId,
                            Salary = model.Salary,
                            Bank = model.Bank,
                            BankAccountNumber = model.BankAccountNumber,
                            PANCard = model.PANCard,
                            PaymentMode = model.PaymentMode,                          
                            Experience = model.Experience,
                            DateOfJoin = model.DateOfJoin,
                            ConfirmationDate = model.ConfirmationDate,
                            DateOfResignation = model.DateOfResignation,
                            LastDate = model.LastDate,
                            OfficialEmail = model.OfficialEmail,
                            OfficialPhone = model.OfficialPhone,
                            OfficialMessengerId = model.OfficialMessengerId,
                            EmployeeStatus = model.EmployeeStatus,
                            RequiresTimeSheet = model.RequiresTimeSheet,
                            SeatNo = model.SeatNo,
                          
                        };
                        var user = _userRepository.Create(newUser);                       

                        var newEmployee = new Employee
                        {
                            UserId = user.Id,
                            EmployeeCode = model.EmployeeCode,
                            DepartmentId = model.DepartmentId,
                            LocationId = model.LocationId,
                            DesignationId = model.DesignationId,
                            ShiftId = model.ShiftId,
                            Salary = model.Salary,
                            Bank = model.Bank,
                            BankAccountNumber = model.BankAccountNumber,
                            PANCard = model.PANCard,
                            PaymentMode = model.PaymentMode,
                            ReportingPersonId = model.ReportingPersonId,
                            ManagerId = model.ManagerId,
                            Experience = model.Experience,
                            DateOfJoin = model.DateOfJoin,
                            ConfirmationDate = model.ConfirmationDate,
                            DateOfResignation = model.DateOfResignation,
                            LastDate = model.LastDate,
                            OfficialEmail = model.OfficialEmail,
                            OfficialPhone = model.OfficialPhone,
                            OfficialMessengerId = model.OfficialMessengerId,
                            EmployeeStatus = model.EmployeeStatus,
                            RequiresTimeSheet = model.RequiresTimeSheet,
                            SeatNo = model.SeatNo

                        };
                        _employeeRepository.Create(newEmployee);
                        _unitOfWork.Commit();


                        var User = _userRepository.Get(newUser.Id);
                        if (User != null)
                        {
                            var reportingPerson = _employeeRepository.Get(newEmployee.ReportingPersonId.Value);
                            var manager = _employeeRepository.Get(newEmployee.ManagerId.Value);

                            User.ReportingPersonId  = reportingPerson.UserId;
                            User.ManagerId = manager.UserId;

                            _userRepository.Update(User);
                            _unitOfWork.Commit();
                        }
                        
                        // Map the Technologies
                        if (model.TechnologyIds != null)
                        {
                            foreach (var technologyId in model.TechnologyIds)
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

                        // Map the Roles
                        if (model.RoleIds != null)
                        {
                            foreach (var roleId in model.RoleIds)
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



                        return newEmployee;
                    }, "Employee created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Employee>();
                var data = ApiResultFromModelErrors<Employee>();

            }
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ResetPassword(ResetPasswordViewModel vm)
        {
            ApiResult<User> apiResult;

            if (ModelState.IsValid)
            {
                if (vm.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var user = _userRepository.Get(vm.UserId, "AccessRule");
                        if (user != null)
                        {
                            user.Password = HashHelper.Hash(vm.NewPassword);
                            _userRepository.Update(user);
                            _unitOfWork.Commit();

                            user.AccessRule.LastPasswordResetDate = DateTime.UtcNow;
                            _accessRuleRepository.Update(user.AccessRule);
                            _unitOfWork.Commit();
                        }
                        return user;
                    }, "Password updated sucessfully");
                }
                else
                {
                    apiResult = ApiResultFromModelErrors<User>();
                }
            }
            return Json(vm, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _employeeRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Employee deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult UploadProfileImage(EditImageViewModel vm)
        {
            var apiResult = TryExecute(() =>
            {

                var imageViewModel = new ImageViewModel();
                if (vm.Photo != null)
                {
                    var siteSettings = _settingsService.GetSiteSettings();
                    var blobUploadService = new BlobUploadService(siteSettings.BlobSettings);
                    var blobPath = blobUploadService.UploadProfileImage(vm.Photo);
                    imageViewModel.SavedName = blobPath;
                    imageViewModel.OriginalName = vm.FileName;

                }
                return imageViewModel;
            }, "Image  uploaded sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);

        }
    }
}
