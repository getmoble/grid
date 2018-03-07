ko.components.register('crmcontactssearch', {
    viewModel: function (params) {
      
        var self = this;

        self.entity = "crmcontact";

        self.account = ko.observable();
       
        self.filter = function () {         
            var args = [{ key: "account", value: self.account() }];
            rapid.eventManager.publish(self.entity + "Search", args);
        };
        self.refresh = function (item) {
            self.account("");

            var jsonData = { entityType: self.entity, entityId: item.Id };
            rapid.eventManager.publish(self.entity + "Refresh", jsonData);
        };

        amplify.subscribe(self.entity + "ContactSaveNSearch", function () {
            self.filter();
        });

    },
    template: { element: 'crmcontactssearch-template' }
});