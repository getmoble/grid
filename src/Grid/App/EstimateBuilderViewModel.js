function EstimateLineItem(v) {
    var self = this;

    self.Module = ko.observable(v.Module || "");
    self.Task = ko.observable(v.Task || "");
    self.Effort = ko.observable(v.Effort || 0);
    self.Comment = ko.observable(v.Comment || "");
    self.WorkType = ko.observable(v.WorkType || 2);
}

function EstimateBuilderViewModel() {

    var self = this;
    self = ViewModelBase(self);

    self.id = ko.observable();
    self.pageTitle = ko.observable();

    self.title = ko.observable();
    self.Comments = ko.observable();

    self.WorkTypes = ko.observableArray([{ Id: 1, Title: "Design" }, { Id: 2, Title: "UX" }, { Id: 3, Title: "Coding" }, { Id: 4, Title: "Testing" }]);

    self.Rows = ko.observableArray([]);

    self.addRow = function () {
        self.Rows.push(new EstimateLineItem({}));
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
        $("#createEstimateModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    };

    self.showEditModal = function () {
        $("#editEstimateModel").modal();
    };

    self.createEstimate = function () {
        self.pageTitle("New Estimate");
        self.id(-1);
        self.Rows.removeAll();
        self.Comments("");
        self.Rows.push(new EstimateLineItem({}));
        self.showAddModal();
    };

    self.postSheet = function () {
        if (self.hasMinimumHours() > 0) {

            self.isBusy(true);

            var payload =
            {
                Id: self.id(),
                Title: self.title(),
                Comments: self.Comments(),
                Rows: []
            };

            $.each(self.Rows(), function (k, v) {
                payload.Rows.push({
                    Module: v.Module(),
                    Task: v.Task(),
                    Effort: v.Effort(),
                    WorkType: v.WorkType(),
                    Comment: v.Comment()
                });
            });

            // Update or new
            if (self.id() > 1) {

                $.ajax({
                    type: "POST",
                    data: payload,
                    url: '/Estimate/UpdateSheet/',
                    success: function (status) {
                        self.isBusy(false);
                        if (!status) {
                            bootbox.alert("Seems duplicate TimeSheet");
                        } else {
                            location.reload();
                        }
                    },
                });

            } else {

                $.ajax({
                    type: "POST",
                    data: payload,
                    url: '/Estimate/CreateSheet/',
                    success: function (status) {
                        self.isBusy(false);
                        if (!status) {
                            bootbox.alert("Oops couldnt create");
                        } else {
                            location.reload();
                        }
                    },
                });
            }
        } else {
            bootbox.alert("You need to have atleast some hours in your Estimate");
        }

    };

    self.loadEstimate = function (id) {
        $.getJSON('/PMS/Estimate/GetEstimate/' + id, function (data) {
            self.Rows.removeAll();

            self.id(data.Id);
            self.title(data.Title);
            self.Comments(data.Comments);

            $.each(data.Rows, function (k, v) {
                self.Rows.push(new EstimateLineItem(v));
            });

            // 2 is approved
            if (true) {
                self.showAddModal();
            } else {
                self.showEditModal();
            }
        });
    };
};