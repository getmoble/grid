function LeaveBalance(data) {
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

   
    self.modelState = ko.validatedObservable(
    {      
    });

    self.resetValidation = function () {       
    };
};