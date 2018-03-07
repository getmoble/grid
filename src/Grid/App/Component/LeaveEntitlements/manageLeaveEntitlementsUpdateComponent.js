ko.components.register('leaveentitlementslog', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("entitlement");

        self.employee = params.employeeId;
    },
    template: { element: "leaveentitlementslog-template" }
});