using CommonLayer.Models;
using MassTransit;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FundooSubscriberApp.Services
{
    public class UserRegistrationEmailSubscriber : IConsumer<UserRegistrationMessage>
    {

        public async Task Consume(ConsumeContext<UserRegistrationMessage> context)
        {
            var userRegistrationMessage = context.Message;

            await SendWelcomeEmail(userRegistrationMessage.Email);
        }

        private async Task SendWelcomeEmail(string email)
        {
            try
            {
                using (SmtpClient  smtpClient = new SmtpClient("smtp.gmail.com")) 
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential("sainikhil392@gmail.com", "njbmrjdmoxasqrpc");
                    smtpClient.EnableSsl = true;

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress("sainikhil392@gmail.com");
                        mailMessage.To.Add(email);
                        mailMessage.Subject = "Welcome to Our App!";
                        mailMessage.Body = "Thank you for registering. Welcome to our app!";
                        mailMessage.IsBodyHtml = true;

                        //send the email
                        smtpClient.Send(mailMessage);
                    }
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
    }
}
