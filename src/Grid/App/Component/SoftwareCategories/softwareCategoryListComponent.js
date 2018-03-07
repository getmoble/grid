ko.components.register('softwarecategorieslist', {
    viewModel: function (params) {
        var self = ListViewModelBase(this);
        self.setValues(params);
    },
    template: { fromUrl: 'List?type=softwarecategories' }
});