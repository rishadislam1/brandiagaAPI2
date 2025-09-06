namespace brandiagaAPI2.Interfaces.ServiceInterfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}

