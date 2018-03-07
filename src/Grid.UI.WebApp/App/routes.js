define(["require", "exports"], function (require, exports) {
    return {
        routes: [
            { url: '', params: { page: 'locationlist' } },

            { url: 'locations', params: { page: 'locationlist' } },
            { url: 'location/create', params: { page: 'locationupdate' } },
            { url: 'location/edit/{id}', params: { page: 'locationupdate' } },

            { url: 'permissionlist', params: { page: 'permissionlist' } },

            { url: 'rolelist', params: { page: 'rolelist' } },
            { url: 'role/create', params: { page: 'roleupdate' } },
            { url: 'role/edit/{id}', params: { page: 'roleupdate' } },

            { url: 'technologylist', params: { page: 'technologylist' } },
            { url: 'technology/create', params: { page: 'technologyupdate' } },
            { url: 'technology/edit/{id}', params: { page: 'technologyupdate' } },

            { url: 'emailtemplatelist', params: { page: 'emailtemplatelist' } },
            { url: 'emailtemplate/create', params: { page: 'emailtemplateupdate' } },
            { url: 'emailtemplate/edit/{id}', params: { page: 'emailtemplateupdate' } },

            { url: 'awardlist', params: { page: 'awardlist' } },
            { url: 'award/create', params: { page: 'awardupdate' } },
            { url: 'award/edit/{id}', params: { page: 'awardupdate' } },




            { url: 'departmentlist', params: { page: 'departmentlist' } },
            { url: 'department/create', params: { page: 'departmentupdate' } },
            { url: 'department/edit/{id}', params: { page: 'departmentupdate' } },

            { url: 'designationlist', params: { page: 'designationlist' } },
            { url: 'designation/create', params: { page: 'designationupdate' } },
            { url: 'designation/edit/{id}', params: { page: 'designationupdate' } },

            { url: 'shiftlist', params: { page: 'shiftlist' } },
            { url: 'shift/create', params: { page: 'shiftupdate' } },
            { url: 'shift/edit/{id}', params: { page: 'shiftupdate' } },

            { url: 'skilllist', params: { page: 'skilllist' } },
            { url: 'skill/create', params: { page: 'skillupdate' } },
            { url: 'skill/edit/{id}', params: { page: 'skillupdate' } },

            { url: 'hobbylist', params: { page: 'hobbylist' } },
            { url: 'hobby/create', params: { page: 'hobbyupdate' } },
            { url: 'hobby/edit/{id}', params: { page: 'hobbyupdate' } },

            { url: 'certificationlist', params: { page: 'certificationlist' } },
            { url: 'certification/create', params: { page: 'certificationupdate' } },
            { url: 'certification/edit/{id}', params: { page: 'certificationupdate' } },

            { url: 'employeelist', params: { page: 'employeelist' } },
            { url: 'employee/edit/{id}', params: { page: 'employeeupdate' } },




            { url: 'jobopeningslist', params: { page: 'jobopeningslist' } },
            { url: 'candidatedesignationslist', params: { page: 'candidatedesignationslist' } },

            { url: 'candidateslist', params: { page: 'candidateslist' } },
            { url: 'candidate/create', params: { page: 'candidateupdate' } },
            { url: 'candidate/edit/{id}', params: { page: 'candidateupdate' } },
            { url: 'candidate/{id}', params: { page: 'candidatedetails' } },

            { url: 'referalslist', params: { page: 'referalslist' } },
            { url: 'roundslist', params: { page: 'roundslist' } },
            { url: 'interviewslist', params: { page: 'interviewslist' } },
            { url: 'offerslist', params: { page: 'offerslist' } },




            { url: 'holidaylist', params: { page: 'holidaylist' } },
            { url: 'holiday/create', params: { page: 'holidayupdate' } },
            { url: 'holiday/edit/{id}', params: { page: 'holidayupdate' } },

            { url: 'leaveperiodlist', params: { page: 'leaveperiodlist' } },
            { url: 'leaveperiod/create', params: { page: 'leaveperiodupdate' } },
            { url: 'leaveperiod/edit/{id}', params: { page: 'leaveperiodupdate' } },

            { url: 'leavetypelist', params: { page: 'leavetypelist' } },
            { url: 'leavetype/create', params: { page: 'leavetypeupdate' } },
            { url: 'leavetype/edit/{id}', params: { page: 'leavetypeupdate' } },

            { url: 'leavelist', params: { page: 'leavelist' } },
            { url: 'leave/create', params: { page: 'leaveupdate' } },
            { url: 'leave/edit/{id}', params: { page: 'leaveupdate' } },


            { url: 'softwarecategorylist', params: { page: 'softwarecategorylist' } },
            { url: 'softwarecategory/create', params: { page: 'softwarecategoryupdate' } },
            { url: 'softwarecategory/edit/{id}', params: { page: 'softwarecategoryupdate' } },

            { url: 'softwarelist', params: { page: 'softwarelist' } },
            { url: 'software/create', params: { page: 'softwareupdate' } },
            { url: 'software/edit/{id}', params: { page: 'softwareupdate' } },




            { url: 'vendorlist', params: { page: 'vendorlist' } },
            { url: 'vendor/create', params: { page: 'vendorupdate' } },
            { url: 'vendor/edit/{id}', params: { page: 'vendorupdate' } },

            { url: 'assetcategorylist', params: { page: 'assetcategorylist' } },
            { url: 'assetcategory/create', params: { page: 'assetcategoryupdate' } },
            { url: 'assetcategory/edit/{id}', params: { page: 'assetcategoryupdate' } },

            { url: 'assetlist', params: { page: 'assetlist' } },
            { url: 'asset/create', params: { page: 'assetupdate' } },
            { url: 'asset/edit/{id}', params: { page: 'assetupdate' } },

            { url: 'imsdashboardlist', params: { page: 'imsdashboardlist' } },


            { url: 'leadsourcelist', params: { page: 'leadsourcelist' } },
            { url: 'leadsource/create', params: { page: 'leadsourceupdate' } },
            { url: 'leadsource/edit/{id}', params: { page: 'leadsourceupdate' } },

            { url: 'leadstatuslist', params: { page: 'leadstatuslist' } },
            { url: 'leadstatus/create', params: { page: 'leadstatusupdate' } },
            { url: 'leadstatus/edit/{id}', params: { page: 'leadstatusupdate' } },

            { url: 'leadcategorylist', params: { page: 'leadcategorylist' } },
            { url: 'leadcategory/create', params: { page: 'leadcategoryupdate' } },
            { url: 'leadcategory/edit/{id}', params: { page: 'leadcategoryupdate' } },

            { url: 'leadlist', params: { page: 'leadlist' } },
            { url: 'lead/create', params: { page: 'leadupdate' } },
            { url: 'lead/edit/{id}', params: { page: 'leadupdate' } },

            { url: 'salesstagelist', params: { page: 'salesstagelist' } },
            { url: 'salesstage/create', params: { page: 'salesstageupdate' } },
            { url: 'salesstage/edit/{id}', params: { page: 'salesstageupdate' } },

            { url: 'potentialcategorylist', params: { page: 'potentialcategorylist' } },
            { url: 'potentialcategory/create', params: { page: 'potentialcategoryupdate' } },
            { url: 'potentialcategory/edit/{id}', params: { page: 'potentialcategoryupdate' } },

            { url: 'potentiallist', params: { page: 'potentiallist' } },
            { url: 'potential/create', params: { page: 'potentialupdate' } },
            { url: 'potential/edit/{id}', params: { page: 'potentialupdate' } },

            { url: 'crmaccountlist', params: { page: 'crmaccountlist' } },
            { url: 'crmaccount/create', params: { page: 'crmaccountupdate' } },
            { url: 'crmaccount/edit/{id}', params: { page: 'crmaccountupdate' } },

            { url: 'contactlist', params: { page: 'contactlist' } },
            { url: 'contact/create', params: { page: 'contactupdate' } },
            { url: 'contact/edit/{id}', params: { page: 'contactupdate' } },

            { url: 'directory', params: { page: 'employeedirectory' } },
            { url: 'employeeprofile', params: { page: 'employeeprofile' } },


            { url: 'requirementcategorylist', params: { page: 'requirementcategorylist' } },
            { url: 'requirementcategory/create', params: { page: 'requirementcategoryupdate' } },
            { url: 'requirementcategory/edit/{id}', params: { page: 'requirementcategoryupdate' } },

            { url: 'requirementlist', params: { page: 'requirementlist' } },
            { url: 'requirement/create', params: { page: 'requirementupdate' } },
            { url: 'requirement/edit/{id}', params: { page: 'requirementupdate' } },

            { url: 'projectlist', params: { page: 'projectlist' } },
            { url: 'project/create', params: { page: 'projectupdate' } },
            { url: 'project/edit/{id}', params: { page: 'projectupdate' } },

            { url: 'tasklist', params: { page: 'tasklist' } },
            { url: 'task/create', params: { page: 'taskupdate' } },
            { url: 'task/edit/{id}', params: { page: 'taskupdate' } },

            { url: 'articlecategorylist', params: { page: 'articlecategorylist' } },
            { url: 'articlecategory/create', params: { page: 'articlecategoryupdate' } },
            { url: 'articlecategory/edit/{id}', params: { page: 'articlecategoryupdate' } },

            { url: 'articlelist', params: { page: 'articlelist' } },
            { url: 'article/create', params: { page: 'articleupdate' } },
            { url: 'article/edit/{id}', params: { page: 'articleupdate' } },

            { url: 'ticketcategorylist', params: { page: 'ticketcategorylist' } },
            { url: 'ticketcategory/create', params: { page: 'ticketcategoryupdate' } },
            { url: 'ticketcategory/edit/{id}', params: { page: 'ticketcategoryupdate' } },

            { url: 'ticketsubcategorylist', params: { page: 'ticketsubcategorylist' } },
            { url: 'ticketsubcategory/create', params: { page: 'ticketsubcategoryupdate' } },
            { url: 'ticketsubcategory/edit/{id}', params: { page: 'ticketsubcategoryupdate' } },

            { url: 'ticketslist', params: { page: 'ticketslist' } },
            { url: 'ticket/create', params: { page: 'ticketupdate' } },
            { url: 'ticket/edit/{id}', params: { page: 'ticketupdate' } },
            { url: 'ticket/{id}', params: { page: 'ticketdetails' } }
        ]
    }
});