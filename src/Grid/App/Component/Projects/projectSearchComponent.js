ko.components.register('projectsearch', {
    viewModel: function (params) {
        var self = this;

        self.entity = "project";

        self.title = ko.observable();
        self.status = ko.observable();
        self.clientId = ko.observable(); 
        self.projecttype = ko.observable();

        self.filter = function () {


            var args = [{ key: "projecttitle", value: self.title() },
                        { key: "status", value: self.status() },
                        { key: "clientid", value: self.clientId() },
                        { key: "projecttype", value: self.projecttype() },
                        
            ];            
            rapid.eventManager.publish(self.entity + "Search", args);
        };

        
        self.refresh = function (item) {
            self.title("");
            self.status("");
            self.clientId("");
            self.projecttype("");
            var jsonData = { entityType: self.entity, entityId: item.Id };
            rapid.eventManager.publish(self.entity + "Refresh", jsonData);
        };

        amplify.subscribe(self.entity + "UpdatedAndSearch", function () {
            self.filter();
        });


    },
    template: { element: 'projectsearch-template' }
});

