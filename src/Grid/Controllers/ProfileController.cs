using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Grid.Clients.ITSync.Models;
using Grid.Features.Auth.DAL.Interfaces;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.ViewModels;
using Newtonsoft.Json;
using Grid.Infrastructure;
using Grid.Providers.Blob;
using Grid.Providers.Slack;
using Grid.Providers.Social;
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

namespace Grid.Controllers
{
    [GridPermission(PermissionCode = 300)]
    public class ProfileController : GridBaseController
    {
        private readonly IAccessRuleRepository _accessRuleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserSkillRepository _userSkillRepository;
        private readonly IUserCertificationRepository _userCertificationRepository;
        private readonly IUserDocumentRepository _userDocumentRepository;
        private readonly IUserHobbyRepository _userHobbyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILinkedAccountRepository _linkedAccountRepository;

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

        private readonly ITechnologyRepository _technologyRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IHobbyRepository _hobbyRepository;
        private readonly ICertificationRepository _certificationRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISettingsService _settingsService;

        public ProfileController(IAccessRuleRepository accessRuleRepository,
                                 IUserRepository userRepository,
                                 IUserSkillRepository userSkillRepository,
                                 IUserCertificationRepository userCertificationRepository,          
                                 IUserDocumentRepository userDocumentRepository,
                                 IUserHobbyRepository userHobbyRepository,
                                 ILinkedAccountRepository linkedAccountRepository,
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
                                 ICertificationRepository certificationRepository,
                                 ISystemSnapshotRepository systemSnapshotRepository,
                                 ISettingsService settingsService,
                                 IEmployeeRepository employeeRepository,
                                 IUnitOfWork unitOfWork)
        {
            _accessRuleRepository = accessRuleRepository;
            _userRepository = userRepository;
            _userSkillRepository = userSkillRepository;
            _userCertificationRepository = userCertificationRepository;
            _userDocumentRepository = userDocumentRepository;
            _userHobbyRepository = userHobbyRepository;

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

            _technologyRepository = technologyRepository;
            _skillRepository = skillRepository;
            _hobbyRepository = hobbyRepository;
            _certificationRepository = certificationRepository;
            _employeeRepository = employeeRepository;
            _linkedAccountRepository = linkedAccountRepository;
            _settingsService = settingsService;
            _unitOfWork = unitOfWork;
        }
        
        #region Ajax Calls
        [HttpPost]
        public JsonResult AddSkill(AddSkillViewModel vm)
        {
            var userSkill = new UserSkill
            {
                UserId = WebUser.Id,
                SkillId = vm.SkillId,
            };

            _userSkillRepository.Create(userSkill);
            _unitOfWork.Commit();

            return Json(true);
        }

        public JsonResult AddCertification(AddCertificationViewModel vm)
        {
            var userSkill = new UserCertification
            {
                UserId = WebUser.Id,
                CertificationId = vm.CertificationId,
            };

            _userCertificationRepository.Create(userSkill);
            _unitOfWork.Commit();

            return Json(true);
        }
        #endregion


        [HttpGet]
        [SelectList("Skill", "SkillId")]
        [SelectList("Certification", "CertificationId")]
        public ActionResult Index()
        {
            var user = _userRepository.GetBy(u => u.Id == WebUser.Id, "Person,AccessRule,ReportingPerson.Person,Manager.Person,Location,Department,Designation,Shift");
            var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person,ReportingPerson.User.Person,Manager.User.Person,Location,Department,Designation,Shift");

            var userDocs = _userDocumentRepository.GetAllBy(m => m.UserId == user.Id);
            var userSkills = _userSkillRepository.GetAllBy(m => m.UserId == user.Id,"Skill");
            var userHobbies = _userHobbyRepository.GetAllBy(m => m.UserId == user.Id, "Hobby");
            var userCertifications = _userCertificationRepository.GetAllBy(m => m.UserId == user.Id, "Certification");

            var roleMembers = _roleMemberRepository.GetAllBy(m => m.UserId == user.Id, "Role").ToList();
            var assets = _assetRepository.GetAllBy(u => u.AllocatedEmployeeId == user.Id, "AssetCategory").ToList();
            var projects = _projectMemberRepository.GetAllBy(p => p.EmployeeId == user.Id, "Project").ToList();
            var technologies = _userTechnologyMapRepository.GetAllBy(r => r.UserId == user.Id, "Technology").Select(t => t.Technology).ToList();
            //var emergencyContacts = _emergencyContactRepository.GetAllBy(c => c.UserId == user.Id).ToList();
            //var dependants = _employeeDependentRepository.GetAllBy(d => d.UserId == user.Id).ToList();
            var reportees = _userRepository.GetAllBy(u => u.EmployeeStatus != EmployeeStatus.Ex && u.ReportingPersonId == WebUser.Id, "Person,Designation,AccessRule").ToList();
            var tokens = _tokenRepository.GetAllBy(t => t.AllocatedToUserId == user.Id).ToList();
            var awards = _userAwardRepository.GetAllBy(u => u.UserId == user.Id, "Award").ToList();
            ViewBag.WebuserId = employee.Id;
            var userViewModel = new UserViewModel(user)
            {
                UserDocuments = userDocs.ToList(),
                UserSkills = userSkills.ToList(),
                UserCertifications = userCertifications.ToList(),
                RoleMembers = roleMembers.ToList(),
                //Projects = projects.ToList(),
                Technologies = technologies,
                //EmergencyContacts = emergencyContacts.ToList(),
                //EmployeeDependents = dependants.ToList(),
                Reportees = reportees,
                //Tokens = tokens,
                UserHobbies = userHobbies.ToList(),
                UserAwards = awards
            };

            var linkedAccounts = _linkedAccountRepository.GetAllBy(u => u.UserId == user.Id).ToList();
            userViewModel.LinkedAccounts = linkedAccounts.ToList();

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

        [HttpGet]
        public ActionResult ChangePassword(bool expired = false)
        {
            ViewBag.Message = expired ? "It's been a long time, Please update your password" : "";
            var vm = new ChangePasswordViewModel();
            return View(vm);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(vm.CurrentPassword))
                        throw new Exception("Current Password is Empty");

                    if (string.IsNullOrEmpty(vm.NewPassword) || string.IsNullOrEmpty(vm.ConfirmNewPassword))
                        throw new Exception("New Password is Empty");

                    if (vm.NewPassword != vm.ConfirmNewPassword)
                        throw new Exception("New Password and Confirmation doesn't match");

                    if (vm.NewPassword == vm.CurrentPassword)
                        throw new Exception("New Password should be different from current password");

                    var user = _userRepository.GetBy(u => u.Id == WebUser.Id, "AccessRule,Person");

                    if (user == null)
                        throw new Exception("User not found");

                    if (user.AccessRule == null)
                        throw new Exception("User AccessRule not found");

                    if (!user.AccessRule.IsApproved)
                        throw new Exception("User is not approved");

                    if (vm.NewPassword != vm.ConfirmNewPassword)
                        throw new Exception("Passwords don't match");

                    var hashedPassword = user.Password;

                    var verificationSucceeded = hashedPassword != null && HashHelper.CheckHash(vm.CurrentPassword, hashedPassword);

                    if (verificationSucceeded)
                    {
                        user.Password = HashHelper.Hash(vm.NewPassword);
                        _userRepository.Update(user);
                        _unitOfWork.Commit();

                        // Update Last Password Changed Time
                        var selectedAccessRule = _accessRuleRepository.Get(user.AccessRuleId);
                        if (selectedAccessRule != null)
                        {
                            selectedAccessRule.LastPasswordChangedDate = DateTime.UtcNow;
                            selectedAccessRule.LastActivityDate = DateTime.UtcNow;
                            _accessRuleRepository.Update(selectedAccessRule);
                            _unitOfWork.Commit();
                        }

                        return RedirectToAction("SignOut", "Account");
                    }

                    ModelState.AddModelError(string.Empty, "Incorrect Current Password");

                    return View(vm);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(vm);
                }
                
            }

            return View(vm);
        }


        [HttpGet]
        public ActionResult Edit()
        {
            var mappedTechnologies = _userTechnologyMapRepository.GetAllBy(m => m.UserId == WebUser.Id).Select(m => m.TechnologyId).ToList();
            var mappedSkills = _userSkillRepository.GetAllBy(m => m.UserId == WebUser.Id).Select(m => m.SkillId).ToList();
            var mappedHobbies = _userHobbyRepository.GetAllBy(m => m.UserId == WebUser.Id).Select(m => m.HobbyId).ToList();
            var mappedCertifications = _userCertificationRepository.GetAllBy(m => m.UserId == WebUser.Id).Select(m => m.CertificationId).ToList();

            ViewBag.Technologies = new MultiSelectList(_technologyRepository.GetAll(), "Id", "Title", mappedTechnologies);
            ViewBag.Skills = new MultiSelectList(_skillRepository.GetAll(), "Id", "Title", mappedSkills);
            ViewBag.Hobbies = new MultiSelectList(_hobbyRepository.GetAll(), "Id", "Title", mappedHobbies);
            ViewBag.Certifications = new MultiSelectList(_certificationRepository.GetAll(), "Id", "Title", mappedCertifications);

            var selectedUser = _userRepository.GetBy(u => u.Id == WebUser.Id, "Person");
            var vm = new EditUserViewModel(selectedUser);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(EditUserViewModel vm)
        {
            var selectedUser = _userRepository.GetBy(u => u.Id == WebUser.Id, "Person");

            if (selectedUser != null)
            {
                selectedUser.Person.FirstName = vm.Person.FirstName;
                selectedUser.Person.MiddleName = vm.Person.MiddleName;
                selectedUser.Person.LastName = vm.Person.LastName;
                selectedUser.Person.Gender = vm.Person.Gender;
                selectedUser.Person.Email = vm.Person.Email;
                selectedUser.Person.SecondaryEmail = vm.Person.SecondaryEmail;
                selectedUser.Person.PhoneNo = vm.Person.PhoneNo;
                selectedUser.Person.OfficePhone = vm.Person.OfficePhone;
                selectedUser.Person.Website = vm.Person.Website;
                selectedUser.Person.Skype = vm.Person.Skype;
                selectedUser.Person.Facebook = vm.Person.Facebook;
                selectedUser.Person.Twitter = vm.Person.Twitter;
                selectedUser.Person.GooglePlus = vm.Person.GooglePlus;
                selectedUser.Person.LinkedIn = vm.Person.LinkedIn;
                selectedUser.Person.City = vm.Person.City;
                selectedUser.Person.Country = vm.Person.Country;
                selectedUser.Person.Address = vm.Person.Address;
                selectedUser.Person.CommunicationAddress = vm.Person.CommunicationAddress;
                selectedUser.Person.PassportNo = vm.Person.PassportNo;
                selectedUser.Person.DateOfBirth = vm.Person.DateOfBirth;
                selectedUser.Person.BloodGroup = vm.Person.BloodGroup;
                selectedUser.Person.MaritalStatus = vm.Person.MaritalStatus;
                selectedUser.Person.MarriageAnniversary = vm.Person.MarriageAnniversary;
                selectedUser.OfficialPhone = vm.OfficialPhone;

                _userRepository.Update(selectedUser);
                _unitOfWork.Commit();

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
                var existingSkillMaps = _userSkillRepository.GetAllBy(m => m.UserId == selectedUser.Id).ToList();
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
            
            return View(vm);
        }

        [HttpGet]
        public ActionResult EditImage()
        {
            var selectedUser = _userRepository.GetBy(u => u.Id == WebUser.Id, "Person");
            var vm = new EditImageViewModel
            {
                UserId = WebUser.Id
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
                var selectedUser = _userRepository.GetBy(u => u.Id == WebUser.Id, "Person");
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

            return RedirectToAction("Index");
        }

        public ActionResult DeleteMySkill(int? id)
        {
            var userSkill = _userSkillRepository.Get(id.GetValueOrDefault(), "User.Person");
            if (userSkill == null)
            {
                return HttpNotFound();
            }
            return View(userSkill);
        }

        [HttpPost, ActionName("DeleteMySkill")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMySkill(int id)
        {
            _userSkillRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index", "Profile", new { id = WebUser.Id});
        }

        public ActionResult DeleteMyCertification(int? id)
        {
            var userCertification = _userCertificationRepository.Get(id.GetValueOrDefault(), "User.Person");
            if (userCertification == null)
            {
                return HttpNotFound();
            }

            return View(userCertification);
        }

        [HttpPost, ActionName("DeleteMyCertification")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMyCertification(int id)
        {
            _userCertificationRepository.Delete(id);
            return RedirectToAction("Index", "Profile", new { id = WebUser.Id });
        }

        [HttpGet]
        public ActionResult ConnectSlack()
        {
            var siteSettings = _settingsService.GetSiteSettings();
            var slackService = new SlackService(siteSettings.SlackSettings);

            var urlHelper = new UrlHelper(ControllerContext.RequestContext);
            var callbackUrl = urlHelper.Action("SlackCallback", "Profile", null, "http");

            var redirect = slackService.Authorize(callbackUrl);
            return Redirect(redirect);
        }

        [HttpGet]
        public ActionResult SlackCallback(string code)
        {
            var siteSettings = _settingsService.GetSiteSettings();
            var slackService = new SlackService(siteSettings.SlackSettings);

            var oauthToken = slackService.ExchangeCodeForToken(code);
            var slackExists = _linkedAccountRepository.GetBy(l => l.AccountType == LinkedAccountType.Slack && l.UserId == WebUser.Id);
            if (slackExists != null)
            {
                slackExists.AccessToken = oauthToken.access_token;
                slackExists.Scope = oauthToken.scope;
                _linkedAccountRepository.Update(slackExists);
                _unitOfWork.Commit();
            }
            else
            {
                var linkedAccount = new LinkedAccount
                {
                    AccountType = LinkedAccountType.Slack,
                    UserId = WebUser.Id,
                    AccessToken = oauthToken.access_token,
                    Scope = oauthToken.scope
                };

                _linkedAccountRepository.Create(linkedAccount);
                _unitOfWork.Commit();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ConnectLinkedIn()
        {
            var state = new Guid().ToString("N");
            Session["linkedin_state"] = state;
            var linkedInService = new LinkedInService("81ed9a1o1frnfw", "1LhWRlz3ZCWxSjyF");

            var urlHelper = new UrlHelper(ControllerContext.RequestContext);
            var callbackUrl = urlHelper.Action("LinkedInCallback", "Profile", null, "http");

            var redirect = linkedInService.Authorize(state, callbackUrl);
            return Redirect(redirect);
        }

        [HttpGet]
        public ActionResult LinkedInCallback(string code, string state)
        {
            var linkedInService = new LinkedInService("81ed9a1o1frnfw", "1LhWRlz3ZCWxSjyF");
            var urlHelper = new UrlHelper(ControllerContext.RequestContext);
            var callbackUrl = urlHelper.Action("LinkedInCallback", "Profile", null, "http");
            state = Session["linkedin_state"].ToString();
            var oauthToken = linkedInService.ExchangeCodeForToken(code, state, callbackUrl);

            var linkedInExists = _linkedAccountRepository.GetBy(l => l.AccountType == LinkedAccountType.LinkedIn && l.UserId == WebUser.Id);
            if (linkedInExists != null)
            {
                linkedInExists.AccessToken = oauthToken.access_token;
                linkedInExists.Scope = oauthToken.scope;
                _linkedAccountRepository.Update(linkedInExists);
                _unitOfWork.Commit();
            }
            else
            {
                var linkedAccount = new LinkedAccount
                {
                    AccountType = LinkedAccountType.LinkedIn,
                    UserId = WebUser.Id,
                    AccessToken = oauthToken.access_token,
                    Scope = oauthToken.scope
                };

                _linkedAccountRepository.Create(linkedAccount);
                _unitOfWork.Commit();
            }

            return RedirectToAction("Index");
        }
    }
}