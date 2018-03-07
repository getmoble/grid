ko.components.register('salesstages', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("crmsalesstage");
    },
    template: { element: "salesstages-template" }
});

