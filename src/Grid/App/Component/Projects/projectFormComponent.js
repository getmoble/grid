ko.components.register('projectsupdate', {
    viewModel: function (params) {
        var self = new FormViewModelBase(this);
        self.setValue(params.entity, params.entity + "-form-template", new Project({}), "create")
        self.projectId = ko.observable();
        self.showAddMember = ko.observable(false);

        self.save = function () {
            self.isBusy(true);
            var data = ko.toJS(self.selectedData);
            if (self.selectedData().modelState.isValid()) {
                var result = rapid.dataManager.postData(urls.projects.update, data);
                result.done(function (response) {
                    self.isBusy(false);
                    if (data.id == 0) {
                        self.selectedData(new Project({}));
                        rapid.notificationManager.showSuccess(rapid.messageManager.createMessage("Project"));
                        rapid.eventManager.publish(self.entity + "Cancel");
                        rapid.eventManager.publish(self.entity + "UpdatedAndSearch");
                    }
                    else {
                        rapid.notificationManager.showSuccess(rapid.messageManager.updateMessage("Project"));
                        rapid.eventManager.publish(self.entity + "UpdatedAndSearch");                                             
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
            self.selectedData(new Project({}));
        });

        amplify.subscribe(self.entity + "Update", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-form-template");
            self.mode = ko.observable("edit");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.projects.get + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new Project(response.Result));                
                });
            }
        });

        amplify.subscribe(self.entity + "AddMember", function (data) {
            self.showAddMember(false);
            self.showAddMember(true);
            self.isBusy(true);
           self.templateName("addmember-template");
            if (data.entityId) {
                self.projectId(data.entityId);
            }
        });
       
    },
    template: {
        fromUrl: 'Form/FormContent?type=Project'
    }
});