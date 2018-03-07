function Designations(data) {
    var self = ModelBase(this);

    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };
    self.id = ko.observable(data.Id || 0);
    self.departmentId = ko.observable(data.DepartmentId).extend({ required: { params: true, message: messages.designation.DepartmentId } });
    self.department = ko.observable();
    self.title = ko.observable(data.Title).extend({ required: { params: true, message: messages.designation.Title } });
    self.band = ko.observable(data.Band).extend({ required: { params: true, message: messages.designation.Band } });
    self.bandName = ko.observable();
    self.mailAlias = ko.observable(data.MailAlias).extend({ email: { message: messages.designation.MailAlias, params: true } });
    self.description = ko.observable(data.Description);
    if (data.Department)
        self.department(data.Department.Title);

    if (data.Band == 0) {
        self.bandName("Band1");
    }
    else if (data.Band == 1) {
        self.bandName("Band2");
    } else if (data.Band == 2) {
        self.bandName("Band3");
    }
    self.modelState = ko.validatedObservable(
{
    DepartmentId: self.departmentId,
    Title: self.title,
    Band: self.band,
    MailAlias: self.mailAlias

});
    self.resetValidation = function () {
        self.departmentId.isModified(false);
        self.title.isModified(false);
        self.band.isModified(false);
        self.mailAlias.isModified(false);
    };
};
