using Getmeajob.ViewModel;
using Getmeajob.WebApp.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using System.Diagnostics;

namespace Getmeajob.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Legal()
        {
            return View();
        }
        public IActionResult Tips(string type)
        {
            ViewBag.Type = type;
            return View();
        }
        public IActionResult Contact(QuestionVM? question)
        {
            return View(question);
        }
        [HttpPost]
        public IActionResult Emailed(QuestionVM questionVM)
        {
            if (questionVM != null)
            {
                if (questionVM.contactVM != null)
                {
                    SendEmail("testg9921@gmail.com", "gqgerpwfmfnzgmdm", "izzath.info@gmail.com", questionVM?.contactVM?.Email, "getmeajob.com - Contact Us", "I still have a question", questionVM?.contactVM?.Message);
                    return View();
                }
            }
            return View(nameof(Contact));
        }

        public void SendEmail(string fromEmail, string fromPassword, string ToEmail, string SenderEmail, string subject, string bodyHeading, string body)
        {
            try
            {
                // create email message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(fromEmail));
                email.To.Add(MailboxAddress.Parse(ToEmail));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = "<h2>" + bodyHeading + " </h2> <br/> <br/>  <b> Question from </b> : <p> " + SenderEmail + "</p> <br/> <br/> <p> " + body + " </p> " };

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}