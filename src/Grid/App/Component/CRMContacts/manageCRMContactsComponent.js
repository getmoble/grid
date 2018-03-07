ko.components.register('crmcontacts', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("crmcontact");
    },
    template: { element: "crmcontacts-template" }
});

