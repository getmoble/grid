ko.components.register('holiday', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("holiday");
    },
    template: { element: "holiday-template" }
});


