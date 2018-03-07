function DashboardViewModel() {
    var self = this;

    self = ViewModelBase(self);

    self.markMyPresence = function () {
        $.ajax({
            type: "POST",
            data: {
                Source: 20
            },
            url: '/Api/Presence/Mark',
            success: function (leaveResult) {
                if (leaveResult)
                    bootbox.alert("Marked Presence successfully");
            }
        });
    }

    self.markPresence = function () {
        bootbox.confirm("Are you sure you want to mark Presence?", function (result) {
            if (result) {
                self.markMyPresence();
            }
        });
    }

    self.checkForPresence = function () {
        $.ajax({
            type: "POST",
            url: '/Api/Presence/IsPresentToday',
            success: function (result) {
                if (!result) {
                    bootbox.confirm("You didn't mark the presence today. Mark your Presence now ?", function (result) {
                        if (result) {
                            self.markMyPresence();
                        }
                    });
                }
            },
        });
    }

    self.init = function () {
        self.checkForPresence();
    }
}