ko.components.register('ticketcategories', {
    viewModel: function (params) {
       var self = ManageViewModelBase(this);        
       self.init("ticketcategorie");
    },
    template: { element: "ticketcategories-template" }
});