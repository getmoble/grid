function LeaveEntitlementUpdate(data) {
    var self = ModelBase(this);

    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };

    self.id = ko.observable(data.Id || 0);
    self.employeeId = ko.observable(data.EmployeeId).extend({ required: { params: true, message: messages.general.Select } });
    self.employee = ko.observable(data.Employee);
    self.createdByUserId = ko.observable(data.CreatedByUserId);
    self.createdByUser = ko.observable(data.CreatedByUser);
    self.leaveTimePeriodId = ko.observable(data.LeaveTimePeriodId).extend({ required: { params: true, message: messages.general.Select } });
    self.leaveTimePeriod = ko.observable(data.LeaveTimePeriod);
    self.leaveTypeId = ko.observable(data.LeaveTypeId).extend({ required: { params: true, message: messages.general.Select } });
    self.leaveType = ko.observable(data.LeaveType);
    self.allocation = ko.observable(data.Allocation).extend({ required: { params: true, message: messages.leaveEntitlements.Allocation } });
    self.comments = ko.observable(data.Comments);

    self.operationType = ko.observable();
    self.operation = ko.observable(data.Operation);
    self.leaveId = ko.observable(data.LeaveId);
    self.leave = ko.observable(data.Leave);
    self.leaveCount = ko.observable(data.LeaveCount);
    self.previousBalance = ko.observable(data.PreviousBalance);
    self.newBalance = ko.observable(data.NewBalance);

    if (data.Operation == "0") {
        self.operationType("Allocate");
    } else if (data.Operation == "1") {
        self.operationType("Deduct");
    }

    if (data.LeaveTimePeriod) {
        self.leaveTimePeriod(data.LeaveTimePeriod.Title);
    }
    if (data.LeaveType) {
        self.leaveType(data.LeaveType.Title);
    }
    if (data.Employee) {
        if (data.Employee.User) {
            if (data.Employee.User.Person) {
                self.employee(data.Employee.User.Person.Name)
            }
        }
      
    }

    self.modelState = ko.validatedObservable(
 {
     EmployeeId: self.employeeId,
     LeaveTimePeriodId: self.leaveTimePeriodId,
     LeaveTypeId: self.leaveTypeId,
     Allocation: self.allocation,

 });
    self.resetValidation = function () {
        self.employeeId.isModified(false);
        self.leaveTimePeriodId.isModified(false);
        self.leaveTypeId.isModified(false);
        self.allocation.isModified(false);
    };
};