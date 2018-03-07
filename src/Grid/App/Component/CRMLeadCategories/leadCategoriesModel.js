function LeadCategory(leadCategory) {
    var self = ModelBase(this);
    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };

    self.id = ko.observable(leadCategory.Id || 0);
    self.title = ko.observable(leadCategory.Title).extend({ required: { params: true, message: messages.holiday.Title } });
    self.description = ko.observable(leadCategory.Description);

    self.modelState = ko.validatedObservable(
    {
        Title: self.title,
    });
    self.resetValidation = function () {
        self.title.isModified(false);
    };
};
