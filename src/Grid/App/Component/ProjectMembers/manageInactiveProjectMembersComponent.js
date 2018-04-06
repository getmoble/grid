ko.components.register('manageinactiveprojectmember', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("inactiveprojectmember");
        self.project = params.projectId;
    },
    template: { element: "manageinactiveprojectmember-template" }
});