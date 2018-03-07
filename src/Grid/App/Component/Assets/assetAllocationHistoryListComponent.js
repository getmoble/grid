ko.components.register('assetallocationhistorylist', {
    viewModel: function (params) {

        var self = this;
        var self = ListViewModelBase(this);       
        self.setValues(params);

        if (params.assetId() != 0) {
            var args = [{ key: "assethistory", value: params.assetId() }];
            self.fillData(args);

        };
    },
    template: { fromUrl: 'List?type=assetallocationhistory' }
});