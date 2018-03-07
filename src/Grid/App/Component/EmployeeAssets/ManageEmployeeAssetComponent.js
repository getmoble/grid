ko.components.register('employeeasset', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("asset");

        self.employee = params.employeeId;
    },
    template: { element: "employeeasset-template" }
});