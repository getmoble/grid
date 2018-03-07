ko.components.register('employeesearch', {
    viewModel: function (params) {
        var self = this;

        self.entity = "leaves";

        self.requestedForUserId = ko.observable();
        self.leaveTypeId = ko.observable();
        self.duration = ko.observable();
        self.status = ko.observable();        
        self.startDate = ko.observable();
        self.approverId = ko.observable();
        self.team = ko.observable();
     


        self.filter = function () {
            var args = [{ key: "employee", value: self.requestedForUserId() },
                        { key: "leave", value: self.leaveTypeId() },
                        { key: "duration", value: self.duration() },
                        { key: "status", value: self.status() },
                        { key: "startDate", value: ko.toJS(self.startDate()) },
                        { key: "approver", value: self.approverId() },
                        { key: "team", value: self.team() }];
                        rapid.eventManager.publish(self.entity + "Search", args);
        };

        self.refresh = function (item) {
            self.requestedForUserId("");
            self.leaveTypeId("");
            self.duration("");
            self.status("");
            self.approverId("");
            self.team("");
          

            var jsonData = { entityType: self.entity, entityId: item.Id };
            rapid.eventManager.publish(self.entity + "Refresh", jsonData);
        };
    },
    template: { element: 'employeesearch-template' }
});