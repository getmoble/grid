ko.components.register('leadstatus-list', {
    viewModel: function (params) {
        var self = ListViewModelBase(this);
        self.setValues(params);
    },
    template: { fromUrl: 'List?type=leadstatuses' }
});