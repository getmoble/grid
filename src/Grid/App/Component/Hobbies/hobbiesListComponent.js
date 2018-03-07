ko.components.register('hobbieslist', {
    viewModel: function (params) {
        var self = ListViewModelBase(this);
        self.setValues(params);
    },
    template: { fromUrl: 'List?type=hobbies' }
});