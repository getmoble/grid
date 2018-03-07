function SalesStage(salesStage) {
    var self = ModelBase(this);
    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };

    self.id = ko.observable(salesStage.Id || 0);
    self.name = ko.observable(salesStage.Name).extend({ required: { params: true, message: messages.general.Name } });
    self.status = ko.observable(salesStage.Status).extend({ required: { params: true, message: messages.general.Select } });
    self.statusType = ko.observable();



    if (salesStage.Status == 0) {
        self.statusType("Prospecting");
    }
    else if (salesStage.Status == 1) {
        self.statusType("Won");
    }
    else if (salesStage.Status == 2) {
        self.statusType("Lost");
    } else if (salesStage.Status == 3) {
        self.statusType("Dead");
    }

    self.modelState = ko.validatedObservable(
    {
        Name: self.name,
        Status: self.status,

    });
    self.resetValidation = function () {
        self.name.isModified(false);
        self.status.isModified(false);

    };
};
