using HealthTracker.Application.Features.WeeklyReports.DTOs;
using MediatR;

namespace HealthTracker.Application.Features.WeeklyReports.Queries;

public record GetWeeklyReportsQuery(Guid UserId) : IRequest<List<WeeklyReportDto>>;