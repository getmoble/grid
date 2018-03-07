ko.components.register('vendorsearch', {
    viewModel: function (params) {
        var self = this;

        self.entity = "vendor";
       
        self.name = ko.observable();

        self.filter = function () {
            var args = [{ key: "vendor", value: self.name() }];
            rapid.eventManager.publish(self.entity + "Search", args);           
        };

        self.refresh = function (item) {
            self.name("");
            var jsonData = { entityType: self.entity, entityId: item.Id };
            rapid.eventManager.publish(self.entity + "Refresh", jsonData);
        };
    },
    template: { element: 'vendorsearch-template' }
});