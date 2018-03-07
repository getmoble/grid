ko.components.register('entitlementhistoryupdate', {
    viewModel: function (params) {
        var self = new FormViewModelBase(this);


        self.setValue(params.entity, params.entity + "-form-template", new EntitlementHistory({}), "create")
        self.selectedData().employeeId(params.employeeId());

   
        amplify.subscribe(self.entity + "Create", function () {
            self.templateName(self.entity + "-form-template");
            self.mode = ko.observable("create");
            self.selectedData(new EntitlementHistory({}));
            self.selectedData().employeeId(params.employeeId());
        });
        amplify.subscribe(self.entity + "Details", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-details-template");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.leaveEntitlement.getLeaveEntitlementsLog + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new EntitlementHistory(response.Result));
                });
            }

        });


    },
    template: {
        fromUrl: 'Form/FormContent?type=EntitlementHistory'
    }
});