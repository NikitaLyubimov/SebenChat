using System;
using System.Web;
using System.IO;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

using Core.Interfaces.Gateways.Reposytories;
using Core.Interfaces.Services;
using Core.Interfaces.Helpers;
using Core.Domain.Entities;
using Core.DTO.Email;

using Newtonsoft.Json;


namespace Core.Helpers
{
    public class EmailActions : IEmailActions
    {
        private IEmailTokenReposytory _tokenReposytory;
        private ITokenFactory _tokenFactory;
        private string _appUrl = "localhost:51816";
        public EmailActions(IEmailTokenReposytory tokenReposytory, ITokenFactory tokenFactory)
        {
            _tokenReposytory = tokenReposytory;
            _tokenFactory = tokenFactory;
        }



        public async Task SendMessage(string receiverEmail,long userId)
        {
            string emailConfToken = _tokenFactory.GenerateToken();

            EmailConfirmToken newToken = new EmailConfirmToken(emailConfToken, userId);
            await _tokenReposytory.Add(newToken);

            EmailSettings settings = new EmailSettings();

            using (StreamReader reader = new StreamReader($@"{Environment.CurrentDirectory}\emailData.json"))
            {
                string json = reader.ReadToEnd();
                settings = JsonConvert.DeserializeObject<EmailSettings>(json);
            }

            var from = new MailAddress(settings.Email);
            var to = new MailAddress(receiverEmail);
            var m = new MailMessage(from, to);

            string encToken = emailConfToken.Trim().Replace("+", "%252b");

            m.Subject = "Confirm Email";
            m.Body = $@"Confirm your email by following this link: https://{_appUrl}/api/verify?token={HttpUtility.UrlEncode(encToken)}";


            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential(settings.Email, settings.Password);
            client.Host = settings.SmtpDomain;
            client.Credentials = new NetworkCredential(settings.Email, settings.Password);
            client.EnableSsl = true;
            client.Send(m);

        }
    }
}
