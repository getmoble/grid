ko.components.register('hobbies', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("hobbie");
    },
    template: { element: "hobbies-template" }
});