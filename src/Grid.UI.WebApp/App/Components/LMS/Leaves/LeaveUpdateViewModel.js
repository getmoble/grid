define(["require", "exports", 'knockout', 'komapping', 'dataManager', 'interactionService', 'navigationService'], function (require, exports, ko, mapping, dataManager, interactionService, navigationService) {
    var vm = function (params) {

        var self = {};

        self.isBusy = ko.observable();

        self.selected = {
            Id: ko.observable(0),
            Start: ko.observable(),
            End: ko.observable(),
            LeaveTypeId: ko.observable(),
            Duration: ko.observable(),
            Reason: ko.observable(),
            Status: ko.observable(),
            Count: ko.observable(),
            IsLOP: ko.observable(),
            CalculationLog: ko.observable(),
            ApproverId: ko.observable(),
            ApproverComments: ko.observable(),
            ActedOn: ko.observable()
        }

        self.startDateLabel = ko.observable("Start Date");
        self.showEndDate = ko.observable(false);

        self.selected.Duration.subscribe(function (newValue) {
            if (newValue > 0) {
                self.showEndDate(false);
                self.startDateLabel("Date");
            } else {
                self.showEndDate(true);
                self.startDateLabel("Start Date");
            }
        });

        self.submitForm = function (formElement) {
            // Validate 

            if (!self.selected.Reason()) {
                interactionService.alert("Please fill out a reason");
                return;
            }

            if (self.selected.Duration() > 0) {
                self.selected.End(self.selected.Start());
            }

            if (self.selected.End() >= self.selected.Start()) {

                self.isBusy(true);

                // First check Leave Balance
                var payload = $(formElement).serialize();
                var leaveBalanceUrl = '/Api/Leaves/CheckLeaveBalance';
                var updateUrl = '/Api/Leaves/Update';

                dataManager.postData(leaveBalanceUrl, payload, function (apiResult) {
                    self.isBusy(false);
                    if (apiResult.Status) {

                        // Set Leave Count
                        self.selected.Count(apiResult.Result.LeaveCount);
                        payload = $(formElement).serialize();

                        var deducationMessage = "You are applying for " + apiResult.Result.LeaveCount + " days leave which will be deducted from your Leave Balance of " + apiResult.Result.Allocation + " days";
                        interactionService.confirm("Are you sure you want to apply leave? " + deducationMessage, function (dialogResult) {
                            if (dialogResult) {
                                self.isBusy(true);
                                dataManager.postData(updateUrl, payload, function (apiResult) {
                                    self.isBusy(false);
                                    if (apiResult.Status) {
                                        navigationService.goBack();
                                    }
                                    else {
                                        interactionService.alert(apiResult.Message);
                                    }
                                });
                            }
                        });
                    } else {
                        interactionService.alert(apiResult.Message);
                    }
                });

            } else {
                interactionService.alert("End Date should be greater than Start Date");
            }
        }

        self.load = function (id) {
            self.isBusy(true);
            var apiUrl = "/Api/Leaves/Get/" + id;

            dataManager.getData(apiUrl, function (apiResult) {
                if (apiResult.Status) {
                    mapping.fromJS(apiResult.Result, {}, self.selected);
                    self.isBusy(false);
                }
                else {
                    interactionService.alert(apiResult.Message);
                }
            });
        }

        // If we have an id coming in, then it's an edit form
        if (typeof (params.id) !== "undefined" && params.id !== null) {
            self.load(params.id);
        }

        return self;
    }
    return vm;
});