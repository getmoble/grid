using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;
using Grid.Features.HRMS.Entities;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Api.Models.CRM;

namespace Grid.Api.Controllers
{
    public class ContactsController : GridApiBaseController
    {
        private readonly ICRMContactRepository _crmContactRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ContactsController(ICRMContactRepository crmContactRepository, IPersonRepository personRepository,
                                   IUnitOfWork unitOfWork)
        {
            _crmContactRepository = crmContactRepository;
            _personRepository = personRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _crmContactRepository.GetAll(), "Contacts Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

       
        [HttpGet]
        public JsonResult Get(int id)
        {
            var apiResult = TryExecute(() =>
            {
                var contact = _crmContactRepository.Get(id, "ParentAccount,Person");
                var contactVm = new CRMContactModel(contact);
                return contactVm;
            }, "Contacts Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(CRMContactModel cRMContact)
        {
            ApiResult<CRMContact> apiResult;

            if (ModelState.IsValid)
            {
                if (cRMContact.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {

                        var person = _personRepository.GetBy(l => l.Id == cRMContact.PersonId);
                        if(person != null)
                        {
                            person.Id = cRMContact.PersonId;
                            person.FirstName = cRMContact.FirstName;
                            person.LastName = cRMContact.LastName;
                            person.Gender = cRMContact.Gender;
                            person.Email = cRMContact.Email;
                            person.Organization = cRMContact.Organization;
                            person.Designation = cRMContact.Designation;
                            person.PhoneNo = cRMContact.PhoneNo;
                            person.SecondaryEmail = cRMContact.SecondaryEmail;
                            person.OfficePhone = cRMContact.OfficePhone;
                            person.Website = cRMContact.Website;
                            person.Skype = cRMContact.Skype;
                            person.Facebook = cRMContact.Facebook;
                            person.Twitter = cRMContact.Twitter;
                            person.GooglePlus = cRMContact.GooglePlus;
                            person.LinkedIn = cRMContact.LinkedIn;
                            person.City = cRMContact.City;
                            person.Country = cRMContact.Country;
                            person.Address = cRMContact.Address;
                            person.CommunicationAddress = cRMContact.CommunicationAddress;
                            person.DateOfBirth = cRMContact.DateOfBirth;
                            _personRepository.Update(person);
                            _unitOfWork.Commit();
                        }
                        

                        var selectedContact = _crmContactRepository.GetBy(l => l.Id == cRMContact.Id);
                        if (selectedContact != null)
                        {
                            selectedContact.ParentAccountId = cRMContact.ParentAccountId;
                            selectedContact.Expertise = cRMContact.Expertise;
                            selectedContact.Comments = cRMContact.Comments;
                            selectedContact.UpdatedByUserId = WebUser.Id;

                            _crmContactRepository.Update(selectedContact);
                            _unitOfWork.Commit();
                        }
                        return selectedContact;
                    }, "Contact updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        var newPerson = new Person
                        {
                            FirstName = cRMContact.FirstName,
                            LastName = cRMContact.LastName,
                            Gender = cRMContact.Gender,
                            Email = cRMContact.Email,
                            Organization = cRMContact.Organization,
                            Designation = cRMContact.Designation,
                            PhoneNo = cRMContact.PhoneNo,
                            SecondaryEmail = cRMContact.SecondaryEmail,
                            OfficePhone = cRMContact.OfficePhone,
                            Website = cRMContact.Website,
                            Skype = cRMContact.Skype,
                            Facebook = cRMContact.Facebook,
                            Twitter = cRMContact.Twitter,
                            GooglePlus = cRMContact.GooglePlus,
                            LinkedIn = cRMContact.LinkedIn,
                            City = cRMContact.City,
                            Country = cRMContact.Country,
                            Address = cRMContact.Address,
                            CommunicationAddress = cRMContact.CommunicationAddress,
                            DateOfBirth = cRMContact.DateOfBirth,
                        };
                        var person = _personRepository.Create(newPerson);


                        var newContact = new CRMContact
                        {
                            PersonId = person.Id,
                            ParentAccountId = cRMContact.ParentAccountId,
                            Expertise = cRMContact.Expertise,
                            Comments = cRMContact.Comments,
                            CreatedByUserId = WebUser.Id
                        };
                        var contact = _crmContactRepository.Create(newContact);
                        _unitOfWork.Commit();
                        return newContact;
                    }, "Contact created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<CRMContact>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _crmContactRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Contact deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
