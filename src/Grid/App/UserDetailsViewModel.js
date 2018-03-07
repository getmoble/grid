function UserDetailsViewModel() {
    var self = this;

    self = ViewModelBase(self);

    self.userId = ko.observable();

    self.addSkill = function () {
        bootbox.confirm("Are you sure you want to add this skill", function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    data: { UserId: self.userId(), SkillId: $("#SkillId").val() },
                    url: '/Users/AddSkill/',
                    success: function () {
                        location.reload();
                    },
                });
            }
        });
    };
    
    self.addCertification = function () {
        bootbox.confirm("Are you sure you want to add this certification", function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    data: { UserId: self.userId(), CertificationId: $("#CertificationId").val() },
                    url: '/Users/AddCertification/',
                    success: function () {
                        location.reload();
                    },
                });
            }
        });
    };
    
    self.updateEntitlements = function () {
        var comments = $("#Comments").val();
        if (!comments) {
            bootbox.alert("Please add comments");
            return;
        }

        bootbox.confirm("Are you sure you want to update the entitlements", function (result) {
            if (result) {
                self.isBusy(true);

                $.ajax({
                    type: "POST",
                    data: {
                        UserId: self.userId(),
                        LeaveTimePeriodId: $("#LeaveTimePeriodId").val(),
                        LeaveTypeId: $("#LeaveTypeId").val(),
                        Operation: $("#LeaveOperation").val(),
                        Count: $("#Count").val(),
                        Comments: $("#Comments").val()
                    },
                    url: '/LMS/LeaveEntitlements/UpdateLeaveEntitlement/',
                    success: function (result) {
                        if (result) {
                            self.isBusy(false);
                            bootbox.alert("Updated Successfully", function () {
                                location.reload();
                            });
                        }
                        else {
                            self.isBusy(false);
                            bootbox.alert("Fail to Update, May be there is no enough Leave Balance");
                        }
                    },
                });
            }
        });
    }

    self.init = function (id) {
        self.userId(id);
    };
}