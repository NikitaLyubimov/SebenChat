using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

using DataBase.Repositories;
using API.Tokens;
using API.ViewModels;
using Newtonsoft.Json;


namespace API.Actions
{
    public class EmailActions
    {
        private EmailTokenReposytory _tokenReposytory;
        public EmailActions(EmailTokenReposytory tokenReposytory)
        {
            _tokenReposytory = tokenReposytory;
        }
        public void SenMessage(TokenFactory tokenFactory, string receiverEmail, string appUrl)
        {
            var emailConfToken = tokenFactory.GenerateToken();

            EmailSettings settings = new EmailSettings();
            using (StreamReader reader = new StreamReader($@"{Environment.CurrentDirectory}\emailData.json"))
            {
                string json = reader.ReadToEnd();
                settings = JsonConvert.DeserializeObject<EmailSettings>(json);
            }

            var emailMessage = new MailMessage();

            emailMessage.From = new MailAddress(settings.Email);
            emailMessage.To.Add(new MailAddress(receiverEmail));
            emailMessage.Subject = "Confirm Email";
            emailMessage.Body = $@"Confirm your email by following this link: {appUrl}/verify?token={emailConfToken}";


            SmtpClient client = new SmtpClient();
            client.Host = settings.SmtpDomain;
            client.Port = 587;
            client.Credentials = new NetworkCredential(settings.Email, settings.Password);
            client.Send(emailMessage);

        }
    }
}
