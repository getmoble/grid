using System;
using System.Collections.Generic;
using System.Net.Mail;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities.Enums;
using Grid.Features.PMS.Services.Interfaces;
using Grid.Providers.Email;

namespace Grid.Features.PMS.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public EmailContext ComposeEmailContextForTaskCreated(int taskId)
        {
            var emailContext = new EmailContext();
            var selectedTask = _taskRepository.Get(taskId, "Project,Assignee.User.Person,CreatedByUser.Person");
            if (selectedTask != null)
            {
                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[Id]", selectedTask.Id.ToString()),
                    new PlaceHolder("[Title]", selectedTask.Title),
                    new PlaceHolder("[Description]", selectedTask.Description),
                    new PlaceHolder("[Project]", selectedTask.Project.Title),
                    new PlaceHolder("[Status]", Enum.GetName(typeof(ProjectTaskStatus), selectedTask.TaskStatus)),
                    new PlaceHolder("[AssignedTo]", selectedTask.Assignee.User.Person.Name),
                    new PlaceHolder("[CreatedBy]", selectedTask.CreatedByUser.Person.Name)
                };

                emailContext.Subject = "Task Created";

                emailContext.ToAddress.Add(new MailAddress(selectedTask.Assignee.OfficialEmail, selectedTask.Assignee.User.Person.Name));
            }

            return emailContext;
        }

        public EmailContext ComposeEmailContextForTaskUpdated(int taskId)
        {
            var emailContext = new EmailContext();
            var selectedTask = _taskRepository.Get(taskId, "Project,Assignee.User.Person,CreatedByUser.Person");
            if (selectedTask != null)
            {
                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[Id]", selectedTask.Id.ToString()),
                    new PlaceHolder("[Title]", selectedTask.Title),
                    new PlaceHolder("[Description]", selectedTask.Description),
                    new PlaceHolder("[Project]", selectedTask.Project.Title),
                    new PlaceHolder("[Status]", Enum.GetName(typeof(ProjectTaskStatus), selectedTask.TaskStatus)),
                    new PlaceHolder("[AssignedTo]", selectedTask.Assignee.User.Person.Name),
                    new PlaceHolder("[CreatedBy]", selectedTask.CreatedByUser.Person.Name)
                };

                emailContext.Subject = "Task Updated";

                emailContext.ToAddress.Add(new MailAddress(selectedTask.Assignee.OfficialEmail, selectedTask.Assignee.User.Person.Name));
            }

            return emailContext;
        }

        public EmailContext ComposeEmailContextForTaskMissed(int taskId)
        {
            var emailContext = new EmailContext();
            var selectedTask = _taskRepository.Get(taskId, "Project,Assignee.User.Person,CreatedByUser.Person");
            if (selectedTask != null)
            {
                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[Id]", selectedTask.Id.ToString()),
                    new PlaceHolder("[Title]", selectedTask.Title),
                    new PlaceHolder("[Description]", selectedTask.Description),
                    new PlaceHolder("[Project]", selectedTask.Project.Title),
                    new PlaceHolder("[Status]", Enum.GetName(typeof(ProjectTaskStatus), selectedTask.TaskStatus)),
                    new PlaceHolder("[AssignedTo]", selectedTask.Assignee.User.Person.Name),
                    new PlaceHolder("[CreatedBy]", selectedTask.CreatedByUser.Person.Name)
                };

                emailContext.Subject = "Task Missed";

                emailContext.ToAddress.Add(new MailAddress(selectedTask.Assignee.OfficialEmail, selectedTask.Assignee.User.Person.Name));
                emailContext.CcAddress.Add(new MailAddress(selectedTask.CreatedByUser.OfficialEmail, selectedTask.CreatedByUser.Person.Name));
            }

            return emailContext;
        }
    }
}
