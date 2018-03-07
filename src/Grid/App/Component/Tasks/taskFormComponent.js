ko.components.register('tasksformupdate', {
    viewModel: function (params) {
        var self = new FormViewModelBase(this);
        self.setValue(params.entity, params.entity + "-form-template", new Task({}), "create")
       
        self.save = function () {
            self.isBusy(true);
            var data = ko.toJS(self.selectedData);
            if (self.selectedData().modelState.isValid()) {
                var result = rapid.dataManager.postData("/Api/Tasks/Update", data);
                result.done(function (response) {
                    self.isBusy(false);
                    if (data.id == 0) {
                        self.selectedData(new Task({}));
                        rapid.notificationManager.showSuccess(rapid.messageManager.createMessage("Task"));
                        rapid.eventManager.publish(self.entity + "Cancel");
                        rapid.eventManager.publish(self.entity + "TaskSaveNSearch");
                    }
                    else {
                        rapid.notificationManager.showSuccess(rapid.messageManager.updateMessage("Task"));
                        rapid.eventManager.publish(self.entity + "TaskSaveNSearch");
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
            self.selectedData(new Task({}));
        });

        amplify.subscribe(self.entity + "Update", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-form-template");
            self.mode = ko.observable("edit");
            if (data.entityId) {
                var result = rapid.dataManager.getData("/Api/Tasks/Get/" + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new Task(response.Result));       
                });
            }
        });
   
    },
    template: {
        fromUrl: 'Form/FormContent?type=Task'
    }
});