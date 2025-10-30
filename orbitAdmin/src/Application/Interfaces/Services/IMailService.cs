using SchoolV01.Application.Requests.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Interfaces.Services
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);
        Task<bool> SendAsync(MailData request, CancellationToken ct = default);
        Task ContactUs(ContactUsEmailModel request);
    }
}