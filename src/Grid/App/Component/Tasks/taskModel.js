function Task(data) {
    var self = ModelBase(this);
    self.modelState = ko.validatedObservable();
    self.isValid = function () {
        return self.modelState.isValid();
    };
    self.showErrors = function () {
        self.modelState.errors.showAllMessages(true);
    };


    self.id = ko.observable(data.Id || 0);
    self.title = ko.observable(data.Title).extend({ required: { params: true, message: messages.general.Title } });
    self.description = ko.observable(data.Description);
    self.expectedTime = ko.observable(data.ExpectedTime);
    self.startDate = ko.observable(data.StartDate);
    self.startDate = ko.observable(self.parseDate(data.StartDate) || "");
    self.dueDate = ko.observable(self.parseDate(data.DueDate) || "");
    self.assigneeId = ko.observable(data.AssigneeId).extend({ required: { params: true, message: messages.general.Select } });
    self.assignee = ko.observable();
    self.projectId = ko.observable(data.ProjectId).extend({ required: { params: true, message: messages.general.Select } });
    self.project = ko.observable();
    self.taskStatus = ko.observable(data.TaskStatus).extend({ required: { params: true, message: messages.general.Select } });
    self.taskStatusType = ko.observable();
    self.priority = ko.observable(data.Priority).extend({ required: { params: true, message: messages.general.Select } });
    self.priorityType = ko.observable();
    self.taskBilling = ko.observable(data.TaskBilling).extend({ required: { params: true, message: messages.general.Select } });
    self.taskBillingType = ko.observable();
   
    if (data.TaskBilling == 0) {
        self.taskBillingType("NonBillable");
    }
    else if (data.TaskBilling == 1) {
        self.taskBillingType("Billable");
    }

    if (data.Priority == 0) {
        self.priorityType("Low");
    }
    else if (data.Priority == 1) {
        self.priorityType("Normal");
    } else if (data.Priority == 2) {
        self.priorityType("High");
    }
    else if (data.Priority == 3) {
        self.priorityType("Urgent");
    } else if (data.Priority == 4) {
        self.priorityType("Immediate");
    }


    if (data.taskStatus == 1) {
        self.taskStatusType("Notstarted");
    } else if (data.taskStatus == 2) {
        self.taskStatusType("Started");
    }
    else if (data.taskStatus == 3) {
        self.taskStatusType("Completed");
    } else if (data.Priority == 4) {
        self.priorityType("Cancelled");
    }


    self.modelState = ko.validatedObservable(
{
    AssigneeId: self.assigneeId,
    ProjectId: self.projectId,
    Title: self.title,
    TaskStatus: self.taskStatus,
    Priority: self.priority,
    TaskBilling: self.taskBilling
});
    self.resetValidation = function () {
        self.title.isModified(false);
        self.assigneeId.isModified(false);
        self.projectId.isModified(false);
        self.taskStatus.isModified(false);
        self.priority.isModified(false);
        self.taskBilling.isModified(false);

    };
};