ko.components.register('leaveentitlements', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("leaveentitlement");
              
    },
    template: { element: "leaveentitlements-template" }
});