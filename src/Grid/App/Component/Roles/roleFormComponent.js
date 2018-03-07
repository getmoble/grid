ko.components.register('rolesupdate', {
    viewModel: function (params) {
        var self = new FormViewModelBase(this);
        self.setValue(params.entity, params.entity + "-form-template", new Roles({}), "create")
        self.permissions = ko.observableArray();

        self.save = function () {
            self.isBusy(true);
            var data = ko.toJS(self.selectedData);
            if (self.selectedData().modelState.isValid()) {
                var result = rapid.dataManager.postData(urls.roles.update, data);
                result.done(function (response) {
                    self.isBusy(false);
                    if (data.id == 0) {
                        self.selectedData(new Roles({}));
                        rapid.notificationManager.showSuccess(rapid.messageManager.createMessage("Role"));
                        rapid.eventManager.publish(self.entity + "Created");
                        rapid.eventManager.publish(self.entity + "Cancel");
                    }
                    else {
                        rapid.notificationManager.showSuccess(rapid.messageManager.updateMessage("Role"));
                        rapid.eventManager.publish(self.entity + "Updated");
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
            self.selectedData(new Roles({}));
            $('.selectit').selectize();
        });

        amplify.subscribe(self.entity + "Update", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-form-template");
            self.mode = ko.observable("edit");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.roles.get + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new Roles(response.Result));
                    $('.selectit').selectize();
                });
            }
        });

        amplify.subscribe(self.entity + "Details", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-details-template");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.roles.get + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new Roles(response.Result));
                    $('.selectit').selectize();
                });
            }

        });

        self.init = function () {

            var result = rapid.dataManager.getData(urls.generic.selectList + "permission");
            result.done(function (response) {
                var dataArray = JSLINQ(response.Result)
                                            .Select(function (item) {
                                                var list = new SelectItem();
                                                list.id = item.Id;
                                                list.name = item.Name;
                                                return list;
                                            }).items;
               
                self.permissions.pushAll(dataArray);
            });
        };

        self.init();
    },
    template: {
        fromUrl: 'Form/FormContent?type=Role'
    }
});
























