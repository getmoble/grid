ko.components.register('searchleavebalance', {
    viewModel: function (params) {
        var self = this;

        self.entity = "leavebalance";

        self.employeeDepartment = ko.observable();
        self.employeeId = ko.observable();       

        self.filter = function () {
            var args = [{ key: "employeeId", value: self.employeeId() },
                        { key: "employeeDepartment", value: self.employeeDepartment() }];
            rapid.eventManager.publish(self.entity + "Search", args);
        };

        self.refresh = function (item) {
            self.employeeId("");
            self.employeeDepartment("");
           
            var jsonData = { entityType: self.entity, entityId: item.Id };
            rapid.eventManager.publish(self.entity + "Refresh", jsonData);
        };
    },
    template: { element: 'searchleavebalance-template' }
});