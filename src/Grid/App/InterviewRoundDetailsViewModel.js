function InterviewRoundActivity(activity) {
    var self = ViewModelBase(this);
    self.Id = activity.Id;
    self.Title = activity.Title;
    self.Comment = activity.Comment;
    self.CreatedOn = new Date(activity.CreatedOn).toLocaleDateString() + " " + new Date(activity.CreatedOn).toLocaleTimeString();
}

function InterviewRoundDetailsViewModel() {
    var self = this;

    self = ViewModelBase(self);

    self.id = ko.observable();
    self.activities = ko.observableArray([]);
    self.interviewRoundId = ko.observable();
    self.activity = ko.observable();

    self.addNewNote = function () {
        $.ajax({
            type: "POST",
            data: { InterviewRoundId: self.interviewRoundId(), Title: $("#newNoteTitle").val(), Comment: $("#newNote").val() },
            url: '/Recruit/InterviewRounds/AddNote/',
            success: function () {
                location.reload();
            },
        });
    };

    self.getAllActivities = function () {
        $.getJSON('/Recruit/InterviewRounds/GetAllActivitiesForInterviewRound/' + self.interviewRoundId(), function (data) {
            $.each(data, function (k, v) {
                self.activities.push(new InterviewRoundActivity(v));
            });
        });
    };

    self.init = function (id) {
        self.interviewRoundId(id);
        self.getAllActivities();
    };

    self.removeActivity = function (activity) {
        bootbox.confirm("Are you sure you want to delete it", function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: '/Recruit/InterviewRounds/DeleteNote/' + activity.Id,
                    success: function () {
                        self.activities.remove(activity);
                    },
                });
            }
        });
    };
}