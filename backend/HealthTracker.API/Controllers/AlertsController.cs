using System.Security.Claims;
using HealthTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HealthTracker.API.Controllers;

/// <summary>
/// AI-powered predictive health alerts based on the user's recent 7-day patterns.
/// Analyzes trends and warns about potential upcoming health issues.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/alerts")]
[Authorize]
[Asp.Versioning.ApiVersion("1.0")]
public class AlertsController : ControllerBase
{
    private readonly IMoodLogRepository _moodRepo;
    private readonly ISleepLogRepository _sleepRepo;
    private readonly ISymptomLogRepository _symptomRepo;
    private readonly IAiAnalysisService _aiService;
    private readonly ILogger<AlertsController> _logger;

    public AlertsController(
        IMoodLogRepository moodRepo,
        ISleepLogRepository sleepRepo,
        ISymptomLogRepository symptomRepo,
        IAiAnalysisService aiService,
        ILogger<AlertsController> logger)
    {
        _moodRepo = moodRepo;
        _sleepRepo = sleepRepo;
        _symptomRepo = symptomRepo;
        _aiService = aiService;
        _logger = logger;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>
    /// Get predictive health alerts based on the past 7 days of data.
    /// Uses rule-based local analysis for fast alerts + AI for deeper predictions.
    /// GET /api/v1/alerts
    /// </summary>
    [HttpGet]
    [EnableRateLimiting("insights")]
    public async Task<IActionResult> GetAlerts()
    {
        var userId = GetUserId();
        var to = DateOnly.FromDateTime(DateTime.UtcNow);
        var from = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7));

        var moods = await _moodRepo.GetByUserIdAsync(userId, from, to);
        var sleep = await _sleepRepo.GetByUserIdAsync(userId, from, to);
        var symptoms = await _symptomRepo.GetByUserIdAsync(userId, from, to);

        var alerts = new List<HealthAlert>();

        // ── Rule-based fast alerts ─────────────────────────────────────────────

        // Mood declining trend (3+ days in a row dropping)
        var recentMoods = moods.OrderByDescending(m => m.LogDate).Take(3).Select(m => m.MoodScore).ToList();
        if (recentMoods.Count >= 3 && recentMoods[0] < recentMoods[1] && recentMoods[1] < recentMoods[2])
        {
            alerts.Add(new HealthAlert(
                "warning",
                "📉 Mood Declining",
                $"Your mood has dropped for 3 consecutive days (latest: {recentMoods[0]}/10). Consider a rest day or a short walk.",
                "mood"
            ));
        }

        // Sleep deprivation pattern
        var last3Sleep = sleep.OrderByDescending(s => s.LogDate).Take(3).ToList();
        if (last3Sleep.Count >= 2 && last3Sleep.All(s => s.HoursSlept < 6))
        {
            alerts.Add(new HealthAlert(
                "danger",
                "⚠️ Sleep Deprivation",
                $"You've slept less than 6 hours for {last3Sleep.Count} consecutive nights. Chronic sleep deprivation affects mood, immunity, and focus.",
                "sleep"
            ));
        }

        // Headache + poor sleep correlation
        var recentHeadaches = symptoms
            .Where(s => s.SymptomName.Contains("Headache", StringComparison.OrdinalIgnoreCase)
                     || s.SymptomName.Contains("Migraine", StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(s => s.LogDate)
            .Take(2)
            .ToList();

        if (recentHeadaches.Any())
        {
            var sleepBeforeHeadache = sleep
                .Where(s => recentHeadaches.Any(h => h.LogDate == s.LogDate || h.LogDate == s.LogDate.AddDays(1)))
                .ToList();

            if (sleepBeforeHeadache.Any(s => s.HoursSlept < 6.5))
            {
                var lastSleep = sleep.OrderByDescending(s => s.LogDate).FirstOrDefault();
                if (lastSleep != null && lastSleep.HoursSlept < 6.5)
                {
                    alerts.Add(new HealthAlert(
                        "warning",
                        "🧠 Headache Risk",
                        $"Your headaches tend to occur after poor sleep. You slept {lastSleep.HoursSlept:F1}h last night — stay hydrated and take breaks.",
                        "symptom"
                    ));
                }
            }
        }

        // Good streak positive alert
        var avgMoodThisWeek = moods.Any() ? moods.Average(m => m.MoodScore) : 0;
        var avgSleepThisWeek = sleep.Any() ? sleep.Average(s => s.HoursSlept) : 0;
        if (avgMoodThisWeek >= 7.5 && avgSleepThisWeek >= 7)
        {
            alerts.Add(new HealthAlert(
                "success",
                "✅ Great Week!",
                $"Your average mood is {avgMoodThisWeek:F1}/10 and average sleep is {avgSleepThisWeek:F1}h this week. Keep it up!",
                "positive"
            ));
        }

        // Recurring symptom alert
        var symptomCounts = symptoms
            .GroupBy(s => s.SymptomName, StringComparer.OrdinalIgnoreCase)
            .Where(g => g.Count() >= 3)
            .ToList();

        foreach (var group in symptomCounts)
        {
            alerts.Add(new HealthAlert(
                "info",
                $"🔁 Recurring {group.Key}",
                $"You've logged {group.Key} {group.Count()} times this week. Consider mentioning this to a healthcare professional.",
                "symptom"
            ));
        }

        // No logs this week warning
        if (!moods.Any() && !sleep.Any())
        {
            alerts.Add(new HealthAlert(
                "info",
                "📋 No Recent Logs",
                "You haven't logged any health data in the past 7 days. Regular tracking helps AI provide better insights.",
                "general"
            ));
        }

        _logger.LogInformation("Generated {Count} predictive alerts for user {UserId}", alerts.Count, userId);

        return Ok(new
        {
            generatedAt = DateTime.UtcNow,
            period = new { from = from.ToString("yyyy-MM-dd"), to = to.ToString("yyyy-MM-dd") },
            totalAlerts = alerts.Count,
            alerts
        });
    }
}

public record HealthAlert(string Severity, string Title, string Message, string Category);
