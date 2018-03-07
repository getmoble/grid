ko.components.register('designationsearch', {
    viewModel: function (params) {
      
        var self = this;

        self.entity = "designation";

        self.departmentId = ko.observable();       
        self.band = ko.observable();
       
        self.filter = function () {         
            var args = [{ key: "department", value: self.departmentId() }, { key: "band", value: self.band() }];
            rapid.eventManager.publish(self.entity + "Search", args);
        };
        self.refresh = function (item) {
            self.departmentId("");
            self.band("");

            var jsonData = { entityType: self.entity, entityId: item.Id };
            rapid.eventManager.publish(self.entity + "Refresh", jsonData);
        };

        amplify.subscribe(self.entity + "DesignationSaveNSearch", function () {
            self.filter();
        });

    },
    template: { element: 'designationsearch-template' }
});