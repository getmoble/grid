function EmergencyContact(data) {
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
    self.relationship = ko.observable(data.Relationship).extend({ required: { params: true, message: messages.general.Select } });
    self.relationshipType = ko.observable();
    self.mobile = ko.observable(data.Mobile).extend({ required: { params: true, message: messages.general.Mobile } });
    self.phone = ko.observable(data.Phone);
    self.workPhone = ko.observable(data.WorkPhone);
    self.address = ko.observable(data.Address);
    self.email = ko.observable(data.Email);  
    self.employeeId = ko.observable(data.EmployeeId);
    
    
    if (data.Relationship == 0) {
        self.relationshipType("Parent");
    }
    else if (data.Relationship == 1) {
        self.relationshipType("Sibling");
    }
    else if (data.Relationship == 2) {
        self.relationshipType("Friend");
    }
    else if (data.Relationship == 3) {
        self.relationshipType("Spouse");
    }

    self.modelState = ko.validatedObservable(
{
    Name: self.name,   
    Relationship: self.relationship,
    Mobile: self.mobile
});
    self.resetValidation = function () {
        self.name.isModified(false);      
        self.relationship.isModified(false);
        self.mobile.isModified(false);
    };
};
