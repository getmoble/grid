using Grid.Api.Models;
using Grid.Api.Models.HRMS;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;
using System.Linq;
using System.Web.Mvc;

namespace Grid.Api.Controllers
{
    public class EmergencyContactsController : GridApiBaseController
    {
        private readonly IEmergencyContactRepository _emergencyContactRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmergencyContactsController(IEmergencyContactRepository emergencyContactRepository,
                                           IUnitOfWork unitOfWork)
        {
            _emergencyContactRepository = emergencyContactRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() =>
            {
                return _emergencyContactRepository.GetAll().Select(h => new EmergencyContactModel(h)).ToList();
            }, "Employee Dependents Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]

        public JsonResult Get(int id)
        {
            var apiResult = TryExecute(() => _emergencyContactRepository.Get(id), "Emergency Contacts Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Update(EmergencyContactModel vm)
        {
            ApiResult<EmergencyContact> apiResult;

            if (ModelState.IsValid)
            {
                if (vm.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var emergencyContact = new EmergencyContact
                        {

                            Name = vm.Name,
                            Relationship = vm.Relationship,
                            Phone = vm.Phone,
                            EmployeeId = vm.EmployeeId,
                            Mobile = vm.Mobile,
                            WorkPhone = vm.WorkPhone,
                            Address = vm.Address,                       
                            Email = vm.Email,                         
                            Id = vm.Id,
                        };
                        _emergencyContactRepository.Update(emergencyContact);
                        _unitOfWork.Commit();
                        return emergencyContact;
                    }, "Emergency Contact updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        var emergencyContact = new EmergencyContact
                        {

                            Name = vm.Name,
                            Relationship = vm.Relationship,
                            Phone = vm.Phone,
                            EmployeeId = vm.EmployeeId,
                            Mobile = vm.Mobile,
                            WorkPhone = vm.WorkPhone,
                            Address = vm.Address,
                            Email = vm.Email,                         
                            Id = vm.Id
                        };
                        _emergencyContactRepository.Create(emergencyContact);
                        _unitOfWork.Commit();
                        return emergencyContact;
                    }, "Emergency Contact created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<EmergencyContact>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _emergencyContactRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Emergency Contact deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }


    }
}
