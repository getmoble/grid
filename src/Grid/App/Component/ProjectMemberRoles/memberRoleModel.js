function MemberRole(data) {
    var self = ModelBase(this);

    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };
    self.id = ko.observable(data.Id || 0);
    self.title = ko.observable(data.Title).extend({ required: { params: true, message: messages.general.Name } });
    self.description = ko.observable(data.Description);
    self.departmentId = ko.observable(data.DepartmentId).extend({ required: { params: true, message: messages.general.Select } });
    self.department = ko.observable();
    self.role = ko.observable(data.Role).extend({ required: { params: true, message: messages.general.Select } });
    self.roleType = ko.observable();


    if (data.Department) {
        self.department(data.Department.Title);
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


    self.modelState = ko.validatedObservable(
{
    Title: self.title,
    Role: self.role,
    DepartmentId: self.departmentId,
});
    self.resetValidation = function () {
        self.title.isModified(false);
        self.departmentId.isModified(false);
        self.role.isModified(false);

    };
};


