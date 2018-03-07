function Software(data) {
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
    self.version = ko.observable(data.Version).extend({ required: { params: true, message: messages.software.Version } });
    self.latestVersion = ko.observable(data.LatestVersion);
    self.recommendedVersion = ko.observable(data.RecommendedVersion);
    self.statusType = ko.observable();
    self.status = ko.observable(data.Status).extend({ required: { params: true, message: messages.software.Status } });
    self.licenseType = ko.observable(data.LicenseType).extend({ required: { params: true, message: messages.software.LicenseType } });
    self.license = ko.observable();
    self.softwareCategoryId = ko.observable(data.SoftwareCategoryId).extend({ required: { params: true, message: messages.software.SoftwareCategoryId } });
    self.category = ko.observable();

   
        if (data.Category) {
            self.category(data.Category.Title);
        }
    

    if (data.Status == 0) {
        self.statusType("Allowed");
    }
    else if (data.Status == 1) {
        self.statusType("NotAllowed");
    } else if (data.Status == 2) {
        self.statusType("Unknown");
    }

    if (data.LicenseType == 0) {
        self.license("Freeware");
    }
    else if (data.LicenseType == 1) {
        self.license("Licensed");
    } else if (data.LicenseType == 2) {
        self.license("Unknown");
    }

    self.modelState = ko.validatedObservable(
{
    Title: self.title,
    Version: self.version,
    SoftwareCategoryId: self.softwareCategoryId,
    Status: self.status,
    LicenseType: self.licenseType,
});
    self.resetValidation = function () {
        self.title.isModified(false);
        self.version.isModified(false);
        self.softwareCategoryId.isModified(false);
        self.status.isModified(false);
        self.licenseType.isModified(false);
    };
};


