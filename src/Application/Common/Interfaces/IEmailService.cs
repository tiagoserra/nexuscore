namespace Application.Common.Interfaces;

public interface IEmailService
{
    Task SendAsync(string subject, string to, string name, string htmlContent, string plainTextContent);

    Task SendWithAttachmentAsync(string subject, string to, string name, string htmlContent, string plainTextContent, string filePath);
}