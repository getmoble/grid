function LeaveTypes(leaveTypes) {
    var self = ModelBase(this);

    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };
    self.id = ko.observable(leaveTypes.Id || 0);
    self.title = ko.observable(leaveTypes.Title).extend({ required: { params: true, message: messages.leaveType.Title } });
    self.description = ko.observable(leaveTypes.Description);
    self.maxInAStretch = ko.observable(leaveTypes.MaxInAStretch).extend({ required: { params: true, message: messages.leaveType.MaxInAStretch } });
    self.maxInMonth = ko.observable(leaveTypes.MaxInMonth).extend({ required: { params: true, message: messages.leaveType.MaxInMonth } });
    self.canCarryForward = ko.observable(leaveTypes.CanCarryForward);
    self.maxCarryForward = ko.observable(leaveTypes.MaxCarryForward).extend({ required: { params: true, message: messages.leaveType.MaxCarryForward } });

    self.modelState = ko.validatedObservable(
{
    Title: self.title,
    MaxInAStretch: self.maxInAStretch,
    MaxInMonth: self.maxInMonth,
    MaxCarryForward: self.maxCarryForward,
});
    self.resetValidation = function () {
        self.title.isModified(false);
        self.maxInAStretch.isModified(false);
        self.maxInMonth.isModified(false);
        self.maxCarryForward.isModified(false);
    };
};