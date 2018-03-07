function CRMContactsModel(crmContacts) {
    var self = ModelBase(this);
    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };

    self.id = ko.observable(crmContacts.Id || 0);
    self.name = ko.observable(crmContacts.Name);
    self.firstName = ko.observable(crmContacts.FirstName);
    self.lastName = ko.observable(crmContacts.LastName);
    self.email = ko.observable(crmContacts.Email).extend({ email: { message: messages.general.ValidMail, params: true } });
    self.genderType = ko.observable(crmContacts.GenderType);
    self.gender = ko.observable(crmContacts.Gender);
    self.secondaryEmail = ko.observable(crmContacts.SecondaryEmail).extend({ email: { message: messages.general.ValidMail, params: true } });
    self.phoneNo = ko.observable(crmContacts.PhoneNo);
    self.officePhone = ko.observable(crmContacts.OfficePhone);
    self.website = ko.observable(crmContacts.Website);
    self.skype = ko.observable(crmContacts.Skype);
    self.facebook = ko.observable(crmContacts.Facebook);
    self.twitter = ko.observable(crmContacts.Twitter);
    self.googlePlus = ko.observable(crmContacts.GooglePlus);
    self.linkedIn = ko.observable(crmContacts.LinkedIn);
    self.organization = ko.observable(crmContacts.Organization);
    self.designation = ko.observable(crmContacts.Designation);
    self.city = ko.observable(crmContacts.City);
    self.country = ko.observable(crmContacts.Country);
    self.address = ko.observable(crmContacts.Address);
    self.communicationAddress = ko.observable(crmContacts.CommunicationAddress);
    self.expertise = ko.observable(crmContacts.Expertise);
    self.comments = ko.observable(crmContacts.Comments);
    self.parentAccountId = ko.observable(crmContacts.ParentAccountId);
    self.parentAccount = ko.observable(crmContacts.ParentAccount);
    self.personId = ko.observable(crmContacts.PersonId);
    self.person = ko.observable(crmContacts.Person);
    self.dateOfBirth = ko.observable(self.parseDate(crmContacts.DateOfBirth) || "");


    if (crmContacts.Gender == 0) {
        self.genderType("Male");
    }
    else if (crmContacts.Gender == 1) {
        self.genderType("Female");
    }
   
  



    self.modelState = ko.validatedObservable(
    {
       // FirstName: self.firstName,
    });
    self.resetValidation = function () {
        //self.title.isModified(false);
    };
};
