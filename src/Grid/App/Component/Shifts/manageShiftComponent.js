ko.components.register('shifts', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("shift");
    },
    template: { element: "shifts-template" }
});