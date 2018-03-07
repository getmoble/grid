function TicketActivity(activity) {
    var self = ViewModelBase(this);
    self.Id = activity.Id;
    self.Title = activity.Title;
    self.Comment = activity.Comment;
    self.CreatedOn = new Date(activity.CreatedOn).toLocaleDateString() + " " + new Date(activity.CreatedOn).toLocaleTimeString();
}

function TicketDetailsViewModel() {
    var self = this;

    self = ViewModelBase(self);
    
    self.activities = ko.observableArray([]);
    self.ticketId = ko.observable();
    self.activity = ko.observable();

    self.addNewNote = function () {
        $.ajax({
            type: "POST",
            data: { TicketId: self.ticketId(), Title: $("#newNoteTitle").val(), Comment: $("#newNote").val() },
            url: '/TicketDesk/Tickets/AddNote/',
            success: function () {
                location.reload();
            },
        });
    };
    
    self.getAllActivities = function () {
        $.getJSON('/TicketDesk/Tickets/GetAllActivitiesForTicket/' + self.ticketId(), function (data) {
            $.each(data, function(k, v) {
                self.activities.push(new TicketActivity(v));
            });
        });
    };

    self.removeActivity = function (activity) {
        bootbox.confirm("Are you sure you want to delete it", function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: '/Tickets/DeleteNote/' + activity.Id,
                    success: function () {
                        self.activities.remove(activity);
                    },
                });
            }
        });
    };

    self.init = function(id) {
        self.ticketId(id);
        self.getAllActivities();
    };
}