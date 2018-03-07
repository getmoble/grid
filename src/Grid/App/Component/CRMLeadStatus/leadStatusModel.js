function LeadStatus(leadStatus) {
    var self = ModelBase(this);
    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };

    self.id = ko.observable(leadStatus.Id || 0);
    self.name = ko.observable(leadStatus.Name).extend({ required: { params: true, message: messages.general.Name } });

    self.modelState = ko.validatedObservable(
    {
        Name: self.name,
    });
    self.resetValidation = function () {
        self.name.isModified(false);
    };
};
