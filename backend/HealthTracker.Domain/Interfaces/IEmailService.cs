namespace HealthTracker.Domain.Interfaces;

public interface IEmailService
{
    Task SendHealthAlertAsync(string toEmail, string userName, string alertTitle, string alertMessage);
    Task SendWeeklyReportAsync(string toEmail, string userName, string weekRange, string insights);
    Task SendWelcomeEmailAsync(string toEmail, string firstName);
}
