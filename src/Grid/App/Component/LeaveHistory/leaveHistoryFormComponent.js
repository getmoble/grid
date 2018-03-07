ko.components.register('leaveapplications', {
    viewModel: function (params) {
        var self = new FormViewModelBase(this);
        self.setValue(params.entity, params.entity + "-form-template", new LeaveHistory({}), "create")


        self.save = function () {                  
            if (self.selectedData().modelState.isValid()) {                           
                    var result = rapid.dataManager.postData(urls.leavesHistory.checkLeaveBalance, ko.toJS(self.selectedData));
                    result.done(function (response) {
                        if (response.Status) {
                            var deducationMessage = "You are applying for " + response.Result.LeaveCount + " days leave which will be deducted from your Leave Balance of " + response.Result.Allocation + " days";
                            bootbox.confirm("Are you sure you want to apply leave? " + deducationMessage, function (dialogResult) {
                                if (dialogResult) {
                                 
                                    self.selectedData().leaveCount(response.RequestedDays);
                                    // Create the leave
                                    self.isBusy(true);
                                    if (self.selectedData().duration() != 0) {
                                        self.selectedData().end(self.selectedData().start());
                                    }
                                    var data = ko.toJS(self.selectedData);

                                  
                                    if (self.selectedData().modelState.isValid()) {
                                        var result = rapid.dataManager.postData(urls.leavesHistory.update, data);
                                        result.done(function (response) {
                                            self.isBusy(false);
                                            if (data.id == 0) {
                                                self.selectedData(new LeaveHistory({}));
                                                rapid.notificationManager.showSuccess(rapid.messageManager.createMessage("Leave"));
                                                rapid.eventManager.publish(self.entity + "Created");
                                            }
                                            else {
                                                rapid.notificationManager.showSuccess(rapid.messageManager.updateMessage("Leave"));
                                                rapid.eventManager.publish(self.entity + "Updated");
                                            }
                                        });


                                    }
                                    else {
                                        self.isBusy(false);
                                        self.selectedData().showErrors();
                                    }

                                }
                            });

                        }
                        else {
                            bootbox.alert(response.Message);
                        }
                    });            
            }
            else {
                self.isBusy(false);
                self.selectedData().showErrors();
            }
        };

      
        self.approve = function () {
            var data = ko.toJS(self.selectedData);
            self.isBusy(true);
            bootbox.confirm("Are you sure you want to approve this? ", function (dialogResult) {
                if (dialogResult) {
                    var result = rapid.dataManager.postData(urls.leavesHistory.approve, data);
                    result.done(function (response) {
                        if (response.Status) {
                            self.isBusy(false);
                            self.selectedData(new LeaveHistory({}));
                            rapid.notificationManager.showSuccess("Succesfully Approved");
                            rapid.eventManager.publish(self.entity + "Cancel");
                            rapid.eventManager.publish(self.entity + "Refresh");
                        }
                        else {
                            self.isBusy(false);
                            rapid.notificationManager.showWarning("Not Enough Balance to Approve the Leave");
                        }
                    });
                }
                else {
                    self.isBusy(false);
                    rapid.eventManager.publish(self.entity + "Refresh");
                }
            });
        };

        self.reject = function () {
            var data = ko.toJS(self.selectedData);
            self.isBusy(true);
            bootbox.confirm("Are you sure you want to reject this? ", function (dialogResult) {
                if (dialogResult) {
                    var result = rapid.dataManager.postData(urls.leavesHistory.reject, data);
                    result.done(function (response) {
                        if (response.Status) {
                            self.isBusy(false);
                            self.selectedData(new LeaveHistory({}));
                            rapid.notificationManager.showSuccess("Succesfully Rejected");
                            rapid.eventManager.publish(self.entity + "Cancel");
                            rapid.eventManager.publish(self.entity + "Refresh");
                        }
                        else {
                            self.isBusy(false);
                            bootbox.alert(response.Message);
                        }
                    });
                }
                else {
                    self.isBusy(false);
                    rapid.eventManager.publish(self.entity + "Refresh");
                }
            });
        };
        self.delete = function () {
            self.isBusy(true);
            bootbox.confirm("Are you sure you want to delete this? ", function (dialogResult) {
                if (dialogResult) {
                    var result = rapid.dataManager.postData(urls.leavesHistory.delete + self.selectedData().id());
                    result.done(function (response) {
                        if (response.Status) {
                            self.isBusy(false);
                            self.selectedData(new LeaveHistory({}));
                            rapid.notificationManager.showSuccess("Succesfully Deleted");
                            rapid.eventManager.publish(self.entity + "Cancel");
                            rapid.eventManager.publish(self.entity + "Refresh");
                        }
                        else {
                            self.isBusy(false);
                            bootbox.alert(response.Message);
                        }
                    });
                }
                else {
                    self.isBusy(false);
                    rapid.eventManager.publish(self.entity + "Refresh");
                }
            });
        };
        
        amplify.subscribe(self.entity + "Create", function () {
            self.templateName(params.entity + "-form-template");
            self.mode = ko.observable("create");
            self.selectedData(new LeaveHistory({}));
        self.selectedData().resetValidation();
        });

        amplify.subscribe(self.entity + "Update", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-form-template");
            self.mode = ko.observable("edit");
            if (data.entityId) {
                var result = rapid.dataManager.getData(urls.leavesHistory.get + data.entityId);
                result.done(function (response) {
                    self.isBusy(false);
                    self.selectedData(new LeaveHistory(response.Result));
                });
            }
        });
     

        amplify.subscribe(self.entity + "Details", function (data) {
            self.isBusy(true);
            self.templateName(self.entity + "-details-template");           
                    var showAprove = rapid.dataManager.getData(urls.leavesHistory.checkShowApprove + data.entityId);
                    showAprove.done(function (response) {
                        self.isBusy(false);
                        self.selectedData(new LeaveHistory(response.Result.Leave));
                        self.selectedData().isApprove(response.Result.IsApprove);
                    });               
        });

    },
    template: {
        fromUrl: 'Form/FormContent?type=Leave'
    }
});