define(["require", "exports", 'knockout', 'jquery'], function (require, exports, ko, $) {

    var self = {};

    self.isBusy = ko.observable();

    self.entity = '';

    self.filters = ko.observableArray([]);
    self.data = ko.observableArray([]);

    self.loadAll = function () {
        self.isBusy(true);
        var apiUrl = '/Api/' + self.entity;

        if (self.filters().length > 0) {
            apiUrl = apiUrl + "?";
            // Add all filters
            $.each(self.filters(), function (key, val) {
                apiUrl = apiUrl + val;
            });
        }

        $.getJSON(apiUrl, function (apiResult) {
            if (apiResult.Status) {
                self.data.removeAll();
                $.each(apiResult.Result, function (key, val) {
                    self.data.push(val);
                });
                self.isBusy(false);
            }
            else {
                self.isBusy(false);
                bootbox.alert(apiResult.Message, function () {

                });
            }
        });
    };

    self.delete = function (row) {
        bootbox.confirm("Are you sure, you want to delete this?", function (result) {
            if (result) {
                var deleteUrl = '/Api/' + self.entity + "/Delete/" + row.Id;
                $.post(deleteUrl, function (apiResult) {
                    if (apiResult.Status) {
                        bootbox.alert(apiResult.Message, function () {
                            self.loadAll();
                        });
                    } else {
                        bootbox.alert(apiResult.Message, function () {

                        });
                    }
                });
            }
        });
    }
    self.details = function (row) {
        var detailsUrl = self.entity + "/Details/" + row.Id;
        window.location = detailsUrl;
    }
    self.edit = function (row) {
        var editUrl = self.entity + "/Edit/" + row.Id;
        window.location = editUrl;
    }

    self.init = function (entity) {
        self.entity = entity;
        self.loadAll();
    }

    return self;
});