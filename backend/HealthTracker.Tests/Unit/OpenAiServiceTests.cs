using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using HealthTracker.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using HealthTracker.Infrastructure.Services;

namespace HealthTracker.Tests.Unit;

public class OpenAiServiceTests
{
    // ── Config helper ─────────────────────────────────────────────────────────

    private static IConfiguration BuildConfig(string apiKey = "test-key", string model = "gpt-4o-mini")
    {
        var settings = new Dictionary<string, string?>
        {
            ["OpenAI:ApiKey"] = apiKey,
            ["OpenAI:Model"]  = model,
        };
        return new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
    }

    private static HealthDataContext EmptyContext() => new()
    {
        From     = new DateOnly(2025, 6, 1),
        To       = new DateOnly(2025, 6, 7),
        Symptoms = [],
        Moods    = [],
        Sleep    = [],
        Meals    = []
    };

    private static string BuildOpenAiResponse(string content) =>
        JsonSerializer.Serialize(new
        {
            choices = new[] { new { message = new { content } } }
        });

    // ── Tests ─────────────────────────────────────────────────────────────────

    [Fact]
    public async Task AnalyzeHealthLogsAsync_SuccessfulResponse_ReturnsInsightText()
    {
        // Arrange
        const string expectedInsight = "Your headaches correlate with poor sleep.";
        var http   = new HttpClient(new FakeHttpMessageHandler(BuildOpenAiResponse(expectedInsight)));
        var sut    = new OpenAiAnalysisService(http, BuildConfig());

        var ctx = new HealthDataContext
        {
            From     = new DateOnly(2025, 6, 1),
            To       = new DateOnly(2025, 6, 7),
            Symptoms = ["Headache severity 8 on 2025-06-02"],
            Moods    = ["Mood 5 on 2025-06-02"],
            Sleep    = ["5.0 hrs on 2025-06-01"],
            Meals    = []
        };

        // Act
        var result = await sut.AnalyzeHealthLogsAsync(ctx);

        // Assert
        result.Should().Be(expectedInsight);
    }

    [Fact]
    public async Task AnalyzeHealthLogsAsync_ApiError_ThrowsException()
    {
        // Arrange
        var http = new HttpClient(new FakeHttpMessageHandler(
            "{\"error\":\"invalid_api_key\"}", HttpStatusCode.Unauthorized));
        var sut  = new OpenAiAnalysisService(http, BuildConfig());

        // Act & Assert
        await sut.Invoking(s => s.AnalyzeHealthLogsAsync(EmptyContext()))
            .Should().ThrowAsync<Exception>()
            .Where(ex => ex.Message.Contains("OpenAI API error"));
    }

    [Fact]
    public async Task AnalyzeHealthLogsAsync_EmptyHealthData_StillCallsApi()
    {
        // Arrange
        const string expected = "No significant patterns detected.";
        var http = new HttpClient(new FakeHttpMessageHandler(BuildOpenAiResponse(expected)));
        var sut  = new OpenAiAnalysisService(http, BuildConfig());

        // Act
        var result = await sut.AnalyzeHealthLogsAsync(EmptyContext());

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public async Task AnalyzeHealthLogsAsync_UsesConfiguredModel()
    {
        // Arrange
        string? capturedBody = null;
        var handler = new CapturingHttpMessageHandler(
            req => { capturedBody = req.Content!.ReadAsStringAsync().Result; },
            BuildOpenAiResponse("ok"));

        var sut = new OpenAiAnalysisService(new HttpClient(handler),
            BuildConfig(model: "llama-3.1-8b-instant"));

        // Act
        await sut.AnalyzeHealthLogsAsync(EmptyContext());

        // Assert
        capturedBody.Should().Contain("llama-3.1-8b-instant");
    }

    [Fact]
    public async Task AnalyzeHealthLogsAsync_SetsAuthorizationHeader()
    {
        // Arrange
        string? capturedAuthHeader = null;
        var handler = new CapturingHttpMessageHandler(
            req => { capturedAuthHeader = req.Headers.Authorization?.ToString(); },
            BuildOpenAiResponse("ok"));

        var sut = new OpenAiAnalysisService(new HttpClient(handler),
            BuildConfig(apiKey: "my-secret-key"));

        // Act
        await sut.AnalyzeHealthLogsAsync(EmptyContext());

        // Assert
        capturedAuthHeader.Should().Be("Bearer my-secret-key");
    }

    // ── Fake HTTP handlers ────────────────────────────────────────────────────

    private sealed class FakeHttpMessageHandler(
        string body,
        HttpStatusCode code = HttpStatusCode.OK) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(code)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            });
        }
    }

    private sealed class CapturingHttpMessageHandler(
        Action<HttpRequestMessage> capture,
        string responseBody) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            capture(request);
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseBody, Encoding.UTF8, "application/json")
            });
        }
    }
}
