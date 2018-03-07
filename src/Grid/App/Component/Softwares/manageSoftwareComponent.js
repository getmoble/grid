ko.components.register('softwares', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);        
        self.init("software");
    },
    template: { element: "softwares-template" }
});