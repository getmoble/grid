
ko.components.register('leavebalancelist', {
    viewModel: function (params) {
        var self = ListViewModelBase(this);
        self.setValues(params);

        


    },
    template: { fromUrl: 'List?type=leavebalance' }
});