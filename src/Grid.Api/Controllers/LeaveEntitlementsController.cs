using Grid.Api.Models.LMS;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Features.LMS.Entities;
using Grid.Features.LMS.Entities.Enums;
using Grid.Features.LMS.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace Grid.Api.Controllers
{
    public class LeaveEntitlementsController : GridApiBaseController
    {
        private readonly ILeaveEntitlementRepository _leaveEntitlementRepository;
        private readonly ILeaveEntitlementUpdateRepository _leaveEntitlementUpdateRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveTimePeriodRepository _leaveTimePeriodRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LeaveEntitlementsController(ILeaveEntitlementRepository leaveEntitlementRepository,
                                        ILeaveEntitlementUpdateRepository leaveEntitlementUpdateRepository,
                                        ILeaveTimePeriodRepository leaveTimePeriodRepository,
                                        ILeaveTypeRepository leaveTypeRepository,
                                        IUserRepository userRepository,
                                        IUnitOfWork unitOfWork)
        {
            _leaveEntitlementRepository = leaveEntitlementRepository;
            _leaveEntitlementUpdateRepository = leaveEntitlementUpdateRepository;
            _leaveTimePeriodRepository = leaveTimePeriodRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]

        public JsonResult Index()
        {
            var apiResult = TryExecute(() =>
            {
                return _leaveEntitlementRepository.GetAll().Select(h => new LeaveEntitlementModel(h)).ToList();
            }, "Leave Entitlements Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]

        public JsonResult Get(int id)
        {
            var apiResult = TryExecute(() =>
            {
                var leaveEntitlement = _leaveEntitlementRepository.Get(id, "Employee,Employee.User,Employee.User.Person,LeaveType,LeaveTimePeriod");
                return new UpdateLeaveEntitlementViewModel(leaveEntitlement);
            }, "Leave Entitlements Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }      

        [HttpGet]
        public JsonResult GetLeaveEntitlementsLog(int id)
        {
            var apiResult = TryExecute(() => _leaveEntitlementUpdateRepository.Get(id, "Employee,Employee.User,Employee.User.Person,LeaveType,LeaveTimePeriod"), "Leave Entitlements Log fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
            
        [HttpPost]


        public JsonResult UpdateLeaveEntitlement(UpdateLeaveEntitlementViewModel vm)
        {
            var currentEntitlements = _leaveEntitlementRepository.GetBy(e => e.EmployeeId == vm.EmployeeId && e.LeaveTimePeriodId == vm.LeaveTimePeriodId && e.LeaveTypeId == vm.LeaveTypeId);
            if (vm.Operation == LeaveOperation.Deduct)
            {
                if (currentEntitlements != null)
                {
                    if (currentEntitlements.Allocation - vm.Allocation >= 0)
                    {
                        // Create a log
                        var newLeaveEntitlementUpdate = new LeaveEntitlementUpdate
                        {
                            EmployeeId = vm.EmployeeId,
                            LeaveTimePeriodId = vm.LeaveTimePeriodId,
                            LeaveTypeId = vm.LeaveTypeId,
                            Operation = vm.Operation,
                            LeaveCount = vm.Allocation,
                            PreviousBalance = currentEntitlements.Allocation,
                            NewBalance = currentEntitlements.Allocation - vm.Allocation,
                            Comments = vm.Comments,
                            CreatedByUserId = WebUser.Id
                        };

                        _leaveEntitlementUpdateRepository.Create(newLeaveEntitlementUpdate);
                        _unitOfWork.Commit();

                        // Update Entitlements
                        currentEntitlements.Allocation = currentEntitlements.Allocation - vm.Allocation;
                        currentEntitlements.UpdatedByUserId = WebUser.Id;

                        _leaveEntitlementRepository.Update(currentEntitlements);
                        _unitOfWork.Commit();

                        return Json(true);
                    }

                    return Json(false);
                }

                return Json(false);
            }
            else
            {
                if (currentEntitlements != null)
                {
                    // Create a log
                    var newLeaveEntitlementUpdate = new LeaveEntitlementUpdate
                    {
                        EmployeeId = vm.EmployeeId,
                        LeaveTimePeriodId = vm.LeaveTimePeriodId,
                        LeaveTypeId = vm.LeaveTypeId,
                        Operation = vm.Operation,
                        LeaveCount = vm.Allocation,
                        PreviousBalance = currentEntitlements.Allocation,
                        NewBalance = currentEntitlements.Allocation + vm.Allocation,
                        Comments = vm.Comments,
                        CreatedByUserId = WebUser.Id
                    };

                    _leaveEntitlementUpdateRepository.Create(newLeaveEntitlementUpdate);
                    _unitOfWork.Commit();

                    // Update Entitlements
                    currentEntitlements.Allocation = currentEntitlements.Allocation + vm.Allocation;
                    currentEntitlements.UpdatedByUserId = WebUser.Id;


                    _leaveEntitlementRepository.Update(currentEntitlements);
                    _unitOfWork.Commit();

                    return Json(true);
                }
                else
                {
                    // Create a log
                    var newLeaveEntitlementUpdate = new LeaveEntitlementUpdate
                    {
                        EmployeeId = vm.EmployeeId,
                        LeaveTimePeriodId = vm.LeaveTimePeriodId,
                        LeaveTypeId = vm.LeaveTypeId,
                        Operation = vm.Operation,
                        LeaveCount = vm.Allocation,
                        PreviousBalance = 0,
                        NewBalance = vm.Allocation,
                        Comments = vm.Comments,
                        CreatedByUserId = WebUser.Id
                    };

                    _leaveEntitlementUpdateRepository.Create(newLeaveEntitlementUpdate);
                    _unitOfWork.Commit();

                    // Create Entitlements
                    var newEntitlement = new LeaveEntitlement
                    {
                        EmployeeId = vm.EmployeeId,
                        LeaveTimePeriodId = vm.LeaveTimePeriodId,
                        LeaveTypeId = vm.LeaveTypeId,
                        Allocation = vm.Allocation,
                        Comments = vm.Comments,
                        CreatedByUserId = WebUser.Id
                    };

                    _leaveEntitlementRepository.Create(newEntitlement);
                    _unitOfWork.Commit();

                    return Json(true);
                }
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _leaveEntitlementRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Leave Entitlements deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
