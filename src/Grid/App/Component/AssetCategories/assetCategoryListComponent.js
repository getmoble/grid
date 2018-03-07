ko.components.register('assetcategorieslist', {
    viewModel: function (params) {
        var self = ListViewModelBase(this);
        self.setValues(params);  
    },
    template: { fromUrl: 'List?type=assetcategories' }
});