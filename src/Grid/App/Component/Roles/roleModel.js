function Roles(data) {
    var self = ModelBase(this);

    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };
    self.id = ko.observable(data.Id || 0);
    self.permissionIds = ko.observableArray();
    self.name = ko.observable(data.Name).extend({ required: { params: true, message: messages.general.Name } });
    self.description = ko.observable(data.Description);
    self.permissions = ko.observable(); 

    if (data.PermissionIds) {
        self.permissionIds.pushAll(data.PermissionIds);
    }

    if (data.Permission)
        self.permissions(data.Permission.Title);

    self.modelState = ko.validatedObservable(
{
    Name: self.name,

});
    self.resetValidation = function () {
        self.name.isModified(false);
    };
};