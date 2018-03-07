ko.components.register('employeessearch', {
    viewModel: function (params) {

        var self = this;

        self.entity = "employee";

        self.employeeCode = ko.observable();
        self.departmentId = ko.observable();
        self.locationId = ko.observable();
        self.designationId = ko.observable();
        self.shiftId = ko.observable();
        self.employeeId = ko.observable();
        self.employeeStatus = ko.observable();
 
        self.filter = function () {
            var args = [{ key: "employeeCode", value: self.employeeCode() },
                        { key: "employeeDepartment", value: self.departmentId() },
                        { key: "employeeLocation", value: self.locationId() },
                        { key: "employeeDesignation", value: self.designationId() },
                        { key: "employeeShift", value: self.shiftId() },
                        { key: "employeeName", value: self.employeeId() },
                        { key: "employeeStatus", value: self.employeeStatus() }, ];
            rapid.eventManager.publish(self.entity + "Search", args);
        };
        self.refresh = function (item) {
            self.employeeCode("");
            self.departmentId("");
            self.locationId("");
            self.designationId("");
            self.shiftId("");
            self.employeeId("");
            self.employeeStatus("");
           

            var jsonData = { entityType: self.entity, entityId: item.Id };
            rapid.eventManager.publish(self.entity + "Refresh", jsonData);
        };

        amplify.subscribe(self.entity + "EmployeeSaveNSearch", function () {
            self.filter();
        });

    },
    template: { element: 'employeessearch-template' }
});