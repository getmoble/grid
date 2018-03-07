ko.components.register('leavehistory', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("leaves");
        
    },
    template: { element: "leaves-template" }
});
