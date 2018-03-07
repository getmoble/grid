function Dependent(data) {
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
    self.gender = ko.observable(data.Gender).extend({ required: { params: true, message: messages.general.Select } });
    self.genderType = ko.observable();
    self.dependentType = ko.observable(data.DependentType).extend({ required: { params: true, message: messages.general.Select } });
    self.dependent = ko.observable();
    self.birthday = ko.observable(self.parseDate(data.Birthday) || new Date());
    self.employeeId = ko.observable(data.EmployeeId);
   

    if (data.Gender == 0) {
        self.genderType("Male");
    }
    else if (data.Gender == 1) {
        self.genderType("Female");
    }

    if (data.DependentType == 0) {
        self.dependent("Spouse");
    }
    else if (data.DependentType == 1) {
        self.dependent("Sibling");
    }
    else if (data.DependentType == 2) {
        self.dependent("Child");
    }
    else if (data.DependentType == 3) {
        self.dependent("Parent");
    }

    self.modelState = ko.validatedObservable(
{
    Name: self.name,
    Gender: self.gender,
    DependentType : self.dependentType 
});
    self.resetValidation = function () {
        self.name.isModified(false);
        self.gender.isModified(false);
        self.dependentType.isModified(false);
    };
};
