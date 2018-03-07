function TimeSheetRow(v) {
    var self = this;
   
    self.ProjectId = ko.observable(v.ProjectId || 0);
    self.TaskSummary = ko.observable(v.TaskSummary || "");
    self.TaskId = ko.observable(v.TaskId || null);
    self.TaskBillingType = ko.observable(v.TaskBillingType);
    self.Effort = ko.observable(v.Effort || 0);
    self.WorkType = ko.observable(v.WorkType || 2);
    self.Comments = ko.observable(v.Comments || "");

    self.ProjectId.subscribe(function (id) {
        if (id) {
            $.getJSON('/PMS/TimeSheet/GetAllProjectsWorkType/' + id, function (data) {
                if (data) {
                    if (data == "NonBillable") {
                        self.WorkType(2);
                    }
                    else {
                        self.WorkType(1);
                    }

                }
            });
        }
    });
}

function MyTimeSheetViewModel() {

    var self = this;
    self = ViewModelBase(self);

    self.id = ko.observable();
    self.title = ko.observable();

    self.Date = ko.observable();
    self.Comments = ko.observable();

    self.Projects = ko.observableArray([]);
    self.WorkTypes = ko.observableArray([{ Id: 1, Title: "Billable" }, { Id: 2, Title: "Non-Billable" }]);

    self.Rows = ko.observableArray([]);

    self.autoCompleteTasks = '/PMS/Tasks/AutoCompleteTasks?query=%QUERY';

    


    self.addRow = function () {
        self.Rows.push(new TimeSheetRow({}));
    };

    self.removeRow = function (row) {
        self.Rows.remove(row);
    };

    self.hasMinimumHours = ko.computed(function () {
        var total = 0.0;
        $.each(self.Rows(), function (k, v) {
            total = total + parseFloat(v.Effort());
        });
        return total;
    });

    self.showAddModal = function () {
        $("#addTimeSheetModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    };

    self.showEditModal = function () {
        $("#editTimeSheetModal").modal();
    };

    self.addTimeSheet = function (dateTime) {
        self.title("Add Timesheet");
        self.id(-1);
        self.Rows.removeAll();
        self.Date(dateTime);
        self.Comments("");
        self.Rows.push(new TimeSheetRow({}));
        self.showAddModal();
    };

    self.postSheet = function () {
        if (self.hasMinimumHours() > 0) {

            var modifiedDate = $("#timeSheetDate").val();

            var day = moment(modifiedDate, "MM/DD/YYYY");
            if (day.toDate() > new Date()) {
                bootbox.alert("You cannot add TimeSheet for future dates");
                return;
            }

            self.isBusy(true);

            var payload =
            {
                Id: self.id(),
                Date: modifiedDate,
                Comments: self.Comments(),
                Rows: []
            };
           

            $.each(self.Rows(), function (k, v) {
                payload.Rows.push({
                    ProjectId: v.ProjectId(),
                    TaskId: v.TaskId(),
                    TaskSummary: v.TaskSummary(),
                    Effort: v.Effort(),
                    WorkType: v.WorkType(),
                    Comments: v.Comments()
                });
            });
            // Update or new
            if (self.id() > 1) {

                $.ajax({
                    type: "POST",
                    data: payload,
                    url: '/TimeSheet/UpdateSheet/',
                    success: function (status) {
                        self.isBusy(false);
                        if (!status) {
                            bootbox.alert("Seems duplicate TimeSheet");
                        } else {
                            $('#editTimeSheetModal').modal('hide');
                            location.reload();
                        }
                    },
                });

            } else {

                $.ajax({
                    type: "POST",
                    data: payload,
                    url: '/TimeSheet/CreateSheet/',
                    success: function (status) {
                        self.isBusy(false);
                        if (!status) {
                            bootbox.alert("Seems duplicate TimeSheet");
                        } else {
                            $('#addTimeSheetModal').modal('hide');
                            location.reload();
                        }
                    },
                });
            }
        } else {
            bootbox.alert("You need to have atleast some hours in your Timesheet");
        }

    };

    $.getJSON('/PMS/TimeSheet/GetAllProjectsForTimeSheet/', function (data) {
        $.each(data, function (k, v) {
            self.Projects.push(v);
        });
    });

    self.loadTimeSheet = function (id) {
        $.getJSON('/PMS/TimeSheet/GetTimeSheet/' + id, function (data) {
            self.Rows.removeAll();

            self.id(data.Id);
            self.title(data.Title);
            self.Date(data.Date);
            self.Comments(data.Comments);

            $.each(data.Rows, function (k, v) {
                self.Rows.push(new TimeSheetRow(v));
            });

            // 2 is approved
            if (data.State < 2) {
                self.showAddModal();
            } else {
                self.showEditModal();
            }
        });
    };
};