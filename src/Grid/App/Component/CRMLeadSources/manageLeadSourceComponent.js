ko.components.register('leadsource', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("crmleadsource");
    },
    template: { element: "leadsource-template" }
});

