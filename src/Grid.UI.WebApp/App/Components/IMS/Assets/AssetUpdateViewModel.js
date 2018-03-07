define(["require", "exports", 'knockout', 'komapping', 'dataManager', 'interactionService', 'navigationService'], function (require, exports, ko, mapping, dataManager, interactionService, navigationService) {
    var vm = function (params) {

        var self = {};

        self.isBusy = ko.observable();

        self.selected = {
            Id: ko.observable(0),
            SerialNumber: ko.observable(),
            TagNumber: ko.observable(),
            Title: ko.observable(),
            Description: ko.observable(),
            Specifications: ko.observable(),
            Brand: ko.observable(),
            Cost: ko.observable(),
            ModelNumber: ko.observable(),
            IsBrandNew: ko.observable(),
            State: ko.observable(),
            PurchaseDate: ko.observable(),
            WarrantyExpiryDate: ko.observable(),
            AssetCategoryId: ko.observable(),
            DepartmentId: ko.observable(),
            VendorId: ko.observable(),
            AllocatedUserId: ko.observable()
        }

        self.submitForm = function (formElement) {
            self.isBusy(true);
            var updateUrl = '/Api/Assets/Update';
            var payload = $(formElement).serialize();
            dataManager.postData(updateUrl, payload, function (apiResult) {
                self.isBusy(false);
                if (apiResult.Status) {
                    navigationService.goBack();
                }
                else {
                    interactionService.alert(apiResult.Message);
                }
            });
        }

        self.load = function (id) {
            self.isBusy(true);
            var apiUrl = "/Api/Assets/Get/" + id;

            dataManager.getData(apiUrl, function (apiResult) {
                if (apiResult.Status) {
                    mapping.fromJS(apiResult.Result, {}, self.selected);
                    self.isBusy(false);
                }
                else {
                    interactionService.alert(apiResult.Message);
                }
            });
        }

        // If we have an id coming in, then it's an edit form
        if (typeof (params.id) !== "undefined" && params.id !== null) {
            self.load(params.id);
        }

        return self;
    }
    return vm;
});