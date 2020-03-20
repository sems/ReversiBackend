using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ReversiApp.services
{
    public class AuthorizedMessageSender : IEmailSender
    {
        private string _host;
        private int _port;
        private bool _SSL;
        private string _userName;
        private string _password;

        public AuthorizedMessageSender(string host, int port, bool ssl, string userName, string password)
        {
            this._host = host;
            this._port = port;
            this._SSL = ssl;
            this._userName = userName;
            this._password = password;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_userName, _password),
                EnableSsl = _SSL
            };
            return client.SendMailAsync(
                new MailMessage(_userName, email, subject, htmlMessage) { IsBodyHtml = true }
                );
        }
    }
}
