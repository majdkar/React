using SchoolV01.Application.Interfaces.Services;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolV01.Application.Requests.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SportUnionV01.Server.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mail;

        public MailController(IMailService mail)
        {
            _mail = mail;
        }

        [HttpPost("SendEnail")]
        public async Task<IActionResult> ContactUs(MailData request)
        {
            var result = await _mail.SendAsync(request);
            return result ? StatusCode(StatusCodes.Status200OK, "Mail has successfully been sent.") : StatusCode(StatusCodes.Status400BadRequest, "Mail not sent.");
        }
    }
}