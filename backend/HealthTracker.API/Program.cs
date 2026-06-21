using System.Text;
using System.Threading.RateLimiting;
using Asp.Versioning;
using FluentValidation;
using Hangfire;
using Hangfire.SqlServer;
using HealthTracker.API.Hubs;
using HealthTracker.Application.Features.Auth.Handlers;
using HealthTracker.Application.Validators;
using HealthTracker.Domain.Interfaces;
using HealthTracker.Infrastructure.BackgroundJobs;
using HealthTracker.Infrastructure.Persistence;
using HealthTracker.Infrastructure.Persistence.Repositories;
using HealthTracker.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Infrastructure;
using Serilog;
using Serilog.Events;

// ── Serilog bootstrap logger ──────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("logs/healthtracker-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7)
    .CreateLogger();

try
{
    Log.Information("Starting HealthTracker API...");

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    // ── QuestPDF License (Community = free) ───────────────────────────────────
    QuestPDF.Settings.License = LicenseType.Community;

    // ── CORS ──────────────────────────────────────────────────────────────────
    var corsOrigins = builder.Configuration["CorsOrigins"]
        ?.Split(',', StringSplitOptions.RemoveEmptyEntries)
        ?? new[] { "http://localhost:5173" };

    builder.Services.AddCors(options =>
        options.AddPolicy("AllowFrontend", policy =>
            policy.WithOrigins(corsOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials()));   // Required for SignalR

    // ── Rate Limiting ─────────────────────────────────────────────────────────
    builder.Services.AddRateLimiter(options =>
    {
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

        // Auth endpoints: 5 requests/min per IP
        options.AddFixedWindowLimiter("auth", o =>
        {
            o.Window = TimeSpan.FromMinutes(1);
            o.PermitLimit = 5;
            o.QueueLimit = 0;
            o.AutoReplenishment = true;
        });

        // AI insights: 10 requests/min per IP
        options.AddFixedWindowLimiter("insights", o =>
        {
            o.Window = TimeSpan.FromMinutes(1);
            o.PermitLimit = 10;
            o.QueueLimit = 0;
            o.AutoReplenishment = true;
        });

        // Global fallback: 60 requests/min per IP
        options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
            RateLimitPartition.GetFixedWindowLimiter(
                ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                _ => new FixedWindowRateLimiterOptions
                {
                    Window = TimeSpan.FromMinutes(1),
                    PermitLimit = 60,
                    AutoReplenishment = true
                }));
    });

    // ── Database ──────────────────────────────────────────────────────────────
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // ── Hangfire ──────────────────────────────────────────────────────────────
    builder.Services.AddHangfire(config => config
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddHangfireServer();

    // ── Health Checks ─────────────────────────────────────────────────────────
    builder.Services.AddHealthChecks()
        .AddSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")!,
            name: "database",
            failureStatus: HealthStatus.Unhealthy,
            tags: new[] { "db", "sql" });

    // ── MediatR ───────────────────────────────────────────────────────────────
    builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(RegisterHandler).Assembly));

    // ── Validators ────────────────────────────────────────────────────────────
    builder.Services.AddValidatorsFromAssembly(typeof(CreateSymptomLogValidator).Assembly);

    // ── Repositories & Services ───────────────────────────────────────────────
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<ISymptomLogRepository, SymptomLogRepository>();
    builder.Services.AddScoped<IMoodLogRepository, MoodLogRepository>();
    builder.Services.AddScoped<ISleepLogRepository, SleepLogRepository>();
    builder.Services.AddScoped<IMealLogRepository, MealLogRepository>();
    builder.Services.AddScoped<IWeeklyReportRepository, WeeklyReportRepository>();
    builder.Services.AddScoped<WeeklyReportJob>();
    builder.Services.AddScoped<IEmailService, EmailService>();
    builder.Services.AddHttpClient<IAiAnalysisService, OpenAiAnalysisService>();

    // ── Caching ───────────────────────────────────────────────────────────────
    builder.Services.AddMemoryCache();

    // ── SignalR ───────────────────────────────────────────────────────────────
    builder.Services.AddSignalR();

    // ── API Versioning ────────────────────────────────────────────────────────
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    }).AddMvc();

    // ── JWT Authentication ────────────────────────────────────────────────────
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!))
            };
            // Allow SignalR to get token from query string
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = ctx =>
                {
                    var token = ctx.Request.Query["access_token"];
                    if (!string.IsNullOrEmpty(token) &&
                        ctx.HttpContext.Request.Path.StartsWithSegments("/hubs"))
                        ctx.Token = token;
                    return Task.CompletedTask;
                }
            };
        });

    builder.Services.AddAuthorization();
    builder.Services.AddControllers();

    var app = builder.Build();

    // ── Middleware pipeline ───────────────────────────────────────────────────
    app.UseSerilogRequestLogging(opts =>
    {
        opts.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    });

    app.UseCors("AllowFrontend");
    app.UseRateLimiter();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    // ── Health check endpoint ─────────────────────────────────────────────────
    app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        ResponseWriter = async (context, report) =>
        {
            context.Response.ContentType = "application/json";
            var result = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description
                }),
                duration = report.TotalDuration.TotalMilliseconds.ToString("F2") + "ms"
            };
            await context.Response.WriteAsJsonAsync(result);
        }
    });

    // ── SignalR Hub ───────────────────────────────────────────────────────────
    app.MapHub<NotificationsHub>("/hubs/notifications");

    // ── Hangfire ──────────────────────────────────────────────────────────────
    app.UseHangfireDashboard("/hangfire");
    RecurringJob.AddOrUpdate<WeeklyReportJob>(
        "weekly-report",
        job => job.GenerateWeeklyReportsAsync(),
        "0 8 * * 0");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}