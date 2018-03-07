ko.components.register('dependentupdate', {
    viewModel: function (params) {
        var self = new FormViewModelBase(this);    

        self.setValue(params.entity, params.entity + "-form-template", new Dependent({}), "create")
        self.selectedData().employeeId(params.employeeId());

        self.save = function () {
            self.isBusy(true);
            var data = ko.toJS(self.selectedData);
            if (self.selectedData().modelState.isValid()) {
                var result = rapid.dataManager.postData(urls.dependent.update, data);
                result.done(function (response) {
                    self.isBusy(false);
                    if (data.id == 0) {
                        self.selectedData(new Dependent({}));
                        var args = [{ key: "employeeDependent", value: params.employeeId() }];
                        rapid.eventManager.publish(self.entity + "Search", args);
                        rapid.notificationManager.showSuccess("Dependent Created Succesfully");
                        rapid.eventManager.publish(self.entity + "Cancel");
                    }
                    else {
                        var args = [{ key: "employeeDependent", value: params.employeeId() }];
                        rapid.eventManager.publish(self.entity + "Search", args);
                        rapid.notificationManager.showSuccess("Dependent Updated Succesfully");
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
            self.selectedData(new Dependent({}));
            self.selectedData().employeeId(params.employeeId());
        });

        amplify.subscribe(self.entity + "Update", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-form-template");
            self.mode = ko.observable("edit");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.dependent.get + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new Dependent(response.Result));
                    self.selectedData().employeeId(params.employeeId());
                });
            }
        });

        amplify.subscribe(self.entity + "Details", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-details-template");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.dependent.get + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new Dependent(response.Result));
                });
            }

        });

    },
    template: {
        fromUrl: 'Form/FormContent?type=Dependent'
    }
});