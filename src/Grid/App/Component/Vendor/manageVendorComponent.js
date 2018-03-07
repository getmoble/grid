ko.components.register('vendor', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);        
        self.init("vendor");
    },
    template: { element: "vendor-template" }
});


