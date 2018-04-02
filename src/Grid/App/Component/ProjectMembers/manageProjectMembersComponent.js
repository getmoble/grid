ko.components.register('manageprojectmember', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("projectmember");
        self.project = params.projectId;
      

      
    },
    template: { element: "manageprojectmember-template" }
});