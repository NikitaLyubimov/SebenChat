using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

using DataBase.Repositories;
using DataBase.Entities;
using API.Tokens;
using API.ViewModels;
using Newtonsoft.Json;
using System.Web;

namespace API.Actions
{
    public class EmailActions
    {
        private EmailTokenReposytory _tokenReposytory;
        public EmailActions(EmailTokenReposytory tokenReposytory)
        {
            _tokenReposytory = tokenReposytory;
        }
        public async Task SenMessage(TokenFactory tokenFactory, string receiverEmail,long userId, string appUrl)
        {
            string emailConfToken = tokenFactory.GenerateToken();

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
            m.Body = $@"Confirm your email by following this link: https://{appUrl}/api/verify?token={HttpUtility.UrlEncode(encToken)}";


            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential(settings.Email, settings.Password);
            client.Host = settings.SmtpDomain;
            client.Credentials = new NetworkCredential(settings.Email, settings.Password);
            client.EnableSsl = true;
            client.Send(m);

        }
    }
}
