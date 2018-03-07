function ProjectMember(data) {
    var self = ModelBase(this);
    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };


    self.id = ko.observable(data.Id || 0);
    self.title = ko.observable(data.Title);
    self.employeeId = ko.observable(data.EmployeeId).extend({ required: { params: true, message: messages.general.Select } });
    self.memberEmployee = ko.observable(data.MemberEmployee);
    self.role = ko.observable(data.Role).extend({ required: { params: true, message: messages.general.Select } });
    self.billing = ko.observable(data.Billing).extend({ required: { params: true, message: messages.general.Select } });
    self.memberStatus = ko.observable(data.MemberStatus).extend({ required: { params: true, message: messages.general.Select } });
    self.memberStatusType = ko.observable();
    self.rate = ko.observable(data.Rate);
    self.projectId = ko.observable(data.ProjectId);
    self.roleType = ko.observable();
    self.billingType = ko.observable();
    self.date = ko.observable(self.parseDate(data.CreatedOn));

    if (data.MemberEmployee) {
        if (data.MemberEmployee.User) {
            if (data.MemberEmployee.User.Person) {                
                self.memberEmployee(data.MemberEmployee.User.Person.Name);
            }
        }
    }

    if (data.MemberStatus == 0) {
        self.memberStatusType("Active");
    }
    else if (data.MemberStatus == 1) {
        self.memberStatusType("InActive");
    }


    if (data.Role == 0) {
        self.roleType("Developer");
    }
    else if (data.Role == 1) {
        self.roleType("Lead");
    } else if (data.Role == 2) {
        self.roleType("Tester");
    }
    else if (data.Role == 3) {
        self.roleType("ProjectManager");
    } else if (data.Role == 4) {
        self.roleType("Sales");
    }
    else if (data.Role == 5) {
        self.roleType("Designer");
    }


    if (data.Billing == 0) {
        self.billingType("NonBillable");
    }
    else if (data.Billing == 1) {
        self.billingType("Billable");
    } 


    self.modelState = ko.validatedObservable(
{
    EmployeeId: self.employeeId,
    Role: self.role,
    MemberStatus: self.memberStatus,
    Billing: self.billing
});
    self.resetValidation = function () {
        self.employeeId.isModified(false);
        self.role.isModified(false);
        self.memberStatus.isModified(false);
        self.billing.isModified(false);
    };
};