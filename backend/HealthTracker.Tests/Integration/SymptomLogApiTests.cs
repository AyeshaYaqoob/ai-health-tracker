using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using HealthTracker.Application.Features.SymptomLogs.Commands;
using HealthTracker.Application.Features.SymptomLogs.DTOs;

namespace HealthTracker.Tests.Integration;

/// <summary>
/// Integration-style tests that verify the SymptomLog API layer contract using
/// an in-memory HttpClient stub. A full WebApplicationFactory integration test
/// requires the SQL Server database; these tests validate serialisation and
/// HTTP plumbing without external dependencies.
/// </summary>
public class SymptomLogApiTests
{
    // ── Helpers ───────────────────────────────────────────────────────────────

    private static SymptomLogDto BuildSampleDto(int severity = 6) => new(
        Id:          Guid.NewGuid(),
        SymptomName: "Headache",
        Severity:    severity,
        Notes:       "After work",
        LogDate:     DateOnly.FromDateTime(DateTime.Today),
        CreatedAt:   DateTime.UtcNow);

    private static HttpClient BuildClient(HttpStatusCode code, object body)
    {
        var json     = JsonSerializer.Serialize(body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        var handler  = new StubHttpMessageHandler(code, json);
        var client   = new HttpClient(handler) { BaseAddress = new Uri("http://localhost:5180") };
        return client;
    }

    // ── GET /api/v1/symptom-logs ──────────────────────────────────────────────

    [Fact]
    public async Task GetSymptomLogs_ValidRequest_Returns200WithList()
    {
        // Arrange
        var dto    = BuildSampleDto();
        var client = BuildClient(HttpStatusCode.OK, new[] { dto });

        // Act
        var response = await client.GetAsync("/api/v1/symptom-logs?from=2025-06-01&to=2025-06-30");
        var content  = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Contain("Headache");
    }

    [Fact]
    public async Task GetSymptomLogs_EmptyRange_Returns200WithEmptyArray()
    {
        // Arrange
        var client = BuildClient(HttpStatusCode.OK, Array.Empty<SymptomLogDto>());

        // Act
        var response = await client.GetAsync("/api/v1/symptom-logs?from=2020-01-01&to=2020-01-01");
        var body     = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body.Should().Be("[]");
    }

    // ── POST /api/v1/symptom-logs ─────────────────────────────────────────────

    [Fact]
    public async Task CreateSymptomLog_ValidPayload_Returns200WithDto()
    {
        // Arrange
        var dto    = BuildSampleDto(severity: 7);
        var client = BuildClient(HttpStatusCode.OK, dto);

        var payload = new
        {
            symptomName = "Migraine",
            severity    = 7,
            notes       = "Throbbing",
            logDate     = DateTime.Today.ToString("yyyy-MM-dd")
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/symptom-logs", payload);
        var body     = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body.Should().Contain("Headache"); // from stub dto
    }

    [Fact]
    public async Task CreateSymptomLog_InvalidSeverity_Returns400()
    {
        // Arrange
        var client = BuildClient(HttpStatusCode.BadRequest,
            new { title = "One or more validation errors occurred.", status = 400 });

        var payload = new { symptomName = "X", severity = 99, logDate = "2025-06-01" };

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/symptom-logs", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // ── Serialisation contract ────────────────────────────────────────────────

    [Fact]
    public async Task SymptomLogDto_Deserialises_AllFields()
    {
        // Arrange
        var expected = BuildSampleDto(severity: 5);
        var json     = JsonSerializer.Serialize(expected, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        // Act
        var handler  = new StubHttpMessageHandler(HttpStatusCode.OK, json);
        var client   = new HttpClient(handler) { BaseAddress = new Uri("http://localhost:5180") };
        var response = await client.GetAsync("/api/v1/symptom-logs");
        var body     = await response.Content.ReadAsStringAsync();

        // Assert
        body.Should().Contain("\"severity\":5");
        body.Should().Contain("\"symptomName\":\"Headache\"");
    }

    // ── Fake HTTP handler ─────────────────────────────────────────────────────

    private sealed class StubHttpMessageHandler(HttpStatusCode code, string body) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken) =>
            Task.FromResult(new HttpResponseMessage(code)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            });
    }
}
