﻿ko.components.register('assetsupdate', {
    viewModel: function (params) {
        var self = new FormViewModelBase(this);
        self.setValue(params.entity, params.entity + "-form-template", new Asset({}), "create")

        self.save = function () {
            self.isBusy(true);
            var data = ko.toJS(self.selectedData);
            if (self.selectedData().modelState.isValid()) {
                var result = rapid.dataManager.postData(urls.assets.update, data);
                result.done(function (response) {
                    self.isBusy(false);
                    if (data.id == 0) {
                        self.selectedData(new Asset({}));
                        rapid.notificationManager.showSuccess(rapid.messageManager.createMessage("Asset"));
                        rapid.eventManager.publish(self.entity + "AssetSaveNSearch");
                        rapid.eventManager.publish(self.entity + "Cancel");
                    }
                    else {
                        rapid.notificationManager.showSuccess(rapid.messageManager.updateMessage("Asset"));
                        rapid.eventManager.publish(self.entity + "AssetSaveNSearch");
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
            self.selectedData(new Asset({}));
        });

        amplify.subscribe(self.entity + "Update", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-form-template");
            self.mode = ko.observable("edit");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.assets.get + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new Asset(response.Result));
                });
            }
        });

        amplify.subscribe(self.entity + "Details", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-details-template");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.assets.get + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new Asset(response.Result));
                });
            }

        });
    },
    template: {
        fromUrl: 'Form/FormContent?type=Asset'
    }
});