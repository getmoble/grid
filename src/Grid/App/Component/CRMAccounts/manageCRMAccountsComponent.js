ko.components.register('crmaccounts', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("crmaccount");
    },
    template: { element: "crmaccounts-template" }
});

