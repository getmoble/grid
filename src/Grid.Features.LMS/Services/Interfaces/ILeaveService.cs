using System;
using System.Collections.Generic;
using Grid.Features.LMS.Entities;
using Grid.Providers.Email;

namespace Grid.Features.LMS.Services.Interfaces
{
    public interface ILeaveService
    {
        List<Leave> GetPendingLeaves();
        float GetLeaveCount(DateTime startDate, DateTime endDate);
        bool Approve(int leaveId, int approverId, string approverComments, bool autoApproved);
        bool Reject(int leaveId, int approverId, string approverComments);

        EmailContext ComposeEmailContextForLeaveApplication(int leaveId);
        EmailContext ComposeEmailContextForLeaveApproval(int leaveId, bool autoapproved);
        EmailContext ComposeEmailContextForLeaveRejected(int leaveId);
        EmailContext TestEmail(Leave leave);
    }
}
