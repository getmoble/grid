function RequirementActivity(activity) {
    var self = ViewModelBase(this);
    self.Id = activity.Id;
    self.Title = activity.Title;
    self.Comment = activity.Comment;
    self.CreatedOn = new Date(activity.CreatedOn).toLocaleDateString() + " " + new Date(activity.CreatedOn).toLocaleTimeString();
}

function RequirementDetailsViewModel() {
    var self = this;

    self = ViewModelBase(self);
    
    self.activities = ko.observableArray([]);
    self.requirementId = ko.observable();
    self.activity = ko.observable();
    self.currentDate = ko.observable(new Date().toLocaleDateString() + " " + new Date().toLocaleTimeString());
    self.technicalReviewComment = ko.observable();

    self.submitForTechnicalReview = function() {
        bootbox.confirm("Are you sure you want to submit for Review", function(result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    data: { RequirementId: self.requirementId(), Comment: $("#techReviewRequest").val() },
                    url: '/RMS/Requirements/SubmitForTechnicalReview/',
                    success: function() {
                        location.reload();
                    },
                });
            }
        });
    };
    
    self.submitTechnicalReviewContent = function () {
        bootbox.confirm("Are you sure you want to submit this as Technical Review Content", function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    data: { RequirementId: self.requirementId(), Comment: $("#techReviewContent").val() },
                    url: '/RMS/Requirements/SubmitTechnicalReviewContent/',
                    success: function () {
                        location.reload();
                    },
                });
            }
        });
    };
    
    self.submitProposal = function () {
        bootbox.confirm("Are you sure you want to submit this Proposal", function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    data: { RequirementId: self.requirementId(), Comment: $("#proposalContent").val() },
                    url: '/RMS/Requirements/SubmitProposal/',
                    success: function () {
                        location.reload();
                    },
                });
            }
        });
    };
    
    self.changeRequirementStatus = function () {
        bootbox.confirm("Are you sure you want to change the Status", function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    data: { RequirementId: self.requirementId(), Status: $('#RequirementStatus').val(), Comment: $("#statusComment").val() },
                    url: '/RMS/Requirements/ChangeRequirementStatus/',
                    success: function () {
                        location.reload();
                    },
                });
            }
        });
    };

    self.addNewNote = function () {
        $.ajax({
            type: "POST",
            data: {
                RequirementId: self.requirementId(),
                Title: $("#newNoteTitle").val(),
                Comment: $("#newNote").val(),
                CreatedOn: $("#activityDate").val()
            },
            url: '/RMS/Requirements/AddNote/',
            success: function () {
                location.reload();
            },
        });
    };
    
    self.removeActivity = function (activity) {
        bootbox.confirm("Are you sure you want to delete it", function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: '/RMS/Requirements/DeleteRequirementActivity/' + activity.Id,
                    success: function() {
                        self.activities.remove(activity);
                    },
                });
            }
        });
    };

    self.getAllActivities = function () {
        $.getJSON('/RMS/Requirements/GetAllActivitiesForRequirement/' + self.requirementId(), function (data) {
            $.each(data, function(k, v) {
                self.activities.push(new RequirementActivity(v));
            });
        });
    };

    self.init = function(id) {
        self.requirementId(id);
        self.getAllActivities();
    };
}