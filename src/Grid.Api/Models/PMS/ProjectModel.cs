using Grid.Features.PMS.Entities;
using Grid.Features.PMS.Entities.Enums;
using System;
using System.Collections.Generic;

namespace Grid.Api.Models.PMS
{

    public class ProjectModel : ApiModelBase
    {     
        public int ClientId { get; set; }      
        public string Client { get; set; }
        public string MemberEmployee { get; set; }
        public string Title { get; set; }      
        public string Description { get; set; }       
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StatusType { get; set; }
        public ProjectStatus? Status{ get; set; }
        public ProjectType? ProjectType { get; set; }
        public string ProjecttypeType { get; set; }
        public string MemberStatusType { get; set; }
        //public string Role { get; set; }
        public Billing Billing { get; set; }       
        public decimal ExpectedBillingAmount { get; set; }
        public double Rate { get; set; }
        public int? ParentId { get; set; }       
        public string ParentProject { get; set; }
        //public string RoleAndStatus { get; set; }
        public bool IsPublic { get; set; }
        public bool InheritMembers { get; set; }
        public bool IsClosed { get; set; }
        // This stores the serialized settings for the Project.
        public string Setting { get; set; }
        public List<int> TechnologyIds { get; set; }

        public int? ProjectMemberRoleId { get; set; }
        public string ProjectMemberRole { get; set; }

        public ProjectModel()
        {

        }
        public ProjectModel(Project project)
        {
            Id = project.Id;
           
            if (project.Client != null)
            {
                if (project.Client.Person != null)
                {
                    Client = project.Client.Person.Name;
                }
            }
            if (project.ParentProject != null)
            {
                ParentProject = project.ParentProject.Title;
            }
            Title = project.Title;
            ClientId = project.ClientId;
            ParentId = project.ParentId;
            Description = project.Description;
            StartDate = project.StartDate;
            EndDate = project.EndDate;
            IsPublic = project.IsPublic;
            InheritMembers = project.InheritMembers;
            StatusType = GetEnumDescription(project.Status);
            Status = project.Status.Value;
            ProjectType = project.ProjectType;
            CreatedOn = project.CreatedOn;
            ProjecttypeType = GetEnumDescription(project.ProjectType);
        }
        public ProjectModel(ProjectMember projectMember)
        {
            Id = projectMember.Id;

            if (projectMember.MemberEmployee?.User.Person != null)
            {
                MemberEmployee = projectMember.MemberEmployee.User.Person.Name;
            }

            ProjectMemberRoleId = projectMember.ProjectMemberRoleId;

            if (projectMember.ProjectMemberRole != null)
            {
                ProjectMemberRole = projectMember.ProjectMemberRole.Title;
            }
            //Role = GetEnumDescription(projectMember.Role);
            CreatedOn = projectMember.CreatedOn;
            MemberStatusType = GetEnumDescription(projectMember.MemberStatus);
            //RoleAndStatus = GetEnumDescription(projectMember.Role) + " "+"-" +" " +GetEnumDescription(projectMember.MemberStatus);
        }
    }
}

