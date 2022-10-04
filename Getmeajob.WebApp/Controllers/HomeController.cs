using Getmeajob.Interface;
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
        private readonly ICompany _iCompany;
        private readonly IJobSeeker _iJobSeeker;
        private readonly IJob _iJob;
        private readonly IUser _iUser;

        public HomeController(ILogger<HomeController> logger, ICompany iCompany, IJobSeeker iJobSeeker, IUser iUser, IJob iJob)
        {
            _logger = logger;
            _iCompany = iCompany;
            _iJobSeeker = iJobSeeker;
            _iUser = iUser;
            _iJob = iJob;
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
        public IActionResult Contact(string stype, int id)
        {
            ContactVM con = new ContactVM();
            con.stype = stype;
            con.id = id;

            Random rd = new Random();

            con.No1 = rd.Next(1, 20);
            con.No2 = rd.Next(1, 20);

            con.result = con.No1 + con.No2;

            return View(con);
        }
        public async Task<IActionResult> SendMail(ContactVM contactVM)
        {
            try
            {
                if (contactVM != null)
                {
                    if (contactVM.Answer == contactVM.result)
                    {
                        var job = await _iJob.GetById(Convert.ToInt32(contactVM.id));

                        if (job != null)
                        {
                            if (contactVM.stype == "Company")
                            {
                                ViewBag.stype = "Company";
                                SendEmail("testg9921@gmail.com", "gqgerpwfmfnzgmdm",job.user?.Email,contactVM.Email, "getmeajob.com - Contact", "This is regarding "+job.JobTitle + " Job", contactVM?.Message);
                                ViewBag.success = true;
                            }
                            if (contactVM.stype == "Candidate")
                            {
                                ViewBag.stype = "Candidate";
                                SendEmail("testg9921@gmail.com", "gqgerpwfmfnzgmdm",job.user?.Email,contactVM.Email, "getmeajob.com - Contact", "This is regarding "+job.JobTitle + " Resume", contactVM?.Message);
                                ViewBag.success = true;
                            }

                        }

                        return View();

                    }
                    ViewBag.success = false;
                    return View();

                }
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction(nameof(Contact), contactVM);
        }
        public IActionResult ContactUs(QuestionVM? question)
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
            return View(nameof(ContactUs));
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