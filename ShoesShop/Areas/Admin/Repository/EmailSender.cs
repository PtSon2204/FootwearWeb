
using System.Net;
using System.Net.Mail;

namespace ShoesShop.Areas.Admin.Repository
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        { 
            var client = new SmtpClient("smtp.gmail.com", 587) //465 và 587 có 2 cổng nhưng 587 bảo mật hơn
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("oniichanbaka204@gmail.com", "gxhpbvkbsbeiewcy")
            };

            return client.SendMailAsync(
                new MailMessage(from: "oniichanbaka204@gmail.com",
                                to: email, 
                                subject,
                                message));
        }
    }
}
