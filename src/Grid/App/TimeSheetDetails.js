function TimeSheetDetailsViewModel() {

    var self = this;
    self = ViewModelBase(self);

    self.timeSheetId = ko.observable();
    
    self.approveTimeSheet = function () {
        bootbox.confirm("Are you sure you want to approve it", function (result) {
            if (result) {

                self.isBusy(true);

                $.ajax({
                    type: "POST",
                    data: { Id: self.timeSheetId(), Comments: $("#approverComments").val() },
                    url: '/TimeSheet/ApproveTimeSheet/',
                    success: function () {
                        self.isBusy(false);
                        $('#approveTimeSheet').modal('hide');
                        self.goBackAndRefresh("/PMS/TimeSheet/MyTeam");
                    },
                });
            }
        });
    };
    
    self.needsCorrectionTimeSheet = function () {
        bootbox.confirm("Are you sure you want to update it", function (result) {
            if (result) {

                self.isBusy(true);

                $.ajax({
                    type: "POST",
                    data: { Id: self.timeSheetId(), Comments: $("#needsCorrectionComments").val() },
                    url: '/TimeSheet/NeedsCorrection/',
                    success: function () {
                        self.isBusy(false);
                        $('#needsCorrection').modal('hide');
                        self.goBackAndRefresh("/PMS/TimeSheet/MyTeam");
                    },
                });
            }
        });
    };

    self.init = function(id) {
        self.timeSheetId(id);
    };
}