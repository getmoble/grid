ko.components.register('assetsearch', {
    viewModel: function (params) {

        var self = this;

        self.entity = "asset";

        self.departmentId = ko.observable();
        self.categoryId = ko.observable();
        self.vendorId = ko.observable();
        self.state = ko.observable();
        self.userId = ko.observable();
        self.tagNumber = ko.observable();
        self.serialNumber = ko.observable();
        self.modelNumber = ko.observable();
        self.title = ko.observable();
        
      

        self.filter = function () {
            var args = [{ key: "departmentData", value: self.departmentId() }, { key: "category", value: self.categoryId() }, { key: "vendor", value: self.vendorId() }, { key: "assetState", value: self.state() }, { key: "userName", value: self.userId() }, { key: "tagNumber", value: self.tagNumber() }, { key: "modelNumber", value: self.modelNumber() }, { key: "serialNumber", value: self.serialNumber() }, { key: "title", value: self.title() }];
            rapid.eventManager.publish(self.entity + "Search", args);
        };
        self.refresh = function (item) {
            self.departmentId("");
            self.categoryId("");
            self.vendorId("");
            self.state("");
            self.userId("");
            self.tagNumber("");
            self.serialNumber("");
            self.modelNumber("");
            self.title("");


            var jsonData = { entityType: self.entity, entityId: item.Id };
            rapid.eventManager.publish(self.entity + "Refresh", jsonData);
        };

        amplify.subscribe(self.entity + "AssetSaveNSearch", function () {
            self.filter();
        });


        self.exportCsv = function () {
            bootbox.confirm("Are you sure you want to Export Csv file? ", function (result) {
                if (result == true) {
                    window.location = "/Api/Assets/ExportCsv";
                }
            });
        }
    },
    template: { element: 'assetsearch-template' }
});