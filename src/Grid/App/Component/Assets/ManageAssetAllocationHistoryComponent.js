ko.components.register('allocationhistory', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("assetallocationhistory");

        self.asset = params.assetId;
    },
    template: { element: "allocationhistory-template" }
});