using System.Globalization;
using System.Security.Claims;
using CsvHelper;
using CsvHelper.Configuration;
using HealthTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace HealthTracker.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/export")]
[Authorize]
[Asp.Versioning.ApiVersion("1.0")]
public class ExportController : ControllerBase
{
    private readonly IMoodLogRepository _moodRepo;
    private readonly ISleepLogRepository _sleepRepo;
    private readonly ISymptomLogRepository _symptomRepo;
    private readonly IMealLogRepository _mealRepo;

    public ExportController(
        IMoodLogRepository moodRepo,
        ISleepLogRepository sleepRepo,
        ISymptomLogRepository symptomRepo,
        IMealLogRepository mealRepo)
    {
        _moodRepo = moodRepo;
        _sleepRepo = sleepRepo;
        _symptomRepo = symptomRepo;
        _mealRepo = mealRepo;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>
    /// Export all health logs as a multi-section CSV file.
    /// GET /api/v1/export/csv?from=2026-05-01&amp;to=2026-06-20
    /// </summary>
    [HttpGet("csv")]
    public async Task<IActionResult> ExportCsv(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to)
    {
        var userId = GetUserId();
        var dateFrom = from ?? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30));
        var dateTo = to ?? DateOnly.FromDateTime(DateTime.UtcNow);

        var moods = await _moodRepo.GetByUserIdAsync(userId, dateFrom, dateTo);
        var sleep = await _sleepRepo.GetByUserIdAsync(userId, dateFrom, dateTo);
        var symptoms = await _symptomRepo.GetByUserIdAsync(userId, dateFrom, dateTo);
        var meals = await _mealRepo.GetByUserIdAsync(userId, dateFrom, dateTo);

        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream, leaveOpen: true);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture);
        using var csv = new CsvWriter(writer, config);

        // Mood Logs
        writer.WriteLine("# MOOD LOGS");
        csv.WriteHeader<MoodCsvRow>();
        csv.NextRecord();
        foreach (var m in moods)
        {
            csv.WriteRecord(new MoodCsvRow(m.LogDate.ToString("yyyy-MM-dd"), m.MoodScore, m.Notes ?? ""));
            csv.NextRecord();
        }

        writer.WriteLine();
        writer.WriteLine("# SLEEP LOGS");
        csv.WriteHeader<SleepCsvRow>();
        csv.NextRecord();
        foreach (var s in sleep)
        {
            csv.WriteRecord(new SleepCsvRow(s.LogDate.ToString("yyyy-MM-dd"), s.HoursSlept, s.QualityScore, s.BedTime.ToString(), s.WakeTime.ToString()));
            csv.NextRecord();
        }

        writer.WriteLine();
        writer.WriteLine("# SYMPTOM LOGS");
        csv.WriteHeader<SymptomCsvRow>();
        csv.NextRecord();
        foreach (var sym in symptoms)
        {
            csv.WriteRecord(new SymptomCsvRow(sym.LogDate.ToString("yyyy-MM-dd"), sym.SymptomName, sym.Severity, sym.Notes ?? ""));
            csv.NextRecord();
        }

        writer.WriteLine();
        writer.WriteLine("# MEAL LOGS");
        csv.WriteHeader<MealCsvRow>();
        csv.NextRecord();
        foreach (var meal in meals)
        {
            csv.WriteRecord(new MealCsvRow(meal.LogDate.ToString("yyyy-MM-dd"), meal.MealType, meal.Description));
            csv.NextRecord();
        }

        await writer.FlushAsync();
        stream.Position = 0;

        var fileName = $"health-export-{dateFrom:yyyy-MM-dd}-to-{dateTo:yyyy-MM-dd}.csv";
        return File(stream.ToArray(), "text/csv", fileName);
    }

    /// <summary>
    /// Export a formatted PDF health summary report.
    /// GET /api/v1/export/pdf?from=2026-05-01&amp;to=2026-06-20
    /// </summary>
    [HttpGet("pdf")]
    public async Task<IActionResult> ExportPdf(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to)
    {
        var userId = GetUserId();
        var dateFrom = from ?? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30));
        var dateTo = to ?? DateOnly.FromDateTime(DateTime.UtcNow);

        var moods = (await _moodRepo.GetByUserIdAsync(userId, dateFrom, dateTo)).ToList();
        var sleep = (await _sleepRepo.GetByUserIdAsync(userId, dateFrom, dateTo)).ToList();
        var symptoms = (await _symptomRepo.GetByUserIdAsync(userId, dateFrom, dateTo)).ToList();
        var meals = (await _mealRepo.GetByUserIdAsync(userId, dateFrom, dateTo)).ToList();

        var avgMood = moods.Any() ? moods.Average(m => m.MoodScore).ToString("F1") : "N/A";
        var avgSleep = sleep.Any() ? sleep.Average(s => s.HoursSlept).ToString("F1") : "N/A";
        var avgSleepQ = sleep.Any() ? sleep.Average(s => s.QualityScore).ToString("F1") : "N/A";

        var pdf = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                page.Header().Column(col =>
                {
                    col.Item().Text("HealthTracker AI — Health Report")
                        .FontSize(22).Bold().FontColor("#4F46E5");
                    col.Item().Text($"Period: {dateFrom:dd MMM yyyy} → {dateTo:dd MMM yyyy}")
                        .FontSize(12).FontColor("#6B7280");
                    col.Item().PaddingTop(4).LineHorizontal(1).LineColor("#E5E7EB");
                });

                page.Content().PaddingTop(16).Column(col =>
                {
                    // Summary stats
                    col.Item().Text("Summary Statistics").FontSize(15).Bold().FontColor("#1F2937");
                    col.Item().PaddingTop(8).Row(row =>
                    {
                        StatBox(row, "Avg Mood", $"{avgMood}/10", "#FEF3C7", "#D97706");
                        StatBox(row, "Avg Sleep", $"{avgSleep} hrs", "#EDE9FE", "#7C3AED");
                        StatBox(row, "Sleep Quality", $"{avgSleepQ}/10", "#DBEAFE", "#2563EB");
                        StatBox(row, "Symptoms", symptoms.Count.ToString(), "#FEE2E2", "#DC2626");
                    });

                    col.Item().PaddingTop(20).Text("Mood Logs").FontSize(14).Bold().FontColor("#1F2937");
                    col.Item().PaddingTop(6).Table(table =>
                    {
                        table.ColumnsDefinition(c => { c.RelativeColumn(2); c.RelativeColumn(1); c.RelativeColumn(5); });
                        TableHeader(table, "Date", "Score", "Notes");
                        foreach (var m in moods.Take(15))
                            TableRow(table, m.LogDate.ToString("dd MMM yyyy"), $"{m.MoodScore}/10", m.Notes ?? "");
                    });

                    col.Item().PaddingTop(20).Text("Sleep Logs").FontSize(14).Bold().FontColor("#1F2937");
                    col.Item().PaddingTop(6).Table(table =>
                    {
                        table.ColumnsDefinition(c => { c.RelativeColumn(2); c.RelativeColumn(1); c.RelativeColumn(1); c.RelativeColumn(1); c.RelativeColumn(1); });
                        TableHeader(table, "Date", "Hours", "Quality", "Bed", "Wake");
                        foreach (var s in sleep.Take(15))
                            TableRow(table, s.LogDate.ToString("dd MMM yyyy"), s.HoursSlept.ToString("F1"), $"{s.QualityScore}/10", s.BedTime.ToString(), s.WakeTime.ToString());
                    });

                    if (symptoms.Any())
                    {
                        col.Item().PaddingTop(20).Text("Symptoms").FontSize(14).Bold().FontColor("#1F2937");
                        col.Item().PaddingTop(6).Table(table =>
                        {
                            table.ColumnsDefinition(c => { c.RelativeColumn(2); c.RelativeColumn(2); c.RelativeColumn(1); c.RelativeColumn(4); });
                            TableHeader(table, "Date", "Symptom", "Severity", "Notes");
                            foreach (var s in symptoms)
                                TableRow(table, s.LogDate.ToString("dd MMM yyyy"), s.SymptomName, $"{s.Severity}/10", s.Notes ?? "");
                        });
                    }
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Generated by HealthTracker AI • ").FontColor("#9CA3AF");
                    text.Span(DateTime.UtcNow.ToString("dd MMM yyyy HH:mm UTC")).FontColor("#9CA3AF");
                });
            });
        });

        var bytes = pdf.GeneratePdf();
        var fileName = $"health-report-{dateFrom:yyyy-MM-dd}-to-{dateTo:yyyy-MM-dd}.pdf";
        return File(bytes, "application/pdf", fileName);
    }

    // ── PDF Helpers ────────────────────────────────────────────────────────────
    private static void StatBox(RowDescriptor row, string label, string value, string bg, string accent)
    {
        row.RelativeItem().Padding(4).Background(bg).Padding(8).Column(c =>
        {
            c.Item().Text(label).FontSize(9).FontColor(accent);
            c.Item().Text(value).FontSize(16).Bold().FontColor(accent);
        });
    }

    private static void TableHeader(TableDescriptor table, params string[] headers)
    {
        foreach (var h in headers)
            table.Header(hdr => hdr.Cell().Background("#F3F4F6").Padding(6)
                .Text(h).Bold().FontSize(10).FontColor("#374151"));
    }

    private static void TableRow(TableDescriptor table, params string[] values)
    {
        foreach (var v in values)
            table.Cell().BorderBottom(1).BorderColor("#F3F4F6").Padding(6)
                .Text(v).FontSize(10).FontColor("#4B5563");
    }
}

// ── CSV row models ─────────────────────────────────────────────────────────────
public record MoodCsvRow(string Date, int MoodScore, string Notes);
public record SleepCsvRow(string Date, double HoursSlept, int QualityScore, string BedTime, string WakeTime);
public record SymptomCsvRow(string Date, string SymptomName, int Severity, string Notes);
public record MealCsvRow(string Date, string MealType, string Description);
