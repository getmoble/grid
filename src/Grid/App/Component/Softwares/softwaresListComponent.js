ko.components.register('softwareslist', {
    viewModel: function (params) {
        var self = ListViewModelBase(this);
        self.setValues(params);  
    },
    template: { fromUrl: 'List?type=softwares' }
});