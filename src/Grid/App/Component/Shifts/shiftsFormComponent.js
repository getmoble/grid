ko.components.register('shiftsupdate', {
    viewModel: function (params) {
        var self = new FormViewModelBase(this);
        self.setValue(params.entity, params.entity + "-form-template", new Shift({}), "create")

        self.save = function () {
            self.isBusy(true);
            var data = ko.toJS(self.selectedData);
            if (self.selectedData().modelState.isValid()) {
                if (self.selectedData().endTime() >= self.selectedData().startTime()) {
                var result = rapid.dataManager.postData(urls.shifts.update, data);
                result.done(function (response) {
                    self.isBusy(false);
                    if (data.id == 0) {
                        self.selectedData(new Shift({}));
                        rapid.notificationManager.showSuccess(rapid.messageManager.createMessage("Shift"));
                        rapid.eventManager.publish(self.entity + "Created");
                        rapid.eventManager.publish(self.entity + "Cancel");
                    }
                    else {
                        rapid.notificationManager.showSuccess(rapid.messageManager.updateMessage("Shift"));
                        rapid.eventManager.publish(self.entity + "Updated");
                    }
                });
                } else {
                    self.isBusy(false);
                    bootbox.alert("End Date should be greater than Start Date");                   
                }
            }
            else {
                self.isBusy(false);
                self.selectedData().showErrors();
            }
        };

        amplify.subscribe(self.entity + "Create", function () {
            self.templateName(self.entity + "-form-template");
            self.mode = ko.observable("create");
            self.selectedData(new Shift({}));
        });

        amplify.subscribe(self.entity + "Update", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-form-template");
            self.mode = ko.observable("edit");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.shifts.get + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new Shift(response.Result));
                });
            }
        });

        amplify.subscribe(self.entity + "Details", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-details-template");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.shifts.get + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new Shift(response.Result));
                });
            }

        });
    },
    template: {
        fromUrl: 'Form/FormContent?type=Shift'
    }
});