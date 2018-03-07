ko.components.register('list', {
    viewModel: function(params) {
        var self = PagedViewModelBase(this);
        self.data = ko.observableArray();
        self.isBusy = ko.observable(false);
        self.filter = ko.observable('');
        self.entity = params.entity;
        self.showCreate = params.showCreate;
        self.manageList = params.manageList;
        self.enableFilter = params.enableFilter;
        //self.filldata is to fill the list with the enity 
        self.fillData = function (args) {
            var url = "?";
            self.isBusy(true);
            if (args != null && args != 'undefined') {
                $.each(args, function (k, v) {
                    url += v.key + "=" + v.value + "&"
                });
            }
            var searchUrl = urls.generic.list + self.entity + url;
            var result = rapid.dataManager.getData(searchUrl);
            result.done(function (value) {
                rapid.errorManager.checkDataStatus(value, function () {
                    self.data.removeAll();
                    self.data.pushAll(value.Result);
                    self.isBusy(false);
                });
                self.isBusy(false);
            });
        };

        //self.hasData is to find list has value or not.
        self.hasData = ko.computed(function () {
            if (self.data().length > 0) {
                return true;
            }
            else {
                return false;
            }
        });

        //self.filteredData which return the filtered data.
        self.filteredData = ko.computed(function () {
            var search = self.filter().toLowerCase();
            if (!search) {
                return self.data();
            }
            else {
                return ko.utils.arrayFilter(self.data(), function (item) {
                    return (stringStartSearchWith(item.name, search));
                });
            }
        });

        // stringStartSearchWith is used to find the character in the filter search in the 
        var stringStartSearchWith = function (string, startsWith) {
            if (string != undefined) {
                return (string.toLowerCase().indexOf(startsWith) !== -1);
            }
            else
                return false;
        };

        //self.create is the publish create event for the entity.
        self.create = function () {
            rapid.eventManager.publish(self.entity + "Create");
        };

        amplify.subscribe(self.entity + "Created", function () {
            self.fillData();
        });

        amplify.subscribe(self.entity + "Updated", function () {
            self.fillData();
        });

        amplify.subscribe(self.entity + "Deleted", function () {
            self.fillData();
        });

        //self.edit is the publish edit event for the entity.
        self.edit = function (item) {
            var jsonData = { entityType: self.entity, entityId: item.Id };
            rapid.eventManager.publish(self.entity + "Update", jsonData);
        };

        //self.create is the publish details event for the entity.
        self.details = function (item) {
            var jsonData = { entityType: self.entity, entityId: item.Id };
            rapid.eventManager.publish(self.entity + "Details", jsonData);
        };

        self.delete = function (item) {
            rapid.modalManager.confirm(rapid.messageManager.deleteWarningMessage(item.name), function (result) {
                if (result) {
                    var jsonData = { entityType: self.entity, entityId: item.Id };
                    rapid.eventManager.publish(self.entity + "Delete", jsonData);
                }

            });
        };
        amplify.subscribe(self.entity + "Delete", function (data) {
            var deleteResult = rapid.dataManager.postData(urls.generic.delete + data.entityType + "s/" + data.entityId);
            deleteResult.done(function (apiResult) {
                if (apiResult.Status) {
                    rapid.modalManager.alert(rapid.messageManager.deleteMessage(data.entityType));
                    rapid.eventManager.publish(self.entity + "Deleted");
                } else {
                    rapid.modalManager.alert(apiResult.Message);
                }
            });
        });

        amplify.subscribe(self.entity + "Search", function (data) {
            self.fillData(data);
        });
        //initialize by filling data
        self.fillData();
    },
    template: { element: "list-template" }
});