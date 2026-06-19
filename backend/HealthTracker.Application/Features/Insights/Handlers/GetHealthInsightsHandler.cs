using HealthTracker.Application.Features.Insights.DTOs;
using HealthTracker.Application.Features.Insights.Queries;
using HealthTracker.Domain.Interfaces;
using MediatR;

namespace HealthTracker.Application.Features.Insights.Handlers;

public class GetHealthInsightsHandler : IRequestHandler<GetHealthInsightsQuery, HealthInsightsDto>
{
    private readonly ISymptomLogRepository _symptomRepo;
    private readonly IMoodLogRepository _moodRepo;
    private readonly ISleepLogRepository _sleepRepo;
    private readonly IMealLogRepository _mealRepo;
    private readonly IAiAnalysisService _aiService;

    public GetHealthInsightsHandler(
        ISymptomLogRepository symptomRepo,
        IMoodLogRepository moodRepo,
        ISleepLogRepository sleepRepo,
        IMealLogRepository mealRepo,
        IAiAnalysisService aiService)
    {
        _symptomRepo = symptomRepo;
        _moodRepo = moodRepo;
        _sleepRepo = sleepRepo;
        _mealRepo = mealRepo;
        _aiService = aiService;
    }

    public async Task<HealthInsightsDto> Handle(GetHealthInsightsQuery request, CancellationToken cancellationToken)
    {
        // Run sequentially — EF Core doesn't support parallel queries on same DbContext
        var symptoms = await _symptomRepo.GetByUserIdAsync(request.UserId, request.From, request.To);
        var moods = await _moodRepo.GetByUserIdAsync(request.UserId, request.From, request.To);
        var sleep = await _sleepRepo.GetByUserIdAsync(request.UserId, request.From, request.To);
        var meals = await _mealRepo.GetByUserIdAsync(request.UserId, request.From, request.To);

        var context = new HealthDataContext
        {
            UserId = request.UserId,
            From = request.From,
            To = request.To,
            Symptoms = symptoms.Select(s =>
                $"{s.LogDate}: {s.SymptomName} severity {s.Severity}/10" +
                (s.Notes != null ? $" ({s.Notes})" : "")).ToList(),
            Moods = moods.Select(m =>
                $"{m.LogDate}: mood {m.MoodScore}/10" +
                (m.Notes != null ? $" ({m.Notes})" : "")).ToList(),
            Sleep = sleep.Select(s =>
                $"{s.LogDate}: {s.HoursSlept}hrs sleep, quality {s.QualityScore}/10, bed {s.BedTime}, wake {s.WakeTime}").ToList(),
            Meals = meals.Select(m =>
                $"{m.LogDate}: {m.MealType} - {m.Description}").ToList()
        };

        var totalLogs = symptoms.Count + moods.Count + sleep.Count + meals.Count;

        if (totalLogs == 0)
            return new HealthInsightsDto(
                "No health data found for the selected date range. Start logging your symptoms, mood, sleep and meals to get AI insights.",
                request.From,
                request.To,
                0,
                DateTime.UtcNow
            );

        var insights = await _aiService.AnalyzeHealthLogsAsync(context);

        return new HealthInsightsDto(
            insights,
            request.From,
            request.To,
            totalLogs,
            DateTime.UtcNow
        );
    }
}