ko.components.register('softwarecategories', {
    viewModel: function (params) {
       var self = ManageViewModelBase(this);        
       self.init("softwarecategorie");
    },
    template: { element: "softwareCategories-template" }
});