using System.Net;
using System.Net.Mail;
using HealthTracker.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HealthTracker.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendHealthAlertAsync(string toEmail, string userName, string alertTitle, string alertMessage)
    {
        var subject = $"🚨 Health Alert: {alertTitle}";
        var body = BuildAlertEmailBody(userName, alertTitle, alertMessage);
        await SendAsync(toEmail, subject, body);
    }

    public async Task SendWeeklyReportAsync(string toEmail, string userName, string weekRange, string insights)
    {
        var subject = $"📊 Your Weekly Health Report — {weekRange}";
        var body = BuildWeeklyReportEmailBody(userName, weekRange, insights);
        await SendAsync(toEmail, subject, body);
    }

    public async Task SendWelcomeEmailAsync(string toEmail, string firstName)
    {
        var subject = "👋 Welcome to HealthTracker AI!";
        var body = BuildWelcomeEmailBody(firstName);
        await SendAsync(toEmail, subject, body);
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    private async Task SendAsync(string toEmail, string subject, string htmlBody)
    {
        var smtpSection = _configuration.GetSection("Smtp");
        var host     = smtpSection["Host"]     ?? "smtp.gmail.com";
        var port     = int.Parse(smtpSection["Port"] ?? "587");
        var from     = smtpSection["From"]     ?? "noreply@healthtracker.ai";
        var password = smtpSection["Password"] ?? string.Empty;
        var enableSsl = bool.Parse(smtpSection["EnableSsl"] ?? "true");

        if (string.IsNullOrWhiteSpace(password))
        {
            _logger.LogWarning("SMTP password not configured — skipping email to {Email}", toEmail);
            return;
        }

        using var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(from, password),
            EnableSsl    = enableSsl,
        };

        using var message = new MailMessage
        {
            From       = new MailAddress(from, "HealthTracker AI"),
            Subject    = subject,
            Body       = htmlBody,
            IsBodyHtml = true,
        };
        message.To.Add(toEmail);

        try
        {
            await client.SendMailAsync(message);
            _logger.LogInformation("Email sent to {Email}: {Subject}", toEmail, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}", toEmail);
            // Non-fatal: log and continue, don't crash the calling request
        }
    }

    private static string BuildAlertEmailBody(string userName, string title, string message) => $"""
        <!DOCTYPE html>
        <html>
        <head><meta charset="utf-8"></head>
        <body style="font-family: 'Segoe UI', sans-serif; background:#f0f4ff; margin:0; padding:32px;">
          <div style="max-width:560px; margin:auto; background:#fff; border-radius:16px; overflow:hidden; box-shadow:0 4px 20px rgba(0,0,0,0.08);">
            <div style="background:linear-gradient(135deg,#ef4444,#dc2626); padding:28px 32px;">
              <h1 style="color:#fff; margin:0; font-size:22px;">🚨 Health Alert</h1>
            </div>
            <div style="padding:32px;">
              <p style="color:#374151; font-size:15px;">Hi <strong>{userName}</strong>,</p>
              <p style="color:#374151; font-size:15px;">Your HealthTracker AI has detected something that needs your attention:</p>
              <div style="background:#fef2f2; border-left:4px solid #ef4444; border-radius:8px; padding:16px 20px; margin:20px 0;">
                <p style="color:#991b1b; font-weight:700; margin:0 0 6px 0; font-size:16px;">{title}</p>
                <p style="color:#7f1d1d; margin:0; font-size:14px; line-height:1.6;">{message}</p>
              </div>
              <p style="color:#6b7280; font-size:13px;">Log into your dashboard to review your health data and take action.</p>
              <a href="http://localhost:5173/alerts" style="display:inline-block; background:linear-gradient(135deg,#6366f1,#8b5cf6); color:#fff; text-decoration:none; padding:12px 24px; border-radius:10px; font-weight:600; font-size:14px; margin-top:8px;">View Alerts</a>
            </div>
            <div style="background:#f9fafb; padding:16px 32px; text-align:center;">
              <p style="color:#9ca3af; font-size:12px; margin:0;">HealthTracker AI · Your personal health companion</p>
            </div>
          </div>
        </body>
        </html>
        """;

    private static string BuildWeeklyReportEmailBody(string userName, string weekRange, string insights) => $"""
        <!DOCTYPE html>
        <html>
        <head><meta charset="utf-8"></head>
        <body style="font-family: 'Segoe UI', sans-serif; background:#f0f4ff; margin:0; padding:32px;">
          <div style="max-width:560px; margin:auto; background:#fff; border-radius:16px; overflow:hidden; box-shadow:0 4px 20px rgba(0,0,0,0.08);">
            <div style="background:linear-gradient(135deg,#6366f1,#8b5cf6); padding:28px 32px;">
              <h1 style="color:#fff; margin:0; font-size:22px;">📊 Weekly Health Report</h1>
              <p style="color:#c7d2fe; margin:6px 0 0 0; font-size:14px;">{weekRange}</p>
            </div>
            <div style="padding:32px;">
              <p style="color:#374151; font-size:15px;">Hi <strong>{userName}</strong>,</p>
              <p style="color:#374151; font-size:15px;">Here's your AI-generated health summary for the past week:</p>
              <div style="background:#f5f3ff; border-left:4px solid #6366f1; border-radius:8px; padding:16px 20px; margin:20px 0; white-space:pre-line; color:#3730a3; font-size:14px; line-height:1.7;">{insights}</div>
              <a href="http://localhost:5173/reports" style="display:inline-block; background:linear-gradient(135deg,#6366f1,#8b5cf6); color:#fff; text-decoration:none; padding:12px 24px; border-radius:10px; font-weight:600; font-size:14px; margin-top:8px;">View Full Report</a>
            </div>
            <div style="background:#f9fafb; padding:16px 32px; text-align:center;">
              <p style="color:#9ca3af; font-size:12px; margin:0;">HealthTracker AI · Your personal health companion</p>
            </div>
          </div>
        </body>
        </html>
        """;

    private static string BuildWelcomeEmailBody(string firstName) => $"""
        <!DOCTYPE html>
        <html>
        <head><meta charset="utf-8"></head>
        <body style="font-family: 'Segoe UI', sans-serif; background:#f0f4ff; margin:0; padding:32px;">
          <div style="max-width:560px; margin:auto; background:#fff; border-radius:16px; overflow:hidden; box-shadow:0 4px 20px rgba(0,0,0,0.08);">
            <div style="background:linear-gradient(135deg,#10b981,#059669); padding:28px 32px;">
              <h1 style="color:#fff; margin:0; font-size:22px;">👋 Welcome to HealthTracker AI!</h1>
            </div>
            <div style="padding:32px;">
              <p style="color:#374151; font-size:15px;">Hi <strong>{firstName}</strong>, welcome aboard! 🎉</p>
              <p style="color:#374151; font-size:15px;">You're all set to start tracking your health journey. Here's what you can do:</p>
              <ul style="color:#374151; font-size:14px; line-height:2;">
                <li>📝 <strong>Log daily</strong> — symptoms, mood, sleep and meals</li>
                <li>🧠 <strong>Get AI insights</strong> — discover patterns in your health data</li>
                <li>📊 <strong>Weekly reports</strong> — automatically generated every Sunday</li>
                <li>🔔 <strong>Health alerts</strong> — proactive warnings based on your trends</li>
              </ul>
              <a href="http://localhost:5173/" style="display:inline-block; background:linear-gradient(135deg,#10b981,#059669); color:#fff; text-decoration:none; padding:12px 24px; border-radius:10px; font-weight:600; font-size:14px; margin-top:8px;">Go to Dashboard</a>
            </div>
            <div style="background:#f9fafb; padding:16px 32px; text-align:center;">
              <p style="color:#9ca3af; font-size:12px; margin:0;">HealthTracker AI · Your personal health companion</p>
            </div>
          </div>
        </body>
        </html>
        """;
}
