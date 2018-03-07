function TicketSubCategory(data) {
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
    self.ticketCategoryId = ko.observable(data.TicketCategoryId).extend({ required: { params: true, message: messages.general.Select } });
    self.ticketCategory = ko.observable();

    if (data.TicketCategory) {
        self.ticketCategory(data.TicketCategory.Title);
    }
  
    self.modelState = ko.validatedObservable(
{
    Title: self.title,
    Select: self.ticketCategoryId
});
    self.resetValidation = function () {
        self.title.isModified(false);
        self.ticketCategoryId.isModified(false);
    };
};
