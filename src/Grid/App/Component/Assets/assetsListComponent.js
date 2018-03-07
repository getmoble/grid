ko.components.register('assetslist', {
    viewModel: function (params) { 
        var self = ListViewModelBase(this);
        self.setValues(params);
    },
    template: { fromUrl: 'List?type=assets' }
});