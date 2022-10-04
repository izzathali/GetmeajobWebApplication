using Getmeajob.Interface;
using Getmeajob.Model;
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
        public IActionResult Advertise()
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

                                string body = "<h2>This is regarding " + job.JobTitle + " Job </h2> <br/> <br/>  <b> Question from </b> : <p> " + contactVM.Email + "</p> <br/> <br/> <p> " + contactVM?.Message + " </p> ";

                                SendEmail("testg9921@gmail.com", "gqgerpwfmfnzgmdm",job.user?.Email, "getmeajob.com - Contact", body );
                                ViewBag.success = true;
                            }
                            if (contactVM.stype == "Candidate")
                            {
                                ViewBag.stype = "Candidate";

                                string body = "<h2>This is regarding " + job.JobTitle + " Resume </h2> <br/> <br/>  <b> Question from </b> : <p> " + contactVM.Email + "</p> <br/> <br/> <p> " + contactVM?.Message + " </p> ";

                                SendEmail("testg9921@gmail.com", "gqgerpwfmfnzgmdm",job.user?.Email, "getmeajob.com - Contact", body);
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
                    string body = "<h2>I still have a question</h2> <br/> <br/>  <b> Question from </b> : <p> " + questionVM?.contactVM?.Email + "</p> <br/> <br/> <p> " + questionVM?.contactVM?.Message + " </p> ";
                   
                    SendEmail("testg9921@gmail.com", "gqgerpwfmfnzgmdm", "izzath.info@gmail.com","getmeajob.com - Contact Us",body);
                    ViewBag.success = true;

                    return View();
                }
            }
            return View(nameof(ContactUs));
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(QuestionVM questionVM)
        {
            if (questionVM != null)
            {
                if (questionVM.forgotPassVM != null)
                {
                    UserM u = new UserM();
                    u.Email = questionVM.forgotPassVM.Email;
                    u.Type = questionVM.forgotPassVM.PostingType;

                    var user = await _iUser.GetByEmail(u);

                    if (user != null)
                    {
                        
                        string Body = @"<p>Your password for "+ user.Email +" is "+ user.Password + "</p> </br></br> " +
                            "Security check: </br></br> " +
                            "<p> You have received this email because you requested that your password be emailed to you AND when you submitted your job or resume, you had allowed your password to be mailed to you in the event that you forget it.  If this is not what you wanted, then please disregard this email or change your options by editing your posting.  Receiving this message does not put you on a mailing list.</p>";

                        SendEmail("testg9921@gmail.com", "gqgerpwfmfnzgmdm", questionVM?.forgotPassVM?.Email,"getmeajob.com - Your password request ", Body);
                        ViewBag.success = true;
                    }
                }
            }
            
            return View();
        }
        public void SendEmail(string fromEmail, string fromPassword, string ToEmail,string subject, string body)
        {
            try
            {
                // create email message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(fromEmail));
                email.To.Add(MailboxAddress.Parse(ToEmail));
                email.Subject = subject;
                //email.Body = new TextPart(TextFormat.Html) { Text = "<h2>" + bodyHeading + " </h2> <br/> <br/>  <b> Question from </b> : <p> " + SenderEmail + "</p> <br/> <br/> <p> " + body + " </p> " };
                email.Body = new TextPart(TextFormat.Html) { Text = body };

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