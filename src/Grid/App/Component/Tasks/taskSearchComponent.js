ko.components.register('tasksearch', {
    viewModel: function (params) {
        var self = this;

        self.entity = "task";

        self.status = ko.observable();
        self.projectId = ko.observable();
        self.employeeId = ko.observable();
        self.title = ko.observable();

        self.filter = function () {
            var args = [{ key: "status", value: self.status() },
                        { key: "projectid", value: self.projectId() },
                        { key: "employeeid", value: self.employeeId() },
                        { key: "title", value: self.title() }];
            rapid.eventManager.publish(self.entity + "Search", args);
        };
        self.refresh = function (item) {
            self.status("");
            self.projectId("");
            self.employeeId("");
            self.title("");
            var jsonData = { entityType: self.entity, entityId: item.Id };
            rapid.eventManager.publish(self.entity + "Refresh", jsonData);
        };

        amplify.subscribe(self.entity + "TaskSaveNSearch", function () {
            self.filter();
        });


    },
    template: { element: 'tasksearch-template' }
});

