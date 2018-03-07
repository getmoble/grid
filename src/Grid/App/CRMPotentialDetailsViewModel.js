function CRMPotentialActivity(activity) {
    var self = ViewModelBase(this);
    self.Id = activity.Id;
    self.Title = activity.Title;
    self.Comment = activity.Comment;
    self.CreatedOn = new Date(activity.CreatedOn).toLocaleDateString() + " " + new Date(activity.CreatedOn).toLocaleTimeString();
}

function CRMPotentialDetailsViewModel() {
    var self = this;

    self = ViewModelBase(self);
    
    self.activities = ko.observableArray([]);
    self.crmPotentialId = ko.observable();
    self.activity = ko.observable();

    self.addNewNote = function () {
        $.ajax({
            type: "POST",
            data: { CRMPotentialId: self.crmPotentialId(), Title: $("#newNoteTitle").val(), Comment: $("#newNote").val() },
            url: '/CRM/CRMPotentials/AddNote/',
            success: function () {
                location.reload();
            },
        });
    };
    
    self.changeSalesStage = function () {
        bootbox.confirm("Are you sure you want to change the Status", function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    data: { CRMPotentialId: self.crmPotentialId(), StatusId: $('#SalesStagesId').val(), Comment: $("#statusComment").val() },
                    url: '/CRM/CRMPotentials/ChangeSalesStage/',
                    success: function () {
                        location.reload();
                    },
                });
            }
        });
    };

    self.removeActivity = function (activity) {
        bootbox.confirm("Are you sure you want to delete it", function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: '/CRM/CRMPotentials/DeleteNote/' + activity.Id,
                    success: function () {
                        self.activities.remove(activity);
                    },
                });
            }
        });
    };

    self.getAllActivities = function () {
        $.getJSON('/CRM/CRMPotentials/GetAllActivitiesForCRMPotential/' + self.crmPotentialId(), function (data) {
            $.each(data, function(k, v) {
                self.activities.push(new CRMPotentialActivity(v));
            });
        });
    };

    self.init = function(id) {
        self.crmPotentialId(id);
        self.getAllActivities();
    };
}