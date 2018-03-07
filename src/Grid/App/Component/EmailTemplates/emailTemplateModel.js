function EmailTemplate(data) {
    var self = ModelBase(this);

    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };
    self.id = ko.observable(data.Id || 0);
    self.name = ko.observable(data.Name).extend({ required: { params: true, message: messages.general.Name } });
    self.content = ko.observable(data.Content).extend({ required: { params: true, message: messages.emailTemplate.Content } });

    self.modelState = ko.validatedObservable(
{
    Name: self.name,
    Content:self.content,
});
    self.resetValidation = function () {
        self.name.isModified(false);
        self.content.isModified(false);
    };
};
