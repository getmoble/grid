function Skill(data) {
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

   
    self.modelState = ko.validatedObservable(
{   
    Title: self.title,    
});
    self.resetValidation = function () {
        self.title.isModified(false);        
    };
};
