ko.components.register('leadcategories', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("crmleadcategorie");
    },
    template: { element: "leadcategories-template" }
});

