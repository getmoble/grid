using System;
using System.Collections.Generic;
using Grid.Features.PMS.Entities;
using Grid.Providers.Email;

namespace Grid.Features.PMS.Services.Interfaces
{
    public interface ITimeSheetService
    {
        List<TimeSheet> GetPendingTimeSheets();
        bool AutoApprove(int duedays);

        EmailContext ComposeEmailContextForTimesheetReminder(int employeeId, DateTime date);
        EmailContext ComposeEmailContextForTimesheetMissed(int employeeId, DateTime date);
        EmailContext ComposeEmailContextForTimesheetApprovalReminder(int approverId);
        EmailContext ComposeEmailContextForTimesheetSummary(int employeeId);
        EmailContext ComposeEmailContextForTimesheetSubmitted(int timeSheetId);
        EmailContext ComposeEmailContextForTimeSheetUpdated(int timeSheetId);
        EmailContext ComposeEmailContextForTimeSheetApproved(int timeSheetId);
        EmailContext ComposeEmailContextForTimeSheetNeedsCorrection(int timeSheetId);
    }
}
