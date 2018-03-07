function ProjectActivity(activity) {
   var self = ViewModelBase(this);

    self.Id = activity.Id;
    self.Title = activity.Title;
    self.Comment = activity.Comment;
    self.CreatedOn = new Date(activity.CreatedOn).toLocaleDateString() + " " + new Date(activity.CreatedOn).toLocaleTimeString();
}

function ProjectDetailsViewModel() {
    var self = this;

    self = ViewModelBase(self);
    var self = new FormViewModelBase(this);

    self.activities = ko.observableArray([]);
    self.projectId = ko.observable();
    self.activity = ko.observable();
    self.startDate = ko.observable("");
    self.endDate = ko.observable("");

    self.addNewNote = function () {
        $.ajax({
            type: "POST",
            data: { ProjectId: self.projectId(), Title: $("#newNoteTitle").val(), Comment: $("#newNote").val() },
            url: '/PMS/Projects/AddNote/',
            success: function () {
                location.reload();
                $('#addNoteModal').modal('hide');
                rapid.notificationManager.showSuccess(("Activity added successfully"));
            },
        });
    };

    self.getAllActivities = function () {
        $.getJSON('/PMS/Projects/GetAllActivitiesForProject/' + self.projectId(), function (data) {
            $.each(data, function (k, v) {
                self.activities.push(new ProjectActivity(v));
            });
        });
    };

    self.removeActivity = function (activity) {
        bootbox.confirm("Are you sure you want to delete it?", function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: '/PMS/Projects/DeleteNote/' + activity.Id,
                    success: function () {
                        self.activities.remove(activity);
                    },
                });
            }
        });
    };

    self.init = function (id) {
        self.projectId(id);
        self.getAllActivities();
    };
}