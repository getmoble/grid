ko.components.register('entitlementhistory', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("entitlementhistory");

        self.employee = params.employeeId;
    },
    template: { element: "entitlementhistory-template" }
});