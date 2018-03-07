define(["require", "exports", 'knockout', 'komapping', 'dataManager', 'interactionService', 'navigationService'], function (require, exports, ko, mapping, dataManager, interactionService, navigationService) {
    var vm = function (params) {

        var self = {};

        self.isBusy = ko.observable();

        self.ticketCategories = ko.observableArray([]);
        self.ticketSubCategories = ko.observableArray([]);

        self.selected = {
            Id: ko.observable(0),
            TicketCategoryId: ko.observable(),
            TicketSubCategoryId: ko.observable(),
            Title: ko.observable(),
            Description: ko.observable(),
            DueDate: ko.observable(),
            Status: ko.observable(),
            AssignedToUserId: ko.observable()
        }

        self.selected.TicketCategoryId.subscribe(function (newValue) {
            self.isBusy(true);
            var apiUrl = '/Api/TicketSubCategories?CategoryId=' + newValue;
            dataManager.getData(apiUrl, function (apiResult) {
                if (apiResult.Status) {
                    self.ticketSubCategories.removeAll();
                    $.each(apiResult.Result, function (k, v) {
                        self.ticketSubCategories.push(v);
                    });
                    self.isBusy(false);
                }
                else {
                    interactionService.alert(apiResult.Message);
                }
            });
        });

        self.submitForm = function (formElement) {
            self.isBusy(true);
            var updateUrl = '/Api/Tickets/Update';
            var payload = $(formElement).serialize();
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

        self.loadInit = function () {
            self.isBusy(true);

            var apiUrl = '/Api/TicketCategories';
            dataManager.getData(apiUrl, function (apiResult) {
                if (apiResult.Status) {

                    $.each(apiResult.Result, function (k, v) {
                        self.ticketCategories.push(v);
                    });

                    // If we have an id coming in, then it's an edit form
                    if (typeof (params.id) !== "undefined" && params.id !== null) {
                        self.load(params.id);
                    } else {
                        self.isBusy(false);
                    }

                }
                else {
                    interactionService.alert(apiResult.Message);
                }
            });

        }

        self.load = function (id) {
            var apiUrl = "/Api/Tickets/Get/" + id;
            dataManager.getData(apiUrl, function (apiResult) {
                if (apiResult.Status) {
                    mapping.fromJS(apiResult.Result, {}, self.selected);
                    self.isBusy(false);
                }
                else {
                    self.isBusy(false);
                    interactionService.alert(apiResult.Message);
                }
            });
        }

        self.loadInit();

        return self;
    }
    return vm;
});