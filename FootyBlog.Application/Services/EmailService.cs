using FootyBlog.Application.Interfaces;
using System.Net.Http.Json;

namespace FootyBlog.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly HttpClient _httpClient;
        public EmailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<SendSingleMailResponse> SendEmail(SendSingleMailRequest dto)
        {
           var response = await _httpClient.PostAsJsonAsync("/api/Notify/SendSingleMail", dto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<SendSingleMailResponse>();
                return result!;
            }
            else
            {
                return new SendSingleMailResponse
                {
                    ResponseCode = (int)response.StatusCode,
                    ResponseMessage = response.ReasonPhrase
                };
            }
        }
    }
}
