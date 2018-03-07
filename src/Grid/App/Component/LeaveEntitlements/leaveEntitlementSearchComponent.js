ko.components.register('leaveentitlementsearch', {
    viewModel: function (params) {
        var self = this;

        self.entity = "leaveentitlement";

        self.leaveTypeId = ko.observable();
        self.employeeId = ko.observable();
        self.leaveTimePeriodId = ko.observable();
        self.allocatedBy = ko.observable();
        self.operation = ko.observable();

        self.filter = function () {
            var args = [{ key: "employeeId", value: self.employeeId() },
                        { key: "leavetypeId", value: self.leaveTypeId() },
                        { key: "leaveTimePeriodId", value: self.leaveTimePeriodId() },
                        { key: "allocatedBy", value: self.allocatedBy() },
                        { key: "operation", value: self.operation() }];
            rapid.eventManager.publish(self.entity + "Search", args);
        };

        self.refresh = function (item) {
            self.employeeId("");
            self.leaveTypeId("");
            self.leaveTimePeriodId("");
            self.allocatedBy("");
            self.operation("");

            var jsonData = { entityType: self.entity, entityId: item.Id };
            rapid.eventManager.publish(self.entity + "Refresh", jsonData);
        };

        amplify.subscribe(self.entity + "EntitlementSaveNSearch", function () {
            self.filter();
        });

    },
    template: { element: 'leaveentitlementsearch-template' }
});