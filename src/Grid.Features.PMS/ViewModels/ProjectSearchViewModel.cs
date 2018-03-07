using System.Collections.Generic;
using Grid.Features.Common;
using Grid.Features.PMS.Entities.Enums;

namespace Grid.Features.PMS.ViewModels
{
    public class ProjectSearchViewModel: ViewModelBase
    {
        public bool? IsPublic { get; set; }
        public int? ClientId { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public ProjectStatus? Status { get; set; }
        public Billing? Billing { get; set; }
        public bool ShowClosedProjects { get; set; }
        public List<Entities.Project> Projects { get; set; }
        public List<ProjectTreeNode> TreeNodes { get; set; }    

        public ProjectSearchViewModel()
        {
            Projects = new List<Entities.Project>();
        }
    }
}