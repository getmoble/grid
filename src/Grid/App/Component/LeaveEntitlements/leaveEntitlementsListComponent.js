ko.components.register('leaveentitlementlist', {
    viewModel: function (params) {
        var self = ListViewModelBase(this);
        self.setValues(params);
    },
    template: { fromUrl: 'List?type=leaveentitlements' }
});