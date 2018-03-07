ko.components.register('leavetypes', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("leavetype");
    },
    template: { element: "leavetypes-template" }
});