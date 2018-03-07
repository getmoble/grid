ko.components.register('emailtemplates', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("emailtemplate");
    },
    template: { element: "emailtemplates-template" }
});