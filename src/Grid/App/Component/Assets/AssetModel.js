function Asset(data) {
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
    self.serialNumber = ko.observable(data.SerialNumber);
    self.tagNumber = ko.observable(data.TagNumber);
    self.specifications = ko.observable(data.Specifications);
    self.brand = ko.observable(data.Brand);
    self.cost = ko.observable(data.Cost);
    self.modelNumber = ko.observable(data.ModelNumber);
    self.isBrandNew = ko.observable(data.IsBrandNew);
    self.purchaseDate = ko.observable(self.parseDate(data.PurchaseDate) || "");
    self.warrantyExpiryDate = ko.observable(self.parseDate(data.WarrantyExpiryDate) || "");
    self.assetCategoryId = ko.observable(data.AssetCategoryId).extend({ required: { params: true, message: messages.general.Select } });
    self.assetCategory = ko.observable();
    self.departmentId = ko.observable(data.DepartmentId).extend({ required: { params: true, message: messages.general.Select } });
    self.department = ko.observable();
    self.vendorId = ko.observable(data.VendorId).extend({ required: { params: true, message: messages.general.Select } });
    self.vendor = ko.observable();
    self.allocatedEmployeeId = ko.observable(data.AllocatedEmployeeId);
    self.allocatedEmployee = ko.observable();
    self.state = ko.observable(data.State).extend({ required: { params: true, message: messages.asset.State } });
    self.stateType = ko.observable();

    if (data.State == 0) {
        self.stateType("Active");
    }
    else if (data.State == 1) {
        self.stateType("InStore");
    } else if (data.State == 2) {
        self.stateType("UnderRepair");
    }
    else if (data.State == 3) {
        self.stateType("Dead");
    } else if (data.State == 4) {
        self.stateType("Unknown");
    }

    self.state.subscribe(function (newValue) {
        if (newValue == '1' || newValue == '2' || newValue == '3' || newValue == '4') {
            self.allocatedEmployeeId('');
            self.allocatedEmployee(null)
        }       
    });


    if (data.AssetCategory) {
        self.assetCategory(data.AssetCategory.Title);
    }
    if (data.Department) {
        self.department(data.Department.Title);
    }
    if (data.Vendor) {
        self.vendor(data.Vendor.Title);
    }
    if (data.AllocatedEmployee) {
        if (data.AllocatedEmployee.User) {
            if (data.AllocatedEmployee.User.Person) {
                if (data.State != 3)
                self.allocatedEmployee(data.AllocatedEmployee.User.Person.Name);
            }
        }
    }


    self.modelState = ko.validatedObservable(
{
    Title: self.title,
    AssetCategoryId: self.assetCategoryId,
    State: self.state,
    DepartmentId: self.departmentId,
    VendorId: self.vendorId,
});
    self.resetValidation = function () {
        self.title.isModified(false);
        self.assetCategoryId.isModified(false);
        self.state.isModified(false);
        self.departmentId.isModified(false);
        self.vendorId.isModified(false);
    };
};