ko.components.register('certifications', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("certification");
    },
    template: { element: "certifications-template" }
});