ko.components.register('leaveentitlementupdatelist', {
    viewModel: function (params) {

        var self = this;
        var self = ListViewModelBase(this);


        self.setValues(params);

        if (params.employeeId() != 0) {
            var args = [{ key: "leaveEntitlement", value: params.employeeId() }];
            self.fillData(args);

        };


    },
    template: { fromUrl: 'List?type=entitlements' }
});