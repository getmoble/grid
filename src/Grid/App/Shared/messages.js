
function Messages() {
    var self = this;

    self.general = {};
    self.leaveType = {};
    self.leaveHistory = {};   
    self.holiday = {};
    self.leavePeriod = {};
    self.department = {};
    self.designation = {};
    self.technology = {};
    self.emailTemplate = {};
    self.software = {};
    self.asset = {};
    self.employee = {};
    self.leaveEntitlements = {};

    self.vendor = {};

    //General  
    self.general.Title = "Please enter Title";
    self.general.Select = "Please choose an option";
    self.general.Name = "Please enter Name";
    self.general.ValidMail = "Please enter a valid Email address";
    self.general.Mail = "Please enter Email address";
    self.general.Mobile = "Please enter Phone Number";

    //LeaveEntitlements
    self.leaveEntitlements.Allocation = "Please enter Allocated Days";

    //LeaveHistory
    self.leaveHistory.Reason = "Please enter Reason";
    self.leaveHistory.LeaveTypeId = "Please choose an option";
    self.leaveHistory.Duration = "Please choose an option";
    self.leaveHistory.Start = "Please enter Start Date";
    self.leaveHistory.End = "Please enter End Date";
    self.leaveHistory.RequestedForUserId = "Please choose an option";

    //LeaveType
    self.leaveType.Title = "Please enter Title";
    self.leaveType.MaxInAStretch = "Please enter Max In A Stretch";
    self.leaveType.MaxInMonth = "Please enter Max In Month";
    self.leaveType.MaxCarryForward = "Please enter Max Carry Forward";
    
    //Holiday
    self.holiday.Title = "Please enter Title";
    self.holiday.Type = "Please choose an option";

    //LeavePeriod
    self.leavePeriod.Title = "Please enter Title";

    //Vendor
    self.vendor.Title = "Please enter Title";

    //Department
    self.department.Title = "Please enter Title";
    self.department.MailAlias = "Please enter a valid Email address";

    //Designation
    self.designation.Title = "Please enter Title";
    self.designation.DepartmentId = "Please choose an option";
    self.designation.Band = "Please choose an option";
    self.designation.MailAlias = "Please enter a valid Email address";

    //Technology
    self.technology.Category = "Please enter Category";

    //EmailTemplate
    self.emailTemplate.Content = "Please enter content";

    //Software
    self.software.Version = "Please enter version";
    self.software.LicenseType = "Please choose an option";
    self.software.Status = "Please choose an option";
    self.software.SoftwareCategoryId = "Please choose an option";
 
    //Asset
    self.asset.State = "Please choose an option";

    //Employee
    self.employee.EmployeeCode = "Please enter Employee Code";
    self.employee.Username = "Please enter Username";
    self.employee.Password = "Please enter Password";
    self.employee.NewPassword = "Please enter New Password";
    self.employee.CofirmNewPassword = "Please re-enter Password";
    self.employee.FirstName = "Please enter FirstName";
    self.employee.LastName = "Please enter LastName";

   
};
messages = new Messages();