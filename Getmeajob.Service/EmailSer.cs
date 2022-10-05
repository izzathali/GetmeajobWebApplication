using Getmeajob.Interface;
using Getmeajob.ViewModel;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getmeajob.Service
{
    public class EmailSer : IEmail
    {
        public void SendEmail(EmailVM e)
        {
            //temp
            string fromEmail = "testg9921@gmail.com";
            string fromPassword = "gqgerpwfmfnzgmdm";

            try
            {
                // create email message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(fromEmail));
                email.To.Add(MailboxAddress.Parse(e.EmailTo));
                email.Subject = e.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = e.Body };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                smtp.Authenticate(fromEmail, fromPassword);
                smtp.Send(email);
                smtp.Disconnect(true);

            }
            catch (Exception ex)
            {

            }
        }
    }
}
