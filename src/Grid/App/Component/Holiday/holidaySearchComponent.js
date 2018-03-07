ko.components.register('holidaysearch', {
    viewModel: function (params) {
        var self = this;

        
        self.entity = "holiday";
       
        self.year = ko.observable();
        self.type = ko.observable();
        self.years = ko.observableArray();
        self.years([2017, 2016]);       

        self.filter = function () {           
            var args = [{ key: "leaveType", value: self.type() }, { key: "year", value: self.year() }];
            rapid.eventManager.publish(self.entity + "Search", args);           
        };
        self.refresh = function (item) {
            self.year("");
            self.type("");
            var jsonData = { entityType: self.entity, entityId: item.Id };
            rapid.eventManager.publish(self.entity + "Refresh", jsonData);
        };

        amplify.subscribe(self.entity + "HolidaySaveNSearch", function () {
            self.filter();
        });

    },
    template: { element: 'holidaysearch-template' }
});