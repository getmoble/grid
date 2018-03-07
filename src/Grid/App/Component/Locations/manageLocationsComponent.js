ko.components.register('locations', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("location");
    },
    template: { element: "locations-template" }
});