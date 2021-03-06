﻿define(["require", "exports", 'knockout', 'interactionService', 'navigationService', 'dataManager'], function (require, exports, ko, interactionService, navigationService, dataManager) {
    var vm = function () {

        var self = {};

        self.isBusy = ko.observable();

        self.filters = ko.observableArray([]);
        self.data = ko.observableArray([]);

        self.loadAll = function () {
            self.isBusy(true);
            var apiUrl = '/Api/SoftwareCategories';

            if (self.filters().length > 0) {
                apiUrl = apiUrl + "?";
                // Add all filters
                $.each(self.filters(), function (key, val) {
                    apiUrl = apiUrl + val;
                });
            }

            dataManager.getData(apiUrl, function (apiResult) {
                if (apiResult.Status) {
                    self.data.removeAll();
                    $.each(apiResult.Result, function (key, val) {
                        self.data.push(val);
                    });
                    self.isBusy(false);
                }
                else {
                    self.isBusy(false);
                    interactionService.alert(apiResult.Message);
                }
            });
        };

        self.delete = function (row) {
            interactionService.confirm("Are you sure, you want to delete this?", function (result) {
                if (result) {
                    var deleteUrl = '/Api/SoftwareCategories/Delete/' + row.Id;
                    dataManager.postData(deleteUrl, null, function (apiResult) {
                        if (apiResult.Status) {
                            self.loadAll();
                        } else {
                            interactionService.alert(apiResult.Message);
                        }
                    });
                }
            });
        }

        self.details = function (row) {
            var detailsUrl = "#softwarecategory/" + row.Id;
            navigationService.navigate(detailsUrl);
        }
        self.edit = function (row) {
            var editUrl = "#softwarecategory/edit/" + row.Id;
            navigationService.navigate(editUrl);
        }

        self.loadAll();

        return self;
    }
    return vm;
});