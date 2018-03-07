function CRMLeadActivity(activity) {
    var self = ViewModelBase(this);
    self.Id = activity.Id;
    self.Title = activity.Title;
    self.Comment = activity.Comment;
    self.CreatedOn = new Date(activity.CreatedOn).toLocaleDateString() + " " + new Date(activity.CreatedOn).toLocaleTimeString();
}

function CRMLeadDetailsViewModel() {
    var self = this;

    self = ViewModelBase(self);
    
    self.activities = ko.observableArray([]);
    self.crmLeadId = ko.observable();
    self.activity = ko.observable();

    self.addNewNote = function () {
        $.ajax({
            type: "POST",
            data: { CRMLeadId: self.crmLeadId(), Title: $("#newNoteTitle").val(), Comment: $("#newNote").val() },
            url: '/CRM/CRMLeads/AddNote/',
            success: function () {
                location.reload();
            },
        });
    };
    
    self.changeCRMLeadStatus = function () {
        bootbox.confirm("Are you sure you want to change the Status", function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    data: { CRMLeadId: self.crmLeadId(), StatusId: $('#LeadStatusId').val(), Comment: $("#statusComment").val() },
                    url: '/CRM/CRMLeads/ChangeCRMLeadStatus/',
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
                    url: '/CRM/CRMLeads/DeleteNote/' + activity.Id,
                    success: function () {
                        self.activities.remove(activity);
                    },
                });
            }
        });
    };
    
    self.getAllActivities = function () {
        $.getJSON('/CRM/CRMLeads/GetAllActivitiesForCRMLead/' + self.crmLeadId(), function (data) {
            $.each(data, function(k, v) {
                self.activities.push(new CRMLeadActivity(v));
            });
        });
    };

    self.init = function(id) {
        self.crmLeadId(id);
        self.getAllActivities();
    };
}