function LeaveTimePeriod(leaveTimePeriod) {
    var self = ModelBase(this);

    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };
    self.id = ko.observable(leaveTimePeriod.Id || 0);
    self.title = ko.observable(leaveTimePeriod.Title).extend({ required: { params: true, message: messages.leavePeriod.Title } });
    self.start = ko.observable(self.parseDate(leaveTimePeriod.Start) || "").extend({ required: { params: true, message: "Please enter Start Date" } });
    self.end = ko.observable(self.parseDate(leaveTimePeriod.End) || "").extend({ required: { params: true, message: "Please enter End Date" } });
    self.description = ko.observable(leaveTimePeriod.Description);

    self.modelState = ko.validatedObservable(
{
    Title: self.title,
    Start: self.start,
    End: self.end,
});
    self.resetValidation = function () {
        self.title.isModified(false);
        self.start.isModified(false);
        self.end.isModified(false);
    };
};
