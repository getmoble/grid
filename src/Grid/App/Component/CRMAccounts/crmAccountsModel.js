function CRMAccountsModel(crmAccounts) {
    var self = ModelBase(this);
    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };

    self.id = ko.observable(crmAccounts.Id || 0);
    self.title = ko.observable(crmAccounts.Title).extend({ required: { params: true, message: messages.holiday.Title } });
    self.industry = ko.observable(crmAccounts.Industry);
    self.employeeCount = ko.observable(crmAccounts.EmployeeCount);
    self.employeeCountType = ko.observable();
    //self.foundedOn = ko.observable(crmAccounts.FoundedOn);

    self.foundedOn = ko.observable((self.parseDate(crmAccounts.FoundedOn)));


    self.email = ko.observable(crmAccounts.Email).extend({ email: { message: messages.general.ValidMail, params: true } });
    self.phoneNo = ko.observable(crmAccounts.PhoneNo);
    self.secondaryEmail = ko.observable(crmAccounts.SecondaryEmail);
    self.officePhone = ko.observable(crmAccounts.OfficePhone);
    self.website = ko.observable(crmAccounts.Website);
    self.facebook = ko.observable(crmAccounts.Facebook);
    self.twitter = ko.observable(crmAccounts.Twitter);
    self.googlePlus = ko.observable(crmAccounts.GooglePlus);
    self.linkedIn = ko.observable(crmAccounts.LinkedIn);
    self.city = ko.observable(crmAccounts.City);
    self.country = ko.observable(crmAccounts.Country);
    self.address = ko.observable(crmAccounts.Address);
    self.communicationAddress = ko.observable(crmAccounts.CommunicationAddress);
    self.expertise = ko.observable(crmAccounts.Expertise);
    self.description = ko.observable(crmAccounts.Description);
    self.assignedToEmployeeId = ko.observable(crmAccounts.AssignedToEmployeeId);
    self.assignedToEmployee = ko.observable(crmAccounts.AssignedToEmployee);
    self.parentId = ko.observable(crmAccounts.ParentId);
    self.parent = ko.observable(crmAccounts.Parent);

    if (crmAccounts.EmployeeCount == 0) {
        self.employeeCountType("One2Ten")
    }
    else if (crmAccounts.EmployeeCount == 1) {
        self.employeeCountType("Ten2Hundred");
    }
    else if (crmAccounts.EmployeeCount == 2) {
        self.employeeCountType("Hundred2Thousand");
    }
    if (crmAccounts.EmployeeCount == 3) {
        self.employeeCountType("Thousand2TenThousand");
    }
    else if (crmAccounts.EmployeeCount == 4) {
        self.employeeCountType("TenThousandAbove");
    }
   
  
   



    self.modelState = ko.validatedObservable(
    {
        Title: self.title,
    });
    self.resetValidation = function () {
        self.title.isModified(false);
    };
};
