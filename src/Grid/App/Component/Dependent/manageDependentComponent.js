ko.components.register('dependents', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("employeedependent");

        self.employee = params.employeeId;
    },
    template: { element: "dependents-template" }
});