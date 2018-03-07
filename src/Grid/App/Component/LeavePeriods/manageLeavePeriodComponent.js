
ko.components.register('leave-period', {
    viewModel: function (params) {
        var self = ManageViewModelBase(this);
        self.init("leaveTimePeriod");
    },
    template: { element: "leaveperiod-template" }
});


