define(["require", "exports", 'knockout', 'komapping', 'dataManager', 'interactionService', 'navigationService'], function (require, exports, ko, mapping, dataManager, interactionService, navigationService) {
    var vm = function (params) {

        var self = {};

        self.isBusy = ko.observable();

        self.activities = ko.observableArray([]);
        self.candidateId = null;
        self.activity = ko.observable();

        self.selected = {
            Id: ko.observable(0),
            Email: ko.observable(),
            Source: ko.observable(),
            Qualification: ko.observable(),
            TotalExperience: ko.observable(),
            ResumePath: ko.observable(),
            PhotoPath: ko.observable(),
            Status: ko.observable(),
            Comments: ko.observable(),
            CurrentCTC: ko.observable(),
            ExpectedCTC: ko.observable(),
            PersonId: ko.observable(),
            DesignationId: ko.observable(),
            RecievedOn: ko.observable()
        }

        self.edit = function () {
            var editUrl = "#candidate/edit/" + self.candidateId;
            navigationService.navigate(editUrl);
        }

        self.addNewNote = function () {
            dataManager.postData('/Api/Candidates/AddNote/', { CandidateId: self.candidateId, Title: $("#newNoteTitle").val(), Comment: $("#newNote").val() }, function () {
                self.getAllActivities();
            });
        };

        self.getAllActivities = function () {
            dataManager.getData('/Api/Candidates/GetAllActivitiesForCandidate/' + self.candidateId, function (data) {
                self.activities.removeAll();
                $.each(data, function (k, v) {
                    self.activities.push(v);
                });
            });
        };

        self.removeActivity = function (activity) {
            interactionService.confirm("Are you sure you want to delete it", function (result) {
                if (result) {
                    var apiUrl = '/Api/Candidates/DeleteNote/' + activity.Id;
                    dataManager.postData(apiUrl, {}, function () {
                        self.activities.remove(activity);
                    });
                }
            });
        };

        self.load = function () {
            var apiUrl = "/Api/Candidates/Get/" + self.candidateId;
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
            self.candidateId = params.id;
            self.load();
            self.getAllActivities();
        } else {
            self.isBusy(false);
        }

        return self;
    }
    return vm;
});