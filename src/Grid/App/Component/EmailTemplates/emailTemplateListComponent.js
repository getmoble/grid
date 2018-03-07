ko.components.register('emailtemplateslist', {
    viewModel: function (params) {
        var self = ListViewModelBase(this);
        self.setValues(params);            
    },
    template: { fromUrl: 'List?type=emailtemplates' }
});