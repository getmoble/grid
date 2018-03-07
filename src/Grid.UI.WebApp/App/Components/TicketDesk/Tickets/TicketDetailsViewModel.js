define(["require", "exports", 'knockout', 'komapping', 'dataManager', 'interactionService', 'navigationService', "jquery"], function (require, exports, ko, mapping, dataManager, interactionService, navigationService, $) {
    var vm = function (params) {

        var self = {};

        self.isBusy = ko.observable();

        self.activities = ko.observableArray([]);
        self.ticketId = null;
        self.activity = ko.observable();

        self.selected = {
            Id: ko.observable(0),
            Title: ko.observable(),
            Description: ko.observable(),
            DueDate: ko.observable(),
            TicketCategory: ko.observable(),
            TicketSubCategory: ko.observable(),
            AssignedToUser: ko.observable(),
            CreatedByUser: ko.observable(),
            LastUpdatedOn: ko.observable(),
            CreatedOn: ko.observable(),
            Status: ko.observable(),
            UpdatedOn: ko.observable()
        }

        self.edit = function () {
            var editUrl = "#ticket/edit/" + self.ticketId;
            navigationService.navigate(editUrl);
        }

        self.addNewNote = function () {
            dataManager.postData('/Api/Tickets/AddNote/', { TicketId: self.ticketId, Title: $("#newNoteTitle").val(), Comment: $("#newNote").val() }, function () {
                self.getAllActivities();
            });
        };

        self.getAllActivities = function () {
            dataManager.getData('/Api/Tickets/GetAllActivitiesForTicket/' + self.ticketId, function (data) {
                self.activities.removeAll();
                $.each(data, function (k, v) {
                    self.activities.push(v);
                });
            });
        };

        self.removeActivity = function (activity) {
            interactionService.confirm("Are you sure you want to delete it", function (result) {
                if (result) {
                    var apiUrl = '/Api/Tickets/DeleteNote/' + activity.Id;
                    dataManager.postData(apiUrl, {}, function() {
                        self.activities.remove(activity);
                    });
                }
            });
        };

        self.load = function () {
            var apiUrl = "/Api/Tickets/Get/" + self.ticketId;
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

        // If we have an id coming in, then it's an edit form
        if (typeof (params.id) !== "undefined" && params.id !== null) {
            self.ticketId = params.id;
            self.load();
            self.getAllActivities();
        } else {
            self.isBusy(false);
        }

        return self;
    }
    return vm;
});