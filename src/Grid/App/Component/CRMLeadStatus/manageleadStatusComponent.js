ko.components.register('leadstatus', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("crmleadstatu");
    },
    template: { element: "leadstatus-template" }
});

