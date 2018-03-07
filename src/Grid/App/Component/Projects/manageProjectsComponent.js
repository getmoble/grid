ko.components.register('projects', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("project");

    },
    template: { element: "projects-template" }
});