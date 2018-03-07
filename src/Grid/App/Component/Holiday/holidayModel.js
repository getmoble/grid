function Holiday(holiday) {
    var self = ModelBase(this);
    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };

    self.id = ko.observable(holiday.Id || 0);
    self.title = ko.observable(holiday.Title).extend({ required: { params: true, message: messages.holiday.Title } });
    self.type = ko.observable(holiday.Type).extend({ required: { params: true, message: messages.holiday.Type } });
    self.description = ko.observable(holiday.Description);
  
    self.date = ko.observable((self.parseDate(holiday.Date)) || "").extend({ required: { params: true, message: "Please enter Date" } });

    self.holidayType = ko.observable(holiday.HolidayType);

    if (holiday.Type == 0) {
        self.holidayType("Public");
    }
    else {
        self.holidayType("Restricted");
    }
    self.modelState = ko.validatedObservable(
    {
        Title: self.title,
        Type: self.type,
        Date: self.date,
    });
    self.resetValidation = function () {
        self.title.isModified(false);
        self.type.isModified(false);
        self.date.isModified(false);
    };
};
