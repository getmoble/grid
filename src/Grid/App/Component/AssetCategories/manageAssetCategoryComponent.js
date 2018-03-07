ko.components.register('assetcategories', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);        
        self.init("assetcategorie");
    },
    template: { element: "assetcategories-template" }
});