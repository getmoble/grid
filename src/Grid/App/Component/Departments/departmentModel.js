function Departments(data) {
    var self = ModelBase(this);

    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };
    self.id = ko.observable(data.Id || 0);
    self.parentId = ko.observable(data.ParentId);
    self.parent = ko.observable(null);
    self.title = ko.observable(data.Title).extend({ required: { params: true, message: messages.department.Title } });
    self.mailAlias = ko.observable(data.MailAlias).extend({ email: { message: messages.department.MailAlias, params: true } });
    self.description = ko.observable(data.Description);

    if (data.Parent)
        self.parent(data.Parent.Title);

    self.modelState = ko.validatedObservable(
{   
    Title: self.title,
    MailAlias: self.mailAlias
});
    self.resetValidation = function () {
        self.title.isModified(false);
        self.mailAlias.isModified(false);
    };
};
