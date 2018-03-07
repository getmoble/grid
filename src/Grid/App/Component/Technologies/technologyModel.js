function Technology(data) {
    var self = ModelBase(this);

    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };
    self.id = ko.observable(data.Id || 0);
    self.category = ko.observable(data.Category).extend({ required: { params: true, message: messages.technology.Category } });
    self.icon = ko.observable(data.Icon);
    self.title = ko.observable(data.Title).extend({ required: { params: true, message: messages.general.Title  } });
    self.description = ko.observable(data.Description); 

    self.modelState = ko.validatedObservable(
{
    Title: self.title,
    Category: self.category,
});
    self.resetValidation = function () {
        self.title.isModified(false);
        self.category.isModified(false);
       
    };
};