ko.components.register('assets', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("asset");
    },
    template: { element: "assets-template" }
});