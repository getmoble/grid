namespace Grid.Infrastructure
{
    public static class UrlGenerator
    {
        public static string CreatePage(EntityType entityType)
        {
            string url = "#";
            switch (entityType)
            {
                case EntityType.Location:
                    url = "/Company/Locations/Create"; 
                    break;
                case EntityType.Permission:
                    url = "/Company/Permissions/Create";
                    break;
                case EntityType.Role:
                    url = "/Company/Roles/Create";
                    break;
                case EntityType.Technology:
                    url = "/Company/Technologies/Create";
                    break;
                case EntityType.EmailTemplate:
                    url = "/Company/EmailTemplates/Create";
                    break;
                case EntityType.Award:
                    url = "/Company/Awards/Create";
                    break;
                case EntityType.Department:
                    url = "/HRMS/Departments/Create";
                    break;
                case EntityType.Designation:
                    url = "/HRMS/Designations/Create";
                    break;
                case EntityType.Shift:
                    url = "/HRMS/Shifts/Create";
                    break;
                case EntityType.Skill:
                    url = "/HRMS/Skills/Create";
                    break;
                case EntityType.Hobby:
                    url = "/HRMS/Hobbies/Create";
                    break;
                case EntityType.Certification:
                    url = "/HRMS/Certifications/Create";
                    break;
                case EntityType.Employee:
                    url = "/HRMS/Users/Create";
                    break;
                case EntityType.JobOpening:
                    url = "/Recruit/JobOpenings/Create";
                    break;
                case EntityType.CandidateDesignation:
                    url = "/Recruit/CandidateDesignations/Create";
                    break;
                case EntityType.Candidate:
                    url = "/Recruit/Candidates/Create";
                    break;
                case EntityType.Referal:
                    url = "/Recruit/Referals/Create";
                    break;
                case EntityType.Round:
                    url = "/Recruit/Rounds/Create";
                    break;
                case EntityType.Interview:
                    url = "/Recruit/InterviewRounds/Create";
                    break;
                case EntityType.Offer:
                    url = "/Recruit/JobOffers/Create";
                    break;
                case EntityType.Holiday:
                    url = "/LMS/Holidays/Create";
                    break;
                case EntityType.LeaveType:
                    url = "/LMS/LeaveTypes/Create";
                    break;
                case EntityType.LeavePeriod:
                    url = "/LMS/LeavePeriods/Create";
                    break;
                case EntityType.Leave:
                    url = "/LMS/Leaves/Create";
                    break;
                case EntityType.Vendor:
                    url = "/IMS/Vendors/Create";
                    break;
                case EntityType.AssetCategory:
                    url = "/IMS/AssetCategories/Create";
                    break;
                case EntityType.Asset:
                    url = "/IMS/Assets/Create";
                    break;
                case EntityType.SoftwareCategory:
                    url = "/IT/SoftwareCategories/Create";
                    break;
                case EntityType.Software:
                    url = "/IT/Softwares/Create";
                    break;
                case EntityType.LeadSource:
                    url = "/CRM/CRMLeadSources/Create";
                    break;
                case EntityType.LeadStatus:
                    url = "/CRM/CRMLeadStatus/Create";
                    break;
                case EntityType.LeadCategory:
                    url = "/CRM/CRMLeadSources/Create";
                    break;
                case EntityType.Lead:
                    url = "/CRM/CRMLeads/Create";
                    break;
                case EntityType.SaleStage:
                    url = "/CRM/CRMSaleStages/Create";
                    break;
                case EntityType.PotentialCategory:
                    url = "/CRM/CRMPotentialCategories/Create";
                    break;
                case EntityType.Potential:
                    url = "/CRM/CRMPotentials/Create";
                    break;
                case EntityType.Account:
                    url = "/CRM/CRMAccounts/Create";
                    break;
                case EntityType.Contact:
                    url = "/CRM/CRMContacts/Create";
                    break;
                case EntityType.RequirementCategory:
                    url = "/RMS/RequirementCategories/Create";
                    break;
                case EntityType.Requirement:
                    url = "/RMS/Requirements/Create";
                    break;
                case EntityType.Project:
                    url = "/PMS/Projects/Create";
                    break;
                case EntityType.Task:
                    url = "/PMS/Tasks/Create";
                    break;
                case EntityType.ArticleCategory:
                    url = "/KBS/Categories/Create";
                    break;
                case EntityType.Article:
                    url = "/KBS/Articles/Create";
                    break;
                case EntityType.TicketCategory:
                    url = "/TicketDesk/TicketCategories/Create";
                    break;
                case EntityType.TicketSubCategory:
                    url = "/TicketDesk/TicketSubCategories/Create";
                    break;
                case EntityType.Ticket:
                    url = "/TicketDesk/Tickets/Create";
                    break;
                case EntityType.TimeSheet:
                    url = "/PMS/TimeSheet/MyTimeSheet";
                    break;
            }

            return url;
        }
    }
}
