ko.components.register('awards', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);        
        self.init("award");
    },
    template: { element: "awards-template" }
});