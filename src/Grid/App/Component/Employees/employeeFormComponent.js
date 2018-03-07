ko.components.register('employeeupdate', {
    viewModel: function (params) {
        var self = new FormViewModelBase(this);
        self.setValue(params.entity, params.entity +"-form-template", new Employee({}), "create")
       
        self.imageData = ko.observable(new ImageComponent());

        self.save = function () {
            self.isBusy(true);
            self.selectedData().photoPath(self.imageData().savedName());
            var data = ko.toJS(self.selectedData);
            if (self.selectedData().modelState.isValid()) {
                var result = rapid.dataManager.postData(urls.employees.update, data);
                result.done(function (response) {
                    self.isBusy(false);
                    if (data.id == 0) {
                        self.selectedData(new Employee({}));
                        rapid.notificationManager.showSuccess(rapid.messageManager.createMessage("Employee"));
                        rapid.eventManager.publish(self.entity + "EmployeeSaveNSearch");
                        rapid.eventManager.publish(self.entity + "Cancel");
                    }
                    else {
                        rapid.notificationManager.showSuccess(rapid.messageManager.updateMessage("Employee"));
                        rapid.eventManager.publish(self.entity + "EmployeeSaveNSearch");
                    }
                });
            }
            else {
                self.isBusy(false);
                self.selectedData().showErrors();
            }
        };
 
        self.changePassword = function () {
            self.isBusy(true);
            if (self.selectedData().modelState.isValid()) {
                if (self.selectedData().newPassword() == self.selectedData().confirmNewPassword()) {                
                    var result = rapid.dataManager.postData(urls.employees.resetPassword, ko.toJS(self.selectedData));
                    result.done(function (response) {
                        self.isBusy(false);
                        self.selectedData(new Employee({}));
                        rapid.notificationManager.showSuccess("Password Changed Succesfully");
                        rapid.eventManager.publish(self.entity + "Cancel");
                    });
                }
                else {
                    self.isBusy(false);
                    bootbox.alert("Password not matching...");
                }
            }
            else {
                self.isBusy(false);
                self.selectedData().showErrors();
            }
        };


        amplify.subscribe(self.entity + "Create", function () {
            self.templateName(self.entity +"-form-template");
            self.mode = ko.observable("create");
            self.selectedData(new Employee({}));
            self.imageData().savedName(response.Result.PhotoPath);
            $('.selectit').selectize();
        });


        amplify.subscribe(self.entity + "Update", function (data) {
            
            self.isBusy(true);
            self.templateName(self.entity + "-form-template");
            self.mode = ko.observable("edit");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.employees.get + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new Employee(response.Result));
                    self.imageData().savedName(response.Result.PhotoPath);
                    
                });
            }
        });

        amplify.subscribe(self.entity + "EmployeeDetails", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-employeedetails-template");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.employees.get + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new Employee(response.Result));
                    self.imageData().savedName(response.Result.PhotoPath);
                    var resultName = rapid.dataManager.getData(urls.employees.getRoleAndTechnologyName +self.selectedData().userId());
                    resultName.done(function (response) {
                        self.isBusy(false);
                        self.selectedData().technologyNames.pushAll(response.Result.UserTechnologyNames);
                        self.selectedData().roleNames.pushAll(response.Result.UserRoleNames);
                    });
                });
            }

        });


        amplify.subscribe(self.entity + "Details", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-details-template");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.employees.get + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new Employee(response.Result));
                    self.imageData().savedName(response.Result.PhotoPath);
                    var resultName = rapid.dataManager.getData(urls.employees.getRoleAndTechnologyName + self.selectedData().userId());
                    resultName.done(function (response) {
                        self.isBusy(false);
                        self.selectedData().technologyNames.pushAll(response.Result.UserTechnologyNames);
                        self.selectedData().roleNames.pushAll(response.Result.UserRoleNames);
                    });
                });
            }

        });



        amplify.subscribe(self.entity + "ResetPassword", function (data) {
            self.isBusy(true);
            self.templateName("resetpassword-template");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.employees.get + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new Employee(response.Result));
                });
            }
        });
    },
    template: {
        fromUrl: 'Form/FormContent?type=Employee'
    }
});