ko.components.register('role', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("role");
    },
    template: { element: "roles-template" }
});