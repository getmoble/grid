function Shift(data) {
    var self = ModelBase(this);

    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };

    self.id = ko.observable(data.Id || 0);    
    self.title = ko.observable(data.Title).extend({ required: { params: true, message: messages.general.Title } });    
    self.description = ko.observable(data.Description);
    self.startTime = ko.observable(self.parseDate(data.StartTime) || "").extend({ required: { params: true, message: "Please enter Start Date" } });
    self.endTime = ko.observable(self.parseDate(data.EndTime) || "").extend({ required: { params: true, message: "Please enter Start Date" } });
    self.needsCompensation = ko.observable(data.NeedsCompensation);
    
    self.modelState = ko.validatedObservable(
{   
    Title: self.title,
    StartTime: self.startTime,
    EndTime: self.endTime,
});
    self.resetValidation = function () {
        self.title.isModified(false);
        self.startTime.isModified(false);
        self.endTime.isModified(false);
    };
};
