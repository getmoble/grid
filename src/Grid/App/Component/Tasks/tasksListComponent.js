ko.components.register('taskslist', {
    viewModel: function (params) {
        var self = ListViewModelBase(this);
        self.setValues(params);

        self.taskDetails = function (row) {
            var editUrl = "/PMS/Tasks/Details?id=" + row.Id;
            window.location = editUrl;
        }

    },
    template: { fromUrl: 'List?type=tasks' }
});