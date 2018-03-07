ko.components.register('ticketsubcategoriesupdate', {
    viewModel: function (params) {
        var self = new FormViewModelBase(this);
        self.setValue(params.entity, params.entity + "-form-template", new TicketSubCategory({}), "create")
       

        self.save = function () {
            self.isBusy(true);
            var data = ko.toJS(self.selectedData);           
            if (self.selectedData().modelState.isValid()) {
                var result = rapid.dataManager.postData(urls.ticketSubCategories.update, data);
                result.done(function (response) {
                    self.isBusy(false);
                    if (data.id == 0) {
                        self.selectedData(new TicketSubCategory({}));
                        rapid.notificationManager.showSuccess(rapid.messageManager.createMessage("Ticket Sub Category"));
                        rapid.eventManager.publish(self.entity + "Created");
                        rapid.eventManager.publish(self.entity + "Cancel");
                    }
                    else {
                        rapid.notificationManager.showSuccess(rapid.messageManager.updateMessage("Ticket Sub Category"));
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
            self.selectedData(new TicketSubCategory({}));
        });

        amplify.subscribe(self.entity + "Update", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-form-template");
            self.mode = ko.observable("edit");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.ticketSubCategories.get + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new TicketSubCategory(response.Result));
                });
            }
        });

        amplify.subscribe(self.entity + "Details", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-details-template");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.ticketSubCategories.get + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new TicketSubCategory(response.Result));
                });
            }

        });
    },
    template: {
        fromUrl: 'Form/FormContent?type=TicketSubCategory'
    }
});