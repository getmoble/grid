ko.components.register('entitlementhistorylist', {
    viewModel: function (params) {

        var self = this;
        var self = ListViewModelBase(this);


        self.setValues(params);

        if (params.employeeId() != 0) {
            var args = [{ key: "entitlementhistory", value: params.employeeId() }];
            self.fillData(args);

        };

    },
    template: { fromUrl: 'List?type=entitlementhistorylists' }
});