ko.components.register('emergencycontacts', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("emergencycontact");

        self.employee = params.employeeId;
    },
    template: { element: "emergencycontacts-template" }
});