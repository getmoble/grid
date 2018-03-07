function Project(data) {
    var self = ModelBase(this);
    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };


    self.id = ko.observable(data.Id || 0);
    self.name = ko.observable(data.Name);
    self.title = ko.observable(data.Title).extend({ required: { params: true, message: messages.general.Title } });
    self.description = ko.observable(data.Description);
    self.inheritMembers = ko.observable(data.InheritMembers);
    self.isPublic = ko.observable(data.IsPublic);
    self.startDate = ko.observable(self.parseDate(data.StartDate) || "");
    self.endDate = ko.observable(self.parseDate(data.EndDate) || "");
    self.clientId = ko.observable(data.ClientId).extend({ required: { params: true, message: messages.general.Select } });
    self.client = ko.observable();
    self.parentId = ko.observable(data.ParentId);
    self.parentProject = ko.observable();
    self.status = ko.observable(data.Status).extend({ required: { params: true, message: messages.general.Select } });
    self.projectType = ko.observable(data.ProjectType).extend({ required: { params: true, message: messages.general.Select } });
    self.projectStatus = ko.observable();
    self.type = ko.observable();
    self.technologyIds = ko.observableArray();
    self.technology = ko.observable(data.Technology);
    self.technologyNames = ko.observableArray();
    self.isVisible = ko.observable(false);


    self.parentId.subscribe(function (newValue) {
        if (newValue != undefined || newValue != "") {
            self.isVisible(true);
        }
        if (newValue ==  undefined || newValue == "") {
            self.isVisible(false);
        }
    });

    if (data.TechnologyIds) {
        self.technologyIds.pushAll(data.TechnologyIds);
    }

    if (data.Status == 0) {
        self.projectStatus("Discussion");
    }
    else if (data.Status == 1) {
        self.projectStatus("Design");
    } else if (data.Status == 2) {
        self.projectStatus("Implementation");
    }
    else if (data.Status == 3) {
        self.projectStatus("Testing");
    } else if (data.Status == 4) {
        self.projectStatus("WithHeld");
    }
    else if (data.Status == 5) {
        self.projectStatus("Cancelled");
    } else if (data.Status ==6) {
        self.projectStatus("Closed");
    }

    if (data.ProjectType == 0) {
        self.type("Internal");
    }else if (data.ProjectType == 1) {
        self.type("Client");
    } else if (data.ProjectType == 2) {
        self.type("Research");
    }
    else if (data.ProjectType == 3) {
        self.type("OffHours");
    }

    self.modelState = ko.validatedObservable(
{
    ClientId: self.clientId,
    Title: self.title,
    Status: self.status,
    ProjectType: self.projectType,
});
    self.resetValidation = function () {
        self.title.isModified(false);
        self.clientId.isModified(false);
        self.status.isModified(false);
        self.projectType.isModified(false);
    };
};