using SchoolV01.Application.Configurations;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Application.Requests.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.IO;

namespace SchoolV01.Infrastructure.Shared.Services
{
    public class SMTPMailService : IMailService
    {
        private readonly MailConfiguration _config;
        private readonly ILogger<SMTPMailService> _logger;
        private readonly MailSettings _settings;


        public SMTPMailService(IOptions<MailConfiguration> config, IOptions<MailSettings> settings, ILogger<SMTPMailService> logger)
        {
            _config = config.Value;
            _logger = logger;
            _settings = settings.Value;

        }

        public async Task SendAsync(MailRequest request)
        {
            try
            {
                var email = new MimeMessage
                {
                    Sender = new MailboxAddress(_config.DisplayName, request.From ?? _config.From),
                    Subject = request.Subject,
                    Body = new BodyBuilder
                    {
                        HtmlBody = request.Body
                    }.ToMessageBody()
                };
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_config.Host, _config.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_config.UserName, _config.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }



        public async Task<bool> SendAsync(MailData mailData, CancellationToken ct = default)
        {
            try
            {
                // Initialize a new instance of the MimeKit.MimeMessage class
                var mail = new MimeMessage();

                #region Sender / Receiver
                // Sender
                mail.From.Add(new MailboxAddress(_settings.DisplayName, mailData.From ?? _settings.From));
                mail.Sender = new MailboxAddress(mailData.DisplayName ?? _settings.DisplayName, mailData.From ?? _settings.From);

                // Receiver
                foreach (var mailAddress in mailData.To)
                    mail.To.Add(new MailboxAddress(mailAddress.ToName, mailAddress.ToMail));

                // Set Reply to if specified in mail data
                if (!string.IsNullOrEmpty(mailData.ReplyTo))
                    mail.ReplyTo.Add(new MailboxAddress(mailData.ReplyToName, mailData.ReplyTo));

                // BCC
                // Check if a BCC was supplied in the request
                //if (mailData.Bcc != null)
                //{
                //    // Get only addresses where value is not null or with whitespace. x = value of address
                //    foreach (string mailAddress in mailData.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
                //        mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                //}

                // CC
                // Check if a CC address was supplied in the request
                //if (mailData.Cc != null)
                //{
                //    foreach (string mailAddress in mailData.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
                //        mail.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                //}
                #endregion

                #region Content

                // Add Content to Mime Message
                mail.Subject = mailData.Subject;
                mail.Body = new BodyBuilder
                {
                    HtmlBody = mailData.Body
                }.ToMessageBody();



                #endregion

                #region Send Mail

                using var smtp = new SmtpClient();

                if (_settings.UseSSL)
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect, ct);
                }
                else if (_settings.UseStartTls)
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, ct);
                }
                else
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.None, ct);
                }

                await smtp.AuthenticateAsync(_settings.UserName, _settings.Password, ct);
                await smtp.SendAsync(mail, ct);
                await smtp.DisconnectAsync(true, ct);

                #endregion

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }



        public Task ContactUs(ContactUsEmailModel request)
        {
            throw new NotImplementedException();
        }
    }
}