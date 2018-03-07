function CandidateActivity(activity) {
    var self = ViewModelBase(this);
    self.Id = activity.Id;
    self.Title = activity.Title;
    self.Comment = activity.Comment;
    self.CreatedOn = new Date(activity.CreatedOn).toLocaleDateString() + " " + new Date(activity.CreatedOn).toLocaleTimeString();
}

function CandidateDetailsViewModel() {
    var self = this;

    self = ViewModelBase(self);

    self.id = ko.observable();
    self.activities = ko.observableArray([]);
    self.candidateId = ko.observable();
    self.activity = ko.observable();

    self.addNewNote = function () {
        $.ajax({
            type: "POST",
            data: { CandidateId: self.candidateId(), Title: $("#newNoteTitle").val(), Comment: $("#newNote").val() },
            url: '/Recruit/Candidates/AddNote/',
            success: function () {
                location.reload();
            },
        });
    };

    self.getAllActivities = function () {
        $.getJSON('/Recruit/Candidates/GetAllActivitiesForCandidate/' + self.candidateId(), function (data) {
            $.each(data, function (k, v) {
                self.activities.push(new CandidateActivity(v));
            });
        });
    };

    self.init = function (id) {
        self.candidateId(id);
        self.getAllActivities();
    };

    self.removeActivity = function (activity) {
        bootbox.confirm("Are you sure you want to delete it", function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: '/Recruit/Candidates/DeleteNote/' + activity.Id,
                    success: function () {
                        self.activities.remove(activity);
                    },
                });
            }
        });
    };

    self.addInterviewRound = function() {
        bootbox.confirm("Are you sure you want add an Interview round", function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    data: {
                        JobOpeningId: $('#JobOpeningId').val(),
                        CandidateId: self.candidateId(),
                        RoundId: $('#RoundId').val(),
                        InterviewerIds: $('#InterviewerIds').val(),
                        ScheduledOn: $('#ScheduledOn').val(),
                        Status: $('#InterviewStatus').val(),
                        Comments: $('#Comments').val()
                    },
                    url: '/Recruit/Candidates/AddInterviewRound/',
                    success: function () {
                        location.reload();
                    },
                });
            }
        });
    }
}