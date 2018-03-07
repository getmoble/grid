define(["require", "exports", 'knockout', 'knockoutCustom', 'komapping', 'signals', './router', 'jquery', 'pagedViewModelBase'], function (require, exports, ko, kocustom, komapping, signals, router, $, pagedViewModelBase) {
    "use strict";

    ko.components.register('userimage', {
        viewModel: function (params) {

            var self = this;

            self.isBusy = ko.observable();
            self.image = ko.observable();
            self.name = ko.observable();

            if (typeof (params.UserId) !== "undefined" && params.UserId !== null) {
                self.isBusy(true);
                var apiUrl = '/Api/Employee/ProfileImage/' + params.UserId;
                $.getJSON(apiUrl, function (data) {
                    self.image(data.ImageUrl);
                    self.name(data.Name);
                    self.isBusy(false);
                });
            }
        },
        template: '<div data-bind="loadingWhen: isBusy"><img data-bind="attr: { src: image, alt: name }" width="300"></div>'
    });

    // Company
    ko.components.register('locationlist', {
        template: { require: 'text!Templates/List?type=Locations' },
        viewModel: { require: 'App/Components/Company/Locations/LocationListViewModel' }
    });
    ko.components.register('locationupdate', {
        template: { require: 'text!Templates/Form?type=Locations' },
        viewModel: { require: 'App/Components/Company/Locations/LocationUpdateViewModel' }
    });

    ko.components.register('permissionlist', {
        template: { require: 'text!Templates/List?type=Permissions' },
        viewModel: { require: 'App/Components/Company/Permissions/PermissionListViewModel' }
    });

    ko.components.register('rolelist', {
        template: { require: 'text!Templates/List?type=Roles' },
        viewModel: { require: 'App/Components/Company/Roles/RoleListViewModel' }
    });
    ko.components.register('roleupdate', {
        template: { require: 'text!Templates/Form?type=Roles' },
        viewModel: { require: 'App/Components/Company/Roles/RoleUpdateViewModel' }
    });

    ko.components.register('technologylist', {
        template: { require: 'text!Templates/List?type=Technologies' },
        viewModel: { require: 'App/Components/Company/Technologies/TechnologyListViewModel' }
    });
    ko.components.register('technologyupdate', {
        template: { require: 'text!Templates/Form?type=Technologies' },
        viewModel: { require: 'App/Components/Company/Technologies/TechnologyUpdateViewModel' }
    });

    ko.components.register('emailtemplatelist', {
        template: { require: 'text!Templates/List?type=EmailTemplates' },
        viewModel: { require: 'App/Components/Company/EmailTemplates/EmailTemplateListViewModel' }
    });
    ko.components.register('emailtemplateupdate', {
        template: { require: 'text!Templates/Form?type=EmailTemplates' },
        viewModel: { require: 'App/Components/Company/EmailTemplates/EmailTemplateUpdateViewModel' }
    });

    ko.components.register('awardlist', {
        template: { require: 'text!Templates/List?type=Awards' },
        viewModel: { require: 'App/Components/Company/Awards/AwardListViewModel' }
    });
    ko.components.register('awardupdate', {
        template: { require: 'text!Templates/Form?type=Awards' },
        viewModel: { require: 'App/Components/Company/Awards/AwardUpdateViewModel' }
    });

    // HRMS
    ko.components.register('departmentlist', {
        template: { require: 'text!Templates/List?type=Departments' },
        viewModel: { require: 'App/Components/HRMS/Departments/DepartmentListViewModel' }
    });
    ko.components.register('departmentupdate', {
        template: { require: 'text!Templates/Form?type=Departments' },
        viewModel: { require: 'App/Components/HRMS/Departments/DepartmentUpdateViewModel' }
    });

    ko.components.register('designationlist', {
        template: { require: 'text!Templates/List?type=Designations' },
        viewModel: { require: 'App/Components/HRMS/Designations/DesignationListViewModel' }
    });
    ko.components.register('designationupdate', {
        template: { require: 'text!Templates/Form?type=Designations' },
        viewModel: { require: 'App/Components/HRMS/Designations/DesignationUpdateViewModel' }
    });

    ko.components.register('shiftlist', {
        template: { require: 'text!Templates/List?type=Shifts' },
        viewModel: { require: 'App/Components/HRMS/Shifts/ShiftListViewModel' }
    });
    ko.components.register('shiftupdate', {
        template: { require: 'text!Templates/Form?type=Shifts' },
        viewModel: { require: 'App/Components/HRMS/Shifts/ShiftUpdateViewModel' }
    });

    ko.components.register('skilllist', {
        template: { require: 'text!Templates/List?type=Skills' },
        viewModel: { require: 'App/Components/HRMS/Skills/SkillListViewModel' }
    });
    ko.components.register('skillupdate', {
        template: { require: 'text!Templates/Form?type=Skills' },
        viewModel: { require: 'App/Components/HRMS/Skills/SkillUpdateViewModel' }
    });

    ko.components.register('hobbylist', {
        template: { require: 'text!Templates/List?type=Hobbies' },
        viewModel: { require: 'App/Components/HRMS/Hobbies/HobbyListViewModel' }
    });
    ko.components.register('hobbyupdate', {
        template: { require: 'text!Templates/Form?type=Hobbies' },
        viewModel: { require: 'App/Components/HRMS/Hobbies/HobbyUpdateViewModel' }
    });

    ko.components.register('certificationlist', {
        template: { require: 'text!Templates/List?type=Certifications' },
        viewModel: { require: 'App/Components/HRMS/Certifications/CertificationListViewModel' }
    });
    ko.components.register('certificationupdate', {
        template: { require: 'text!Templates/Form?type=Certifications' },
        viewModel: { require: 'App/Components/HRMS/Certifications/CertificationUpdateViewModel' }
    });

    ko.components.register('employeelist', {
        template: { require: 'text!Templates/List?type=Users' },
        viewModel: { require: 'App/Components/HRMS/Employees/EmployeeListViewModel' }
    });
    ko.components.register('employeeupdate', {
        template: { require: 'text!Templates/Form?type=Users' },
        viewModel: { require: 'App/Components/HRMS/Employees/EmployeeUpdateViewModel' }
    });

    //Recruit
    ko.components.register('jobopeningslist', {
        template: { require: 'text!Templates/List?type=JobOpenings' },
        viewModel: { require: 'App/Components/Recruit/JobOpenings/JobOpeningListViewModel' }
    });

    ko.components.register('candidatedesignationslist', {
        template: { require: 'text!Templates/List?type=Designations' },
        viewModel: { require: 'App/Components/Recruit/Designations/DesignationListViewModel' }
    });

    ko.components.register('candidateslist', {
        template: { require: 'text!Templates/List?type=Candidates' },
        viewModel: { require: 'App/Components/Recruit/Candidates/CandidateListViewModel' }
    });
    ko.components.register('candidateupdate', {
        template: { require: 'text!Templates/Form?type=Candidates' },
        viewModel: { require: 'App/Components/Recruit/Candidates/CandidateUpdateViewModel' }
    });
    ko.components.register('candidatedetails', {
        template: { require: 'text!Templates/Details?type=Candidates' },
        viewModel: { require: 'App/Components/Recruit/Candidates/CandidateDetailsViewModel' }
    });

    ko.components.register('referalslist', {
        template: { require: 'text!Templates/List?type=Referals' },
        viewModel: { require: 'App/Components/Recruit/Referals/ReferalListViewModel' }
    });

    ko.components.register('roundslist', {
        template: { require: 'text!Templates/List?type=Rounds' },
        viewModel: { require: 'App/Components/Recruit/Rounds/RoundListViewModel' }
    });

    ko.components.register('interviewslist', {
        template: { require: 'text!Templates/List?type=Interviews' },
        viewModel: { require: 'App/Components/Recruit/Interviews/InterviewListViewModel' }
    });

    ko.components.register('offerslist', {
        template: { require: 'text!Templates/List?type=Offers' },
        viewModel: { require: 'App/Components/Recruit/Offers/OfferListViewModel' }
    });

    // LMS
    ko.components.register('holidaylist', {
        template: { require: 'text!Templates/List?type=Holidays' },
        viewModel: { require: 'App/Components/LMS/Holidays/HolidayListViewModel' }
    });
    ko.components.register('holidayupdate', {
        template: { require: 'text!Templates/Form?type=Holidays' },
        viewModel: { require: 'App/Components/LMS/Holidays/HolidayUpdateViewModel' }
    });

    ko.components.register('leaveperiodlist', {
        template: { require: 'text!Templates/List?type=LeavePeriods' },
        viewModel: { require: 'App/Components/LMS/LeavePeriods/LeavePeriodListViewModel' }
    });
    ko.components.register('leaveperiodupdate', {
        template: { require: 'text!Templates/Form?type=LeavePeriods' },
        viewModel: { require: 'App/Components/LMS/LeavePeriods/LeavePeriodUpdateViewModel' }
    });

    ko.components.register('leavetypelist', {
        template: { require: 'text!Templates/List?type=LeaveTypes' },
        viewModel: { require: 'App/Components/LMS/LeaveTypes/LeaveTypeListViewModel' }
    });
    ko.components.register('leavetypeupdate', {
        template: { require: 'text!Templates/Form?type=LeaveTypes' },
        viewModel: { require: 'App/Components/LMS/LeaveTypes/LeaveTypeUpdateViewModel' }
    });

    ko.components.register('leavelist', {
        template: { require: 'text!Templates/List?type=Leaves' },
        viewModel: { require: 'App/Components/LMS/Leaves/LeaveListViewModel' }
    });
    ko.components.register('leaveupdate', {
        template: { require: 'text!Templates/Form?type=Leaves' },
        viewModel: { require: 'App/Components/LMS/Leaves/LeaveUpdateViewModel' }
    });

    // IMS
    ko.components.register('assetcategorylist', {
        template: { require: 'text!Templates/List?type=AssetCategories' },
        viewModel: { require: 'App/Components/IMS/AssetCategories/AssetCategoryListViewModel' }
    });
    ko.components.register('assetcategoryupdate', {
        template: { require: 'text!Templates/Form?type=AssetCategories' },
        viewModel: { require: 'App/Components/IMS/AssetCategories/AssetCategoryUpdateViewModel' }
    });

    ko.components.register('assetlist', {
        template: { require: 'text!Templates/List?type=Assets' },
        viewModel: { require: 'App/Components/IMS/Assets/AssetListViewModel' }
    });
    ko.components.register('assetupdate', {
        template: { require: 'text!Templates/Form?type=Assets' },
        viewModel: { require: 'App/Components/IMS/Assets/AssetUpdateViewModel' }
    });

    ko.components.register('vendorlist', {
        template: { require: 'text!Templates/List?type=Vendors' },
        viewModel: { require: 'App/Components/IMS/Vendors/VendorListViewModel' }
    });
    ko.components.register('vendorupdate', {
        template: { require: 'text!Templates/Form?type=Vendors' },
        viewModel: { require: 'App/Components/IMS/Vendors/VendorUpdateViewModel' }
    });

    ko.components.register('imsdashboard', {
        template: { require: 'text!Templates/List?type=AssetCategories' },
        viewModel: { require: 'App/Components/IMS/AssetCategories/AssetCategoryListViewModel' }
    });

    // IT
    ko.components.register('softwarecategorylist', {
        template: { require: 'text!Templates/List?type=SoftwareCategories' },
        viewModel: { require: 'App/Components/IT/SoftwareCategories/SoftwareCategoryListViewModel' }
    });
    ko.components.register('softwarecategoryupdate', {
        template: { require: 'text!Templates/Form?type=SoftwareCategories' },
        viewModel: { require: 'App/Components/IT/SoftwareCategories/SoftwareCategoryUpdateViewModel' }
    });

    ko.components.register('softwarelist', {
        template: { require: 'text!Templates/List?type=Softwares' },
        viewModel: { require: 'App/Components/IT/Softwares/SoftwareListViewModel' }
    });
    ko.components.register('softwareupdate', {
        template: { require: 'text!Templates/Form?type=Softwares' },
        viewModel: { require: 'App/Components/IT/Softwares/SoftwareUpdateViewModel' }
    });

    // CRM
    ko.components.register('leadsourcelist', {
        template: { require: 'text!Templates/List?type=LeadSources' },
        viewModel: { require: 'App/Components/CRM/LeadSources/LeadSourceListViewModel' }
    });
    ko.components.register('leadsourceupdate', {
        template: { require: 'text!Templates/Form?type=LeadSources' },
        viewModel: { require: 'App/Components/CRM/LeadSources/LeadSourceUpdateViewModel' }
    });
    ko.components.register('leadstatuslist', {
        template: { require: 'text!Templates/List?type=LeadStatus' },
        viewModel: { require: 'App/Components/CRM/LeadStatus/LeadStatusListViewModel' }
    });
    ko.components.register('leadstatusupdate', {
        template: { require: 'text!Templates/Form?type=LeadStatus' },
        viewModel: { require: 'App/Components/CRM/LeadStatus/LeadStatusUpdateViewModel' }
    });
    ko.components.register('leadcategorylist', {
        template: { require: 'text!Templates/List?type=LeadCategories' },
        viewModel: { require: 'App/Components/CRM/LeadCategories/LeadCategoryListViewModel' }
    });
    ko.components.register('leadcategoryupdate', {
        template: { require: 'text!Templates/Form?type=LeadCategories' },
        viewModel: { require: 'App/Components/CRM/LeadCategories/LeadCategoryUpdateViewModel' }
    });
    ko.components.register('leadlist', {
        template: { require: 'text!Templates/List?type=Leads' },
        viewModel: { require: 'App/Components/CRM/Leads/LeadListViewModel' }
    });
    ko.components.register('leadupdate', {
        template: { require: 'text!Templates/Form?type=Leads' },
        viewModel: { require: 'App/Components/CRM/Leads/LeadUpdateViewModel' }
    });
    ko.components.register('salesstagelist', {
        template: { require: 'text!Templates/List?type=SalesStages' },
        viewModel: { require: 'App/Components/CRM/SalesStages/SalesStageListViewModel' }
    });
    ko.components.register('salesstageupdate', {
        template: { require: 'text!Templates/Form?type=SalesStages' },
        viewModel: { require: 'App/Components/CRM/SalesStages/SalesStageUpdateViewModel' }
    });
    ko.components.register('potentialcategorylist', {
        template: { require: 'text!Templates/List?type=PotentialCategories' },
        viewModel: { require: 'App/Components/CRM/PotentialCategories/PotentialCategoryListViewModel' }
    });
    ko.components.register('potentialcategoryupdate', {
        template: { require: 'text!Templates/Form?type=PotentialCategories' },
        viewModel: { require: 'App/Components/CRM/PotentialCategories/PotentialCategoryUpdateViewModel' }
    });
    ko.components.register('potentiallist', {
        template: { require: 'text!Templates/List?type=Potentials' },
        viewModel: { require: 'App/Components/CRM/Potentials/PotentialListViewModel' }
    });
    ko.components.register('potentialupdate', {
        template: { require: 'text!Templates/Form?type=Potentials' },
        viewModel: { require: 'App/Components/CRM/Potentials/PotentialUpdateViewModel' }
    });
    ko.components.register('crmaccountlist', {
        template: { require: 'text!Templates/List?type=CRMAccounts' },
        viewModel: { require: 'App/Components/CRM/CRMAccounts/CRMAccountListViewModel' }
    });
    ko.components.register('crmaccountupdate', {
        template: { require: 'text!Templates/Form?type=CRMAccounts' },
        viewModel: { require: 'App/Components/CRM/CRMAccounts/CRMAccountUpdateViewModel' }
    });
    ko.components.register('contactlist', {
        template: { require: 'text!Templates/List?type=Contacts' },
        viewModel: { require: 'App/Components/CRM/Contacts/ContactListViewModel' }
    });
    ko.components.register('contactupdate', {
        template: { require: 'text!Templates/Form?type=Contacts' },
        viewModel: { require: 'App/Components/CRM/Contacts/ContactUpdateViewModel' }
    });

    // RMS
    ko.components.register('requirementcategorylist', {
        template: { require: 'text!Templates/List?type=RequirementCategories' },
        viewModel: { require: 'App/Components/RMS/RequirementCategories/RequirementCategoryListViewModel' }
    });
    ko.components.register('requirementcategoryupdate', {
        template: { require: 'text!Templates/Form?type=RequirementCategories' },
        viewModel: { require: 'App/Components/RMS/RequirementCategories/RequirementCategoryUpdateViewModel' }
    });

    ko.components.register('requirementlist', {
        template: { require: 'text!Templates/List?type=Requirements' },
        viewModel: { require: 'App/Components/RMS/Requirements/RequirementListViewModel' }
    });
    ko.components.register('requirementupdate', {
        template: { require: 'text!Templates/Form?type=Requirements' },
        viewModel: { require: 'App/Components/RMS/Requirements/RequirementUpdateViewModel' }
    });

    // PMS
    ko.components.register('projectlist', {
        template: { require: 'text!Templates/List?type=Projects' },
        viewModel: { require: 'App/Components/PMS/Projects/ProjectListViewModel' }
    });
    ko.components.register('projectupdate', {
        template: { require: 'text!Templates/Form?type=Projects' },
        viewModel: { require: 'App/Components/PMS/Projects/ProjectUpdateViewModel' }
    });

    ko.components.register('tasklist', {
        template: { require: 'text!Templates/List?type=Tasks' },
        viewModel: { require: 'App/Components/PMS/Tasks/TaskListViewModel' }
    });
    ko.components.register('taskupdate', {
        template: { require: 'text!Templates/Form?type=Tasks' },
        viewModel: { require: 'App/Components/PMS/Tasks/TaskUpdateViewModel' }
    });

    // KBS
    ko.components.register('articlecategorylist', {
        template: { require: 'text!Templates/List?type=ArticleCategories' },
        viewModel: { require: 'App/Components/KBS/ArticleCategories/ArticleCategoryListViewModel' }
    });
    ko.components.register('articlecategoryupdate', {
        template: { require: 'text!Templates/Form?type=ArticleCategories' },
        viewModel: { require: 'App/Components/KBS/ArticleCategories/ArticleCategoryUpdateViewModel' }
    });

    ko.components.register('articlelist', {
        template: { require: 'text!Templates/List?type=Articles' },
        viewModel: { require: 'App/Components/KBS/Articles/ArticleListViewModel' }
    });
    ko.components.register('articleupdate', {
        template: { require: 'text!Templates/Form?type=Articles' },
        viewModel: { require: 'App/Components/KBS/Articles/ArticleUpdateViewModel' }
    });

    // Ticket Desk
    ko.components.register('ticketcategorylist', {
        template: { require: 'text!Templates/List?type=TicketCategories' },
        viewModel: { require: 'App/Components/TicketDesk/TicketCategories/TicketCategoryListViewModel' }
    });
    ko.components.register('ticketcategoryupdate', {
        template: { require: 'text!Templates/Form?type=TicketCategories' },
        viewModel: { require: 'App/Components/TicketDesk/TicketCategories/TicketCategoryUpdateViewModel' }
    });

    ko.components.register('ticketsubcategorylist', {
        template: { require: 'text!Templates/List?type=TicketSubCategories' },
        viewModel: { require: 'App/Components/TicketDesk/TicketSubCategories/TicketSubCategoryListViewModel' }
    });
    ko.components.register('ticketsubcategoryupdate', {
        template: { require: 'text!Templates/Form?type=TicketSubCategories' },
        viewModel: { require: 'App/Components/TicketDesk/TicketSubCategories/TicketSubCategoryUpdateViewModel' }
    });

    ko.components.register('ticketslist', {
        template: { require: 'text!Templates/List?type=Tickets' },
        viewModel: { require: 'App/Components/TicketDesk/Tickets/TicketListViewModel' }
    });
    ko.components.register('ticketupdate', {
        template: { require: 'text!Templates/Form?type=Tickets' },
        viewModel: { require: 'App/Components/TicketDesk/Tickets/TicketUpdateViewModel' }
    });
    ko.components.register('ticketdetails', {
        template: { require: 'text!Templates/Details?type=Tickets' },
        viewModel: { require: 'App/Components/TicketDesk/Tickets/TicketDetailsViewModel' }
    });

    ko.applyBindings({ route: router.currentRoute });
});



