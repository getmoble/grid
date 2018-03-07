ko.components.register('designationsupdate', {
    viewModel: function (params) {
        var self = new FormViewModelBase(this);
        self.setValue(params.entity, params.entity + "-form-template", new Designations({}), "create")

        self.save = function () {
            self.isBusy(true);
            var data = ko.toJS(self.selectedData);
            if (self.selectedData().modelState.isValid()) {
                var result = rapid.dataManager.postData(urls.designations.update, data);
                result.done(function (response) {
                    self.isBusy(false);
                    if (data.id == 0) {
                        self.selectedData(new Designations({}));
                        rapid.notificationManager.showSuccess(rapid.messageManager.createMessage("Designation"));
                        rapid.eventManager.publish(self.entity + "DesignationSaveNSearch");
                        rapid.eventManager.publish(self.entity + "Cancel");
                    }
                    else {
                        rapid.notificationManager.showSuccess(rapid.messageManager.updateMessage("Designation"));
                        rapid.eventManager.publish(self.entity + "DesignationSaveNSearch");
                    }
                });
            }
            else {
                self.isBusy(false);
                self.selectedData().showErrors();
            }
        };

        amplify.subscribe(self.entity + "Create", function () {
            self.templateName(self.entity + "-form-template");
            self.mode = ko.observable("create");
            self.selectedData(new Designations({}));
        });

        amplify.subscribe(self.entity + "Update", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-form-template");
            self.mode = ko.observable("edit");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.designations.get + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new Designations(response.Result));
                });
            }
        });

        amplify.subscribe(self.entity + "Details", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-details-template");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.designations.get + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new Designations(response.Result));
                });
            }

        });
    },
    template: {
        fromUrl: 'Form/FormContent?type=Designation'
    }
});