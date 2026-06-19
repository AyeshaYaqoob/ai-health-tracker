using HealthTracker.Application.Features.WeeklyReports.DTOs;
using HealthTracker.Application.Features.WeeklyReports.Queries;
using HealthTracker.Domain.Interfaces;
using MediatR;

namespace HealthTracker.Application.Features.WeeklyReports.Handlers;

public class GetWeeklyReportsHandler : IRequestHandler<GetWeeklyReportsQuery, List<WeeklyReportDto>>
{
    private readonly IWeeklyReportRepository _repository;
    public GetWeeklyReportsHandler(IWeeklyReportRepository repository) => _repository = repository;

    public async Task<List<WeeklyReportDto>> Handle(GetWeeklyReportsQuery request, CancellationToken cancellationToken)
    {
        var reports = await _repository.GetByUserIdAsync(request.UserId);
        return reports.Select(r => new WeeklyReportDto(
            r.Id,
            r.WeekStartDate,
            r.WeekEndDate,
            r.AiInsights,
            r.TopSymptoms,
            r.AvgMoodScore,
            r.AvgSleepHours,
            r.CreatedAt
        )).ToList();
    }
}