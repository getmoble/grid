function ApplyLeaveViewModel() {
    var self = this;
    self.checkLeaveBalance = function () {
        var leaveTypeId = $("#LeaveTypeId").val();
        var duration = $("#Duration").val();
        var start = $("#Start").val();
        var end = $("#End").val();
        var reason = $("#Reason").val();

        // Check if the end date is empty, set to start date
        var trimmedValue = $.trim($('#End').val());
        if (duration > 0 && trimmedValue.length == 0) {
            $("#End").val(start);
            end = start;
        }

        var startDate = new Date(start);
        var endDate = new Date(end);

        if (!reason) {
            bootbox.alert("Please fill out a reason");
            return;
        }

        if (endDate >= startDate) {
            $.ajax({
                type: "POST",
                data: {
                    LeaveTypeId: leaveTypeId,
                    Duration: duration,
                    StartDate: start,
                    EndDate: end
                },
                url: '/LMS/Leaves/CheckLeaveBalance',
                success: function (result) {
                    if (result.Status) {
                        var deducationMessage = "You are applying for " + result.RequestedDays + " days leave which will be deducted from your Leave Balance of " + result.CurrentLeaveBalance + " days";
                        bootbox.confirm("Are you sure you want to apply leave? " + deducationMessage, function (dialogResult) {
                            if (dialogResult) {
                                // Create the leave
                                $.ajax({
                                    type: "POST",
                                    data: {
                                        LeaveTypeId: leaveTypeId,
                                        Duration: duration,
                                        StartDate: start,
                                        EndDate: end,
                                        Reason: reason,
                                        LeaveCount: result.RequestedDays
                                    },
                                    url: '/LMS/Leaves/Create',
                                    success: function (leaveResult) {
                                        if (leaveResult)
                                            window.location.href = "/LMS/Leaves/MyLeaves";
                                    }
                                });
                            }
                        });

                    } else {
                        bootbox.alert(result.Message);
                    }
                },
            });

        } else {
            bootbox.alert("End Date should be greater than Start Date");
        }
    };

    $(function () {
        $('#Duration').change(
            function () {
                var val1 = $('#Duration option:selected').val();
                if (val1 > 0) {
                    $("#endDateGroup").hide();
                    $('#startDate').text("Date");
                }
                else {
                    $('#startDate').text("Start Date");
                    $("#endDateGroup").show();
                }
            }
        );
    });
}