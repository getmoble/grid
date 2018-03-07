ko.components.register('projectslist', {
    viewModel: function (params) {
        var self = ListViewModelBase(this);
        self.setValues(params);

        self.projectId = ko.observable();

        self.projectdetails = function (row) {
            var editUrl = "/PMS/Projects/Details?id=" + row.Id;
            window.location = editUrl;
        }
      
        self.editProject = function (item) {
            $('html, body').animate({ scrollTop: 0 }, 800);
            var jsonData = { entityType: self.entity, entityId: item.Id};
            rapid.eventManager.publish(self.entity + "Update", jsonData);
        };
       
    },
    template: { fromUrl: 'List?type=projects' }
});