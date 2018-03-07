function EntitlementHistory(data) {
    var self = ModelBase(this);

    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };

    self.id = ko.observable(data.Id || 0);
    self.employeeId = ko.observable(data.EmployeeId);
    self.employee = ko.observable(data.Employee);
    self.createdByUserId = ko.observable(data.CreatedByUserId);
    self.createdByUser = ko.observable(data.CreatedByUser);
    self.leaveTimePeriodId = ko.observable(data.LeaveTimePeriodId);
    self.leaveTimePeriod = ko.observable(data.LeaveTimePeriod);
    self.leaveTypeId = ko.observable(data.LeaveTypeId);
    self.leaveType = ko.observable(data.LeaveType);
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
        self.leaveType(data.LeaveTimePeriod.Title);
    }
    if (data.LeaveType) {
        self.leaveTimePeriod(data.LeaveType.Title);
    }

    self.modelState = ko.validatedObservable(
 {

 });
    self.resetValidation = function () {        
    };
};
