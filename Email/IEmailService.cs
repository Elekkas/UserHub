namespace Email
{
    public interface IEmailService
    {
        void SendMail(EmailDto request);
    }
}