ko.components.register('projectmemberslist', {
    viewModel: function (params) {
        var self = ListViewModelBase(this);
        self.setValues(params);
     
        if (params.projectId() != 0) {
            var args = [{ key: "projectId", value: params.projectId() }];
            self.fillData(args);

        };
        self.delete = function (item) {
            rapid.modalManager.confirm(rapid.messageManager.deleteWarningMessage('this'), function (result) {
                if (result) {
                    var jsonData = { entityType: self.entity, entityId: item.Id };
                    rapid.eventManager.publish(self.entity + "Delete", jsonData);
                }

            });
        };
        self.SubscribeDelete = function (data) {
            self.isBusy(true);
            var deleteResult = rapid.dataManager.postData(urls.generic.delete + data.entityType + "s/" + data.entityId);
            deleteResult.done(function (apiResult) {
                if (apiResult.Status) {
                    self.isBusy(false);
                    if (params.projectId() != 0) {
                        var args = [{ key: "projectId", value: params.projectId() }];
                        rapid.eventManager.publish(self.entity + "Search", args);
                        rapid.notificationManager.showSuccess("Deleted Succesfully");
                        rapid.eventManager.publish(self.entity + "Cancel");
                    }
                } else {
                    self.isBusy(false);
                    bootbox.alert("Entity can't be delete!..");
                    rapid.eventManager.publish(self.entity + "Cancel");
                }
            });
        };

        self.changeStatus = function (item) {
            bootbox.confirm("Are you sure you want to change status for this employee??", function (result) {
                if (result == true) {
                    var result = rapid.dataManager.getData("/PMS/ProjectMembers/ChangeMemberStatus?id=" + item.Id);
                    result.done(function (response) {
                        if (response) {
                            rapid.notificationManager.showSuccess("Status Changed Succesfully");
                            if (params.projectId() != 0) {
                                var args = [{ key: "projectId", value: params.projectId() }];
                                self.fillData(args);
                            }
                        }
                    });
                };
            });
        }


    },
    template: { fromUrl: 'List?type=projectmembers' }
});