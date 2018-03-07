ko.components.register('potentialcategories', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("crmpotentialcategorie");
    },
    template: { element: "potentialcategories-template" }
});

