ko.components.register('ticketsubcategories', {
    viewModel: function (params) {
       var self = ManageViewModelBase(this);        
       self.init("ticketsubcategorie");
    },
    template: { element: "ticketsubcategories-template" }
});