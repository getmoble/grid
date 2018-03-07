ko.components.register('technology', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("technologie");
    },
    template: { element: "technologies-template" }
});