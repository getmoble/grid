ko.components.register('employees', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("employee");
    },
    template: { element: "employees-template" }
});