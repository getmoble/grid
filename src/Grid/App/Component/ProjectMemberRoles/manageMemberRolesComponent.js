ko.components.register('projectmemberroles', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("projectmemberrole");
    },
    template: { element: "projectmemberrole-template" }
});