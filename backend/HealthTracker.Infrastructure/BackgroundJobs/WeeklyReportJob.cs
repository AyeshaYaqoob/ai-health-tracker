using HealthTracker.Domain.Entities;
using HealthTracker.Domain.Interfaces;

namespace HealthTracker.Infrastructure.BackgroundJobs;

public class WeeklyReportJob
{
    private readonly IUserRepository _userRepository;
    private readonly ISymptomLogRepository _symptomRepo;
    private readonly IMoodLogRepository _moodRepo;
    private readonly ISleepLogRepository _sleepRepo;
    private readonly IMealLogRepository _mealRepo;
    private readonly IWeeklyReportRepository _reportRepo;
    private readonly IAiAnalysisService _aiService;

    public WeeklyReportJob(
        IUserRepository userRepository,
        ISymptomLogRepository symptomRepo,
        IMoodLogRepository moodRepo,
        ISleepLogRepository sleepRepo,
        IMealLogRepository mealRepo,
        IWeeklyReportRepository reportRepo,
        IAiAnalysisService aiService)
    {
        _userRepository = userRepository;
        _symptomRepo = symptomRepo;
        _moodRepo = moodRepo;
        _sleepRepo = sleepRepo;
        _mealRepo = mealRepo;
        _reportRepo = reportRepo;
        _aiService = aiService;
    }

    public async Task GenerateWeeklyReportsAsync()
    {
        // Get last week's date range
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var weekStart = today.AddDays(-(int)today.DayOfWeek - 6); // last Monday
        var weekEnd = weekStart.AddDays(6); // last Sunday

        var users = await _userRepository.GetAllUsersAsync();

        foreach (var user in users)
        {
            // Skip if report already exists for this week
            if (await _reportRepo.ReportExistsAsync(user.Id, weekStart))
                continue;

            // Fetch all logs for the week
            var symptoms = await _symptomRepo.GetByUserIdAsync(user.Id, weekStart, weekEnd);
            var moods = await _moodRepo.GetByUserIdAsync(user.Id, weekStart, weekEnd);
            var sleep = await _sleepRepo.GetByUserIdAsync(user.Id, weekStart, weekEnd);
            var meals = await _mealRepo.GetByUserIdAsync(user.Id, weekStart, weekEnd);

            var totalLogs = symptoms.Count + moods.Count + sleep.Count + meals.Count;

            // Skip users with no data this week
            if (totalLogs == 0) continue;

            // Build AI context
            var context = new HealthDataContext
            {
                UserId = user.Id,
                From = weekStart,
                To = weekEnd,
                Symptoms = symptoms.Select(s =>
                    $"{s.LogDate}: {s.SymptomName} severity {s.Severity}/10" +
                    (s.Notes != null ? $" ({s.Notes})" : "")).ToList(),
                Moods = moods.Select(m =>
                    $"{m.LogDate}: mood {m.MoodScore}/10" +
                    (m.Notes != null ? $" ({m.Notes})" : "")).ToList(),
                Sleep = sleep.Select(s =>
                    $"{s.LogDate}: {s.HoursSlept}hrs, quality {s.QualityScore}/10").ToList(),
                Meals = meals.Select(m =>
                    $"{m.LogDate}: {m.MealType} - {m.Description}").ToList()
            };

            // Get AI insights
            var aiInsights = await _aiService.AnalyzeHealthLogsAsync(context);

            // Calculate averages
            var avgMood = moods.Any() ? (float)moods.Average(m => m.MoodScore) : 0;
            var avgSleep = sleep.Any() ? (float)sleep.Average(s => s.HoursSlept) : 0;
            var topSymptoms = symptoms
                .GroupBy(s => s.SymptomName)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => g.Key)
                .ToList();

            var report = new WeeklyReport
            {
                UserId = user.Id,
                WeekStartDate = weekStart,
                WeekEndDate = weekEnd,
                AiInsights = aiInsights,
                TopSymptoms = string.Join(", ", topSymptoms),
                AvgMoodScore = avgMood,
                AvgSleepHours = avgSleep,
                EmailSent = false
            };

            await _reportRepo.AddAsync(report);
            await _reportRepo.SaveChangesAsync();

            Console.WriteLine($"Weekly report generated for user {user.Email}");
        }
    }
}