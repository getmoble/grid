ko.components.register('leaveentitlementslogdetails', {
    viewModel: function (params) {
        var self = new FormViewModelBase(this);
        self.setValue(params.entity, params.entity + "-form-template", new LeaveEntitlement({}), "create")

     
        self.selectedData().employeeId(params.employeeId());

        amplify.subscribe(self.entity + "Details", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-details-template");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.leaveEntitlement.getEntitlementLog + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new LeaveEntitlement(response.Result));
                });
            }

        });
    },
    template: {
        fromUrl: 'Form/FormContent?type=LeaveEntitlementsLog'
    }
});