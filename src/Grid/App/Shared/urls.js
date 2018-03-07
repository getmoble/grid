function Urls() {
    var self = this;
    //General
    self.generic = {};

    //LMS
    self.holiday = {};
    self.leavePeriods = {};
    self.leaveTypes = {};
    self.leavesHistory = {};
    self.leaveEntitlement = {};

    //Company
    self.roles = {};
    self.locations = {};
    self.technologies = {};
    self.emailTemplates = {};
    self.awards = {};

    //HRMS
    self.designations = {};
    self.departments = {};  
    self.skills = {};
    self.hobbies = {};
    self.shifts = {};
    self.certifications = {};
    self.employees = {};
    self.dependent = {};
    self.emergencyContact = {};

    //PMS
    self.projects = {};
    self.projectmembers = {};

    //IMS
    self.assets = {};
    self.assetCategories = {};
    self.vendor = {};

    //IT
    self.softwares = {};
    self.softwareCategories = {};

    //Requirement
    self.requirementCategories = {};

    //Ticket Desk
    self.ticketCategories = {};
    self.ticketSubCategories = {};
    //CRM
    self.leadSource = {};
    self.leadstatus = {};
    self.leadCategory = {};
    self.potentialCategory = {};
    self.salesstages = {};
    self.crmaccounts = {};
    self.crmcontacts = {};

    //General
    self.generic.list = "/generic/GenericList/";
    self.generic.searchList = "/generic/GenericList/Search/";

    //selectedList
    self.generic.selectList = "/generic/SelectList/";

    //delete
    self.generic.delete = "/generic/Delete/";

    //Holiday
    self.holiday.update = "/Api/Holiday/Update";
    self.holiday.get = "/Api/Holiday/Get/";

    //LeaveTimePeriods
    self.leavePeriods.update = "/Api/LeavePeriods/Update";
    self.leavePeriods.get = "/Api/LeavePeriods/Get/";
 
    //LeaveTypes
    self.leaveTypes.update = "/Api/LeaveTypes/Update"; 
    self.leaveTypes.get = "/Api/LeaveTypes/Get/";

    //LeaveHistory
    self.leavesHistory.get = "/Api/Leaves/Get/";
    self.leavesHistory.checkShowApprove = "/Api/Leaves/CheckShowApprove/";
    self.leavesHistory.update = "/Api/Leaves/Update";
    self.leavesHistory.checkLeaveBalance = "/Api/Leaves/CheckLeaveBalance";
    self.leavesHistory.approve = "/Api/Leaves/Approve";
    self.leavesHistory.reject = "/Api/Leaves/Reject";
    self.leavesHistory.delete = "/Api/Leaves/Delete/";

    //LeaveEntitlements
    self.leaveEntitlement.get = "/Api/LeaveEntitlements/GetLeaveEntitlementsLog/";
    self.leaveEntitlement.getLeaveEntitlementsLog = "/Api/LeaveEntitlements/GetLeaveEntitlementsLog/";
    //self.leaveEntitlement.update = "/Api/LeaveEntitlements/Update";
    self.leaveEntitlement.getEntitlementLog = "/Api/LeaveEntitlements/GetLeaveEntitlementsLog/";
    self.leaveEntitlement.updateLog = "/Api/LeaveEntitlements/UpdateLeaveEntitlement";

    //Vendor
    self.vendor.update = "/Api/Vendors/Update";
    self.vendor.get = "/Api/Vendors/Get/";

    //Designations
    self.designations.update = "/Api/Designations/Update";
    self.designations.get = "/Api/Designations/Get/";

    //Departments
    self.departments.update = "/Api/Departments/Update";
    self.departments.get = "/Api/Departments/Get/";

    //Awards
    self.awards.update = "/Api/Awards/Update";
    self.awards.get = "/Api/Awards/Get/";

    //Roles
    self.roles.update = "/Api/Roles/Update";
    self.roles.get = "/Api/Roles/Get/";

    //Technologies
    self.technologies.update = "/Api/Technologies/Update";
    self.technologies.get = "/Api/Technologies/Get/";

    //EmailTemplates
    self.emailTemplates.update = "/Api/EmailTemplates/Update";
    self.emailTemplates.get = "/Api/EmailTemplates/Get/";
    
    //Skills
    self.skills.update = "/Api/Skills/Update";
    self.skills.get = "/Api/Skills/Get/";

    //Hobbies
    self.hobbies.update = "/Api/Hobbies/Update";
    self.hobbies.get = "/Api/Hobbies/Get/";

    //Shifts
    self.shifts.update = "/Api/Shifts/Update";
    self.shifts.get = "/Api/Shifts/Get/";

    //Certifications
    self.certifications.update = "/Api/Certifications/Update";
    self.certifications.get = "/Api/Certifications/Get/";

    //AssetCategories
    self.assetCategories.update = "/Api/AssetCategories/Update";
    self.assetCategories.get = "/Api/AssetCategories/Get/";

    //Softwares
    self.softwares.update = "/Api/Softwares/Update";
    self.softwares.get = "/Api/Softwares/Get/";

    //SoftwareCategories
    self.softwareCategories.update = "/Api/SoftwareCategories/Update";
    self.softwareCategories.get = "/Api/SoftwareCategories/Get/";

    //Assets    
    self.assets.update = "/Api/Assets/Update";
    self.assets.get = "/Api/Assets/Get/";

    //Locations    
    self.locations.update = "/Api/Locations/Update";
    self.locations.get = "/Api/Locations/Get/";

    //RequirementCategories
    self.requirementCategories.update = "/Api/RequirementCategories/Update";
    self.requirementCategories.get = "/Api/RequirementCategories/Get/";

    //TicketCategories
    self.ticketCategories.update = "/Api/TicketCategories/Update";
    self.ticketCategories.get = "/Api/TicketCategories/Get/";

    //TicketSubCategories
    self.ticketSubCategories.update = "/Api/TicketSubCategories/Update";
    self.ticketSubCategories.get = "/Api/TicketSubCategories/Get/";

    //Employees    
    self.employees.update = "/Api/Employees/Create";
    self.employees.get = "/Api/Employees/Get/";
    self.employees.getRoleAndTechnologyName = "/Api/Employees/GetRoleAndTechnologyNames/";
    self.employees.resetPassword = "/Api/Employees/ResetPassword";
    self.employees.Uploads = "/Api/Employees/UploadProfileImage"

    
    //Dependents
    self.dependent.update = "/Api/EmployeeDependents/Update";
    self.dependent.get = "/Api/EmployeeDependents/Get/";

    //EmergencyContact
    self.emergencyContact.update = "/Api/EmergencyContacts/Update";
    self.emergencyContact.get = "/Api/EmergencyContacts/Get/";

    //Projects    
    self.projects.update = "/Api/Project/Update";
    self.projects.get = "/Api/Project/Get/";

    //Project Members
    self.projectmembers.update = "/Api/ProjectMember/Update";
    self.projectmembers.get = "/Api/ProjectMember/Get/";
    self.projectmembers.create = "/PMS/ProjectMembers/Create";

    //CRM 
    //Lead Sources
    self.leadSource.update = "/Api/LeadSources/Update";
    self.leadSource.get = "/Api/LeadSources/Get/";

    //Lead Sources
    self.leadstatus.update = "/Api/LeadStatus/Update";
    self.leadstatus.get = "/Api/LeadStatus/Get/";
    
    //Lead Category
    self.leadCategory.update = "/Api/LeadCategories/Update";
    self.leadCategory.get = "/Api/LeadCategories/Get/";

    //Potential Category
    self.potentialCategory.update = "/Api/PotentialCategories/update";
    self.potentialCategory.get = "/Api/PotentialCategories/Get/"

    //SalesStages
    self.salesstages.update = "/Api/SalesStages/update";
    self.salesstages.get = "/Api/SalesStages/Get/"

    //CRMAccounts
    self.crmaccounts.update = "/Api/CRMAccounts/update";
    self.crmaccounts.get = "/Api/CRMAccounts/Get/"

    //CRMContacts
    self.crmcontacts.update = "/Api/Contacts/update";
    self.crmcontacts.get = "/Api/Contacts/Get/"
    
    
};
urls = new Urls();