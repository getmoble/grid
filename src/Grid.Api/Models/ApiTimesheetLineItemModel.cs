using Grid.Features.PMS.Entities;

namespace Grid.Api.Models
{
    public class ApiTimesheetLineItemModel
    {
        public int Id { get; set; }
        public int TimeSheetId { get; set; }
        public int ProjectId   { get; set; }
        public string ProjectName { get; set; }
        public int? TaskId { get; set; }
        public string TaskTitle { get; set; }
        public string TaskSummary { get; set; }
        public double Effort { get; set; }
        public string Comments { get; set; }
        public int WorkType { get; set; }
        public string CreatedByUser { get; set; }
        public int CreatedByUserId { get; set; }

        public ApiTimesheetLineItemModel(TimeSheetLineItem lineItem)
        {
            Id = lineItem.Id;
            TimeSheetId = lineItem.TimeSheetId;

            ProjectId = lineItem.ProjectId;
            if (lineItem.Project != null)
            {
                ProjectName = lineItem.Project.Title;
            }

            TaskId = lineItem.TaskId;
            if (lineItem.Task != null)
            {
                TaskTitle = lineItem.Task.Title;
            }

            TaskSummary = lineItem.TaskSummary;
            Effort = lineItem.Effort;
            Comments = lineItem.Comments;
            WorkType = lineItem.WorkType;
            if (lineItem.TimeSheet != null)
            {
                CreatedByUserId = lineItem.TimeSheet.CreatedByUserId;
                if (lineItem.TimeSheet.CreatedByUser?.Person != null)
                {
                    CreatedByUser = lineItem.TimeSheet.CreatedByUser.Person.Name;
                }
            }
        }
    }
}