﻿using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public.Administration
{
    public interface IReportedIssueNotificationService
    {
        Result<ReportedIssueNotificationDto> Get(int id); 
        Result<ReportedIssueNotificationDto> Update(ReportedIssueNotificationDto reportedIssueNotification);
        Result Delete(int id);
        Result<ReportedIssueNotificationDto> Create(string description, long userId, long reportedIssueId);
        Result<PagedResult<ReportedIssueNotificationDto>> GetAllByUser(long id, int page, int pageSize);
        Result<PagedResult<ReportedIssueNotificationDto>> GetUnreadByUser(long id, int page, int pageSize);
    }
}
