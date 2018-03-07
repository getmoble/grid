ko.components.register('emergencycontactlist', {
    viewModel: function (params) {

        var self = this;
        var self = ListViewModelBase(this);


        self.setValues(params);

        if (params.employeeId() != 0) {
            var args = [{ key: "emergencyContact", value: params.employeeId() }];
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
                    if (params.employeeId() != 0) {
                        var args = [{ key: "emergencyContact", value: params.employeeId() }];
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

    },
    template: { fromUrl: 'List?type=emergencycontacts' }
});