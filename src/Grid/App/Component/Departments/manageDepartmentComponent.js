ko.components.register('departments', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("department");
    },
    template: { element: "departments-template" }
});