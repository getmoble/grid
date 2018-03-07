function Employee(data) {
    var self = ModelBase(this);

    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };

    self.id = ko.observable(data.Id || 0);
    self.employeeId = ko.observable(data.EmployeeId)

    self.employeeCode = ko.observable(data.EmployeeCode).extend({ required: { params: true, message: messages.employee.EmployeeCode }, maxLength: 50 });
    self.username = ko.observable(data.Username).extend({ required: { params: true, message: messages.employee.Username }, maxLength: 254 });
    self.password = ko.observable(data.Password).extend({ required: { params: true, message: messages.employee.Password } });
    self.newPassword = ko.observable().extend({ required: { onlyIf: function () { return self.password() === false; } } });
    self.confirmNewPassword = ko.observable().extend({ required: { onlyIf: function () { return self.password() === false; } } });
    self.isVisible = ko.observable(false);  
    self.firstName = ko.observable(data.FirstName).extend({ required: { params: true, message: messages.employee.FirstName } });
    self.middleName = ko.observable(data.MiddleName);
    self.lastName = ko.observable(data.LastName);
    self.phoneNo = ko.observable(data.PhoneNo);
    self.address = ko.observable(data.Address);
    self.communicationAddress = ko.observable(data.CommunicationAddress);
    self.passportNo = ko.observable(data.PassportNo);
    self.secondaryEmail = ko.observable(data.SecondaryEmail);
    self.email = ko.observable(data.Email).extend({ required:{ params: true, message: messages.general.Mail } });
    self.dateOfBirth = ko.observable(self.parseDate(data.DateOfBirth) || "");
    self.marriageAnniversary = ko.observable(self.parseDate(data.MarriageAnniversary) || "");
    self.gender = ko.observable(data.Gender);
    self.bloodGroup = ko.observable(data.BloodGroup);
    self.maritalStatus = ko.observable(data.MaritalStatus);
    self.genderType = ko.observable(data.GenderType);
    self.bloodGroupType = ko.observable(data.BloodGroupType);
    self.maritalStatusType = ko.observable(data.MaritalStatusType);
    self.designationId = ko.observable(data.DesignationId).extend({ required: { params: true, message: messages.general.Select } });
    self.designation = ko.observable(data.Designation);
    self.departmentId = ko.observable(data.DepartmentId).extend({ required: { params: true, message: messages.general.Select } });
    self.department = ko.observable(data.Department);
    self.locationId = ko.observable(data.LocationId).extend({ required: { params: true, message: messages.general.Select } });
    self.location = ko.observable(data.Location);
    self.shiftId = ko.observable(data.ShiftId);
    self.shift = ko.observable(data.Shift);
    self.reportingPerson = ko.observable(data.ReportingPerson);
    self.manager = ko.observable(data.Manager);
    self.reportingPersonId = ko.observable(data.ReportingPersonId);
    self.managerId = ko.observable(data.ManagerId);
    self.requiresTimeSheet = ko.observable(data.RequiresTimeSheet);
    self.salary = ko.observable(data.Salary);
    self.bank = ko.observable(data.Bank);
    self.panCard = ko.observable(data.PANCard);
    self.bankAccountNumber = ko.observable(data.BankAccountNumber);
    self.seatNo = ko.observable(data.SeatNo);
    self.dateOfJoin = ko.observable(self.parseDate(data.DateOfJoin) || "").extend({ required: { params: true, message: "Please enter Date Of Join" } });
    self.confirmationDate = ko.observable(self.parseDate(data.ConfirmationDate) ||"");
    self.dateOfResignation = ko.observable(self.parseDate(data.DateOfResignation) || "");
    self.lastDate = ko.observable(self.parseDate(data.LastDate) || "");
    self.officialEmail = ko.observable(data.OfficialEmail).extend({ required: { params: true, message: "Please enter Official Email" } });
    self.officialPhone = ko.observable(data.OfficialPhone);
    self.officialMessengerId = ko.observable(data.OfficialMessengerId);
    self.experience = ko.observable(data.Experience);
    self.paymentMode = ko.observable(data.PaymentMode);
    self.paymentType = ko.observable();
    self.employeeStatus = ko.observable(data.EmployeeStatus).extend({ required: { params: true, message: messages.general.Select } });
    self.userId = ko.observable(data.UserId);
    self.personId = ko.observable(data.PersonId);
    self.status = ko.observable();
    self.accessRuleId = ko.observable(data.AccessRuleId);
    self.photoPath = ko.observable(data.PhotoPath);  
    self.technologyIds = ko.observableArray();
    self.technology = ko.observable(data.Technology);
    self.technologyNames = ko.observableArray();
    self.roleNames = ko.observableArray();
    self.userTechnologyNames = ko.observable(data.UserTechnologyNames);
    self.userRoleNames = ko.observable(data.UserRoleNames);
    self.roleIds = ko.observableArray();
    self.role = ko.observable();

    self.button1Visible = ko.observable(false);
    self.button2Visible = ko.observable(false);
    self.button3Visible = ko.observable(false);

    self.toggle1 = function () {
        self.button1Visible(!self.button1Visible());
    }

    self.toggle2 = function () {
        self.button2Visible(!self.button2Visible());
    }

    self.toggle3 = function () {
        self.button3Visible(!self.button3Visible());
    }

    if (data.TechnologyIds) {
        self.technologyIds.pushAll(data.TechnologyIds);
    }    
    if (data.RoleIds) {
        self.roleIds.pushAll(data.RoleIds);
    }

   
    if (data.Gender == 0) {
        self.genderType("Male");
    }
    else if (data.Gender == 1) {
        self.genderType("Female");
    }

    if (data.BloodGroup == 0) {
        self.bloodGroupType("OPositive");
    }
    else if (data.BloodGroup == 1) {
        self.bloodGroupType("ONegative");
    }
    else if (data.BloodGroup == 2) {
        self.bloodGroupType("APositive");
    }
    if (data.BloodGroup == 3) {
        self.bloodGroupType("ANegative");
    }
    else if (data.BloodGroup == 4) {
        self.bloodGroupType("BPositive");
    }
    else if (data.BloodGroup == 5) {
        self.bloodGroupType("BNegative");
    }
    else if (data.BloodGroup == 6) {
        self.bloodGroupType("ABPositive");
    }
    else if (data.BloodGroup == 7) {
        self.bloodGroupType("ABNegative");
    }

    if (data.MaritalStatus == 0) {
        self.maritalStatusType("Single");
    }
    else if (data.MaritalStatus == 1) {
        self.maritalStatusType("Married");
    }

    if (data.EmployeeStatus == 0) {
        self.status("Offered");
    }
    else if (data.EmployeeStatus == 1) {
        self.status("Joined");
    }
    else if (data.EmployeeStatus == 2) {
        self.status("Probation");
    }
    if (data.EmployeeStatus == 3) {
        self.status("Confirmed");
    }
    else if (data.EmployeeStatus == 4) {
        self.status("Resigned");
    }
    else if (data.EmployeeStatus == 5) {
        self.status("Ex");
    }
    else if (data.EmployeeStatus == 6) {
        self.status("OnContract");
    }

    if (data.PaymentMode == 0) {
        self.paymentType("AccountTransfer");
    }
    else if (data.PaymentMode == 1) {
        self.paymentType("Cash");
    }
    else if (data.PaymentMode == 2) {
        self.paymentType("Cheque");
    }

    self.modelState = ko.validatedObservable(
    {
        EmployeeCode: self.employeeCode,
        Password: self.password,
        Username: self.username,
        NewPassword: self.newPassword,
        ConfirmNewPassword: self.confirmNewPassword,
        FirstName: self.firstName,       
        DesignationId: self.designationId,
        DepartmentId: self.departmentId,
        Email: self.email,
        DateOfJoin: self.dateOfJoin,
        LocationId: self.locationId,
        EmployeeStatus: self.employeeStatus,
        OfficialEmail: self.officialEmail
      
    });
    self.resetValidation = function () {
        self.employeeCode.isModified(false);
        self.username.isModified(false);
        self.password.isModified(false);
        self.newPassword.isModified(false);
        self.confirmNewPassword.isModified(false);
        self.firstName.isModified(false);        
        self.departmentId.isModified(false);
        self.designationId.isModified(false);
        self.email.isModified(false);
        self.dateOfJoin.isModified(false);
        self.locationId.isModified(false);
        self.employeeStatus.isModified(false);
        self.officialEmail.isModified(false);
    };
};