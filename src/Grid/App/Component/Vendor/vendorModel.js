function Vendor(vendor) {
    var self = ModelBase(this);
    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };

    self.id = ko.observable(vendor.Id || 0);
    self.title = ko.observable(vendor.Title).extend({ required: { params: true, message: messages.general.Title } });
    self.email = ko.observable(vendor.Email).extend({ email: { message: messages.general.ValidMail, params: true } });
    self.phone = ko.observable(vendor.Phone);
    self.webSite = ko.observable(vendor.WebSite);
    self.address = ko.observable(vendor.Address);
    self.contactPerson = ko.observable(vendor.ContactPerson);
    self.contactPersonEmail = ko.observable(vendor.ContactPersonEmail);
    self.contactPersonPhone = ko.observable(vendor.ContactPersonPhone);
    self.description = ko.observable(vendor.Description);

    self.modelState = ko.validatedObservable(
    {
        Title: self.title,
        ValidMail: self.email
    });

    self.resetValidation = function () {
        self.title.isModified(false);
        self.email.isModified(false);
    };
};
