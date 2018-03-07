function LeaveHistory(data) {
    var self = ModelBase(this);

    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };
    var date = new Date();
    self.id = ko.observable(data.Id || 0);
    self.requestedForUserId = ko.observable(0).extend({ required: { params: true, message: messages.leaveHistory.RequestedForUserId } });
    self.title = ko.observable();
    self.name = ko.observable(data.Name);
    self.reason = ko.observable(data.Reason).extend({ required: { params: true, message: messages.leaveHistory.Reason } });
    self.date = ko.observable(data.CreatedOn);
    self.type = ko.observable(data.Type);
    self.leaveType = ko.observable(data.LeaveType);
    self.team = ko.observable();   
    self.duration = ko.observable(data.LeaveDuration).extend({ required: { params: true, message: messages.leaveHistory.Duration } });
    self.status = ko.observable(data.LeaveStatus);
    self.end = ko.observable(data.End || date).extend({
        required: {
            params: true, message: "Please enter End Date", onlyIf: function () {
                if (self.duration() == 0) {
                    return true;
                } else {
                    return false;
                }
            }
        }
    });
    self.start = ko.observable(data.Start || date).extend({
        required: {
            params: true, message: "Please enter Start Date"}
    });

   
    self.actedOn = ko.observable(data.ActedOn);
    self.approver = ko.observable();
    self.leaveTypeId = ko.observable().extend({ required: { params: true, message: messages.leaveHistory.LeaveTypeId } });
    self.visibleDate = ko.observable(false);
    self.isVisible = ko.observable(false);
    self.isShow = ko.observable(false);
    self.appliedBy = ko.observable(false);
    self.leaveCount = ko.observable();
    self.isApprovedOrReject = ko.observable(false);
    self.userId = ko.observable(data.UserId);
    self.isApprove = ko.observable(false);
 

    self.period = ko.computed(function () {
        return self.start() + "-" + self.end();
    });
    self.approverComments = ko.observable(data.ApproverComments);

    if (data.LeaveType) {
        self.leaveType(data.LeaveType.Title);
    }
    if (data.CreatedByUser) {
        if (data.CreatedByUser.Person) {
            self.name(data.CreatedByUser.Person.Name);
        }
    }
    if (data.RequestedForUser) {
        if (data.RequestedForUser.User) {
            if (data.RequestedForUser.User.Person) {
                self.name(data.RequestedForUser.User.Person.Name);
            }
        }
    }
    if (data.Duration == "0") {
        self.duration("Multiple Days");
    } else if (data.Duration == "1") {
        self.duration("One Full Day");
    } else if (data.Duration == "2") {
        self.duration("First Half");
    } else if (data.Duration == "3") {
        self.duration("Second Half");
    }

    if (data.Status == "0") {
        self.status("Pending");
    } else if (data.Status == "1") {
        self.status("Approved");
        self.isApprovedOrReject(true);
    } else if (data.Status == "2") {
        self.status("Rejected");
        self.isApprovedOrReject(true);
    }

    if (data.Approver) {
        if (data.Approver.User) {
            if (data.Approver.User.Person) {
                self.approver(data.Approver.User.Person.Name);
            }
        }
    }

    self.appliedBy.subscribe(function (value) {
        if (value) {
            self.isVisible(true);
        } else {
            self.isVisible(false);
            self.requestedForUserId(0);
        }
    });

    self.duration.subscribe(function (value) {
        if (value == "0") {
            self.visibleDate(true);
        } else {
            self.visibleDate(false);
        }
    });

    self.modelState = ko.validatedObservable(
 {
     Reason: self.reason,
     LeaveTypeId: self.leaveTypeId,
     Duration: self.duration,
     RequestedForUserId: self.requestedForUserId,
     Start: self.start,
     End: self.end


 });
    self.resetValidation = function () {
        self.reason.isModified(false);
        self.leaveTypeId.isModified(false);
        self.duration.isModified(false);
        self.start.isModified(false);
        self.end.isModified(false);
        self.requestedForUserId.isModified(false)
       
    };
};
