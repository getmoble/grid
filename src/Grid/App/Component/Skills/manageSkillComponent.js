ko.components.register('skills', {
    viewModel: function (params) {
       var self = ManageViewModelBase(this);        
       self.init("skill");
    },
    template: { element: "skills-template" }
});