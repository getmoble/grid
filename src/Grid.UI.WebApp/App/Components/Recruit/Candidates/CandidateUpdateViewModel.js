define(["require", "exports", 'knockout', 'komapping', 'dataManager', 'interactionService', 'navigationService'], function (require, exports, ko, mapping, dataManager, interactionService, navigationService) {
    var vm = function (params) {

        var self = {};

        self.isBusy = ko.observable();

        self.selected = {
            Id: ko.observable(0),
            RecievedOn: ko.observable(),
            Status: ko.observable(),
            Source: ko.observable(),
            TechnologyIds: ko.observableArray([]),
            FirstName: ko.observable(),
            MiddleName: ko.observable(),
            LastName: ko.observable(),
            Gender: ko.observable(),
            Email: ko.observable(),
            SecondaryEmail: ko.observable(),
            PhoneNo: ko.observable(),
            OfficePhone: ko.observable(),
            Skype: ko.observable(),
            Facebook: ko.observable(),
            Twitter: ko.observable(),
            GooglePlus: ko.observable(),
            LinkedIn: ko.observable(),
            Organization: ko.observable(),
            Designation: ko.observable(),
            CandidateDesignation: ko.observable(),
            Address: ko.observable(),
            CommunicationAddress: ko.observable(),
            DateOfBirth: ko.observable(),
            Qualification: ko.observable(),
            TotalExperience: ko.observable(),
            CurrentCTC: ko.observable(),
            ExpectedCTC: ko.observable(),
            Comments: ko.observable()
        }

        self.submitForm = function (formElement) {
            self.isBusy(true);
            var updateUrl = '/Api/Candidates/Update';
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

        self.load = function (id) {
            self.isBusy(true);
            var apiUrl = "/Api/Candidates/Get/" + id;

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