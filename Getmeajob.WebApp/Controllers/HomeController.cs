using Getmeajob.ViewModel;
using Getmeajob.WebApp.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
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
            SendEmail("testg9921@gmail.com", "Password@123","izzath.info@gmail.com","this is subject","this is body");
            return View();
        }

        public void SendEmail(string fromEmail, string fromPassword, string sendEmail,string subject,string body)
        {
            try
            {

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Contact Us", fromEmail));
                message.To.Add(new MailboxAddress("pritom", sendEmail));
                message.Subject = "Getmeajob Contact Us";
                message.Body = new TextPart("plain")
                {
                    Text = body,
                };
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate(fromEmail, fromPassword);

                    client.Send(message);
                    client.Disconnect(true);
                }

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