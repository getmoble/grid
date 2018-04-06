ko.components.register('inactiveprojectmembers', {
    viewModel: function (params) {
        var self = new FormViewModelBase(this);

        self.setValue(params.entity, params.entity + "-form-template", new ProjectMember({}), "create")
        self.selectedData().projectId(params.projectId());


        self.save = function () {
            self.isBusy(true);
            var data = ko.toJS(self.selectedData);
            if (self.selectedData().modelState.isValid()) {
                var result = rapid.dataManager.postData("/PMS/ProjectMembers/CreateProjectMember", data);
                result.done(function (response) {
                    if (response) {
                        self.isBusy(false);
                        if (data.id == 0) {
                            self.selectedData(new ProjectMember({}));

                            var args = [{ key: "projectId", value: params.projectId() }];
                            rapid.eventManager.publish(self.entity + "Search", args);
                            rapid.eventManager.publish("projectmember" + "Search", args);
                            rapid.notificationManager.showSuccess("Member added succesfully");
                            rapid.eventManager.publish(self.entity + "Cancel");
                        }
                        else {
                            var args = [{ key: "projectId", value: params.projectId() }];
                            rapid.eventManager.publish(self.entity + "Search", args);
                            rapid.eventManager.publish("projectmember" + "Search", args);
                            rapid.notificationManager.showSuccess("Member updated succesfully");
                        }

                    }
                    else {
                        self.isBusy(false);
                        bootbox.alert("Member already exits!..");
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
            self.selectedData(new ProjectMember({}));
            self.selectedData().projectId(params.projectId());
        });

        amplify.subscribe(self.entity + "Update", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-form-template");
            self.mode = ko.observable("edit");
            if (data.entityId) {
                var result = rapid.dataManager.getData("/PMS/ProjectMembers/Get/" + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new ProjectMember(response));
                    self.selectedData().projectId(params.projectId());
                });
            }
        });


        amplify.subscribe(self.entity + "Details", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-details-template");
            if (data.entityId) {
                var result = rapid.dataManager.getData("/PMS/ProjectMembers/Get/" + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new ProjectMember(response));
                    self.selectedData().projectId(params.projectId());
                });
            }

        });
    },
    template: {
        fromUrl: 'Form/FormContent?type=InActiveProjectMember'
    }
});

