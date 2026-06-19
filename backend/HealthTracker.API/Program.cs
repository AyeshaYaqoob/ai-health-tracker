using System.Text;
using HealthTracker.Application.Features.Auth.Handlers;
using HealthTracker.Domain.Interfaces;
using HealthTracker.Infrastructure.Persistence;
using HealthTracker.Infrastructure.Persistence.Repositories;
using HealthTracker.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Hangfire;
using Hangfire.SqlServer;
using HealthTracker.Infrastructure.BackgroundJobs;
using FluentValidation;
using HealthTracker.Application.Validators;
var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer();
// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegisterHandler).Assembly));

// Repositories & Services
builder.Services.AddValidatorsFromAssembly(typeof(CreateSymptomLogValidator).Assembly);
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ISymptomLogRepository, SymptomLogRepository>();
builder.Services.AddScoped<IMoodLogRepository, MoodLogRepository>();
builder.Services.AddScoped<ISleepLogRepository, SleepLogRepository>();
builder.Services.AddScoped<IMealLogRepository, MealLogRepository>();
builder.Services.AddScoped<IWeeklyReportRepository, WeeklyReportRepository>();
builder.Services.AddScoped<WeeklyReportJob>();
// OpenAI Service
builder.Services.AddHttpClient<IAiAnalysisService, OpenAiAnalysisService>();
// JWT Authentication
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
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHangfireDashboard("/hangfire");
RecurringJob.AddOrUpdate<WeeklyReportJob>(
    "weekly-report",
    job => job.GenerateWeeklyReportsAsync(),
    "0 8 * * 0");
// app.MapGet("/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
//     string.Join("\n", endpointSources
//         .SelectMany(es => es.Endpoints)
//         .Select(e => e.DisplayName)));
// app.MapPost("/test", () => "test works");
app.Run();