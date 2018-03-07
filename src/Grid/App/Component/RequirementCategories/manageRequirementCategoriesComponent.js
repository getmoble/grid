ko.components.register('requirementcategories', {
    viewModel: function (params) {
       var self = ManageViewModelBase(this);        
       self.init("requirementcategorie");
    },
    template: { element: "requirementcategories-template" }
});