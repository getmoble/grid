
ko.components.register('designations', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("designation");
    },
    template: { element: "designations-template" }
});