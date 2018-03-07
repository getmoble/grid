ko.components.register('tasks', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("task");
       
    },
    template: { element: "tasks-template" }
});