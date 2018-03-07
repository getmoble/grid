ko.components.register('leavebalanceupdate', {
    viewModel: function (params) {
        var self = new FormViewModelBase(this);
        self.setValue(params.entity, params.entity + "-form-template", new LeaveBalance({}), "create")
       
        amplify.subscribe(self.entity + "Details", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-details-template");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.employees.get + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new LeaveBalance(response.Result));                    
                });
            }

        });

    },
    template: {
        fromUrl: 'Form/FormContent?type=LeaveBalance'
    }
});