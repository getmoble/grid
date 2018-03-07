
ko.components.register('leavebalance', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("leavebalance");
    },
    template: { element: "leavebalance-template" }
});