using System.Text;
using System.Text.Json;
using HealthTracker.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace HealthTracker.Infrastructure.Services;

public class OpenAiAnalysisService : IAiAnalysisService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public OpenAiAnalysisService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> AnalyzeHealthLogsAsync(HealthDataContext context)
    {
        var apiKey = _configuration["OpenAI:ApiKey"];
        var model = _configuration["OpenAI:Model"] ?? "gpt-4o-mini";

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        // Build the health data summary
        var sb = new StringBuilder();
        sb.AppendLine($"Health data from {context.From} to {context.To}:");
        sb.AppendLine();

        if (context.Symptoms.Any())
        {
            sb.AppendLine("SYMPTOMS:");
            context.Symptoms.ForEach(s => sb.AppendLine($"  - {s}"));
            sb.AppendLine();
        }

        if (context.Moods.Any())
        {
            sb.AppendLine("MOOD:");
            context.Moods.ForEach(m => sb.AppendLine($"  - {m}"));
            sb.AppendLine();
        }

        if (context.Sleep.Any())
        {
            sb.AppendLine("SLEEP:");
            context.Sleep.ForEach(s => sb.AppendLine($"  - {s}"));
            sb.AppendLine();
        }

        if (context.Meals.Any())
        {
            sb.AppendLine("MEALS:");
            context.Meals.ForEach(m => sb.AppendLine($"  - {m}"));
        }

        var prompt = $"""
            You are a helpful health analyst. Analyze the following health log data and provide:
            1. 3 specific patterns or correlations you notice (e.g. "Your headaches tend to occur after less than 6 hours of sleep")
            2. 2 actionable recommendations based on the data
            3. A brief overall health summary for this period

            Be specific, reference actual dates and values from the data. Keep the response concise and easy to read.
            Use plain text without markdown formatting.

            {sb}
            """;

        var requestBody = new
        {
            model,
            max_tokens = 1000,
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);
        var responseJson = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"OpenAI API error: {responseJson}");

        using var doc = JsonDocument.Parse(responseJson);
        var aiResponse = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return aiResponse ?? "Unable to generate insights at this time.";
    }
}