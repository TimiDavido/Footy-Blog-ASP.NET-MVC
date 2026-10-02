namespace FootyBlog.Application.Interfaces
{
    public interface IEmailService
    {
        Task<SendSingleMailResponse> SendEmail(SendSingleMailRequest dto);
    }
}
 