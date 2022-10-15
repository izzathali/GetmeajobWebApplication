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
        private readonly IEmail _iEmail;
        private readonly IResume _iResume;

        public HomeController(ILogger<HomeController> logger, ICompany iCompany, IJobSeeker iJobSeeker, IUser iUser, IJob iJob, IEmail iEmail, IResume iResume)
        {
            _logger = logger;
            _iCompany = iCompany;
            _iJobSeeker = iJobSeeker;
            _iUser = iUser;
            _iJob = iJob;
            _iEmail = iEmail;
            _iResume = iResume;
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

                        if (contactVM.stype == "Company")
                        {
                            var job = await _iJob.GetById(Convert.ToInt32(contactVM.id));

                            if (job != null)
                            {

                                ViewBag.stype = "Company";

                                string host = Request.Scheme + "://" + Request.Host.Value;
                                string Url = host + "/User/DeleteAll?code=" + job.user.UrlCode;

                                string body = "<p>Do not forward or reply to this email.  It contains personal information that can be used to delete all your postings on getmeajob.com. </p> </br> " +
                                    "<p>You have received this email because of a response to a posting that you submitted on getmeajob.com.  To stop receiving all emails of this type, please visit www.getmeajob.com and delete your posting(s). </p> </br> " +
                                    "<p>To immediately delete all your postings, you may also visit </br> " + Url + " </p>" +
                                    "<p> " + contactVM.Email + " (" + contactVM.Name + ") writes: </p> </br>" +
                                    "<p>-----START OF MESSAGE-----</ p> </br>" +
                                    "<p> " + contactVM?.Message + " </p> </br>" +
                                    "<p>-----END OF MESSAGE----- </ p> </br>" +
                                    "<p>If desired, please contact " + contactVM.Email + " directly.</ p> </br>" +
                                    "<p>getmeajob.com is not responsible for the contents of the messages sent through the system and cannot provide further details or clarifications.  You are requested to use your own judgment and common sense. </ p> </br>" +
                                    "<p>Please do not reply to this email.  Replies to this mailbox may be ignored.  If you need to contact getmeajob.com, please visit http://www.getmeajob.com and click Contact Us. </ p> </br>";

                                _iEmail.SendEmail(new EmailVM
                                {
                                    EmailTo = job.user.Email,
                                    Subject = "getmeajob.com - Contact",
                                    Body = body
                                });

                                ViewBag.success = true;
                            }

                        }



                        if (contactVM.stype == "Candidate")
                        {
                            var res = await _iResume.GetById(Convert.ToInt32(contactVM.id));

                            if (res != null)
                            {

                                ViewBag.stype = "Candidate";

                                string host = Request.Scheme + "://" + Request.Host.Value;
                                string Url = host + "/User/DeleteAll?code=" + res.user.UrlCode;

                                string body = "<p>Do not forward or reply to this email.  It contains personal information that can be used to delete all your postings on getmeajob.com. </p> </br> " +
                                    "<p>You have received this email because of a response to a posting that you submitted on getmeajob.com.  To stop receiving all emails of this type, please visit www.getmeajob.com and delete your posting(s). </p> </br> " +
                                    "<p>To immediately delete all your postings, you may also visit </br> " + Url + " </p>" +
                                    "<p> " + contactVM.Email + " (" + contactVM.Name + ") writes: </p> </br>" +
                                    "<p>-----START OF MESSAGE-----</ p> </br>" +
                                    "<p> " + contactVM?.Message + " </p> </br>" +
                                    "<p>-----END OF MESSAGE----- </ p> </br>" +
                                    "<p>If desired, please contact " + contactVM.Email + " directly.</ p> </br>" +
                                    "<p>getmeajob.com is not responsible for the contents of the messages sent through the system and cannot provide further details or clarifications.  You are requested to use your own judgment and common sense. </ p> </br>" +
                                    "<p>Please do not reply to this email.  Replies to this mailbox may be ignored.  If you need to contact getmeajob.com, please visit http://www.getmeajob.com and click Contact Us. </ p> </br>";

                                _iEmail.SendEmail(new EmailVM
                                {
                                    EmailTo = res.user.Email,
                                    Subject = "getmeajob.com - Contact",
                                    Body = body
                                });


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

                    _iEmail.SendEmail(new EmailVM
                    {
                        EmailTo = "testg9921@gmail.com",
                        Subject = "getmeajob.com - Contact Us",
                        Body = body
                    });
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
                        if (user.IsEmailPassword == true)
                        {


                            string Body = @"<p>Your password for " + user.Email + " is " + user.Password + "</p> </br></br> " +
                                "Security check: </br></br> " +
                                "<p> You have received this email because you requested that your password be emailed to you AND when you submitted your job or resume, you had allowed your password to be mailed to you in the event that you forget it.  If this is not what you wanted, then please disregard this email or change your options by editing your posting.  Receiving this message does not put you on a mailing list.</p>";

                            _iEmail.SendEmail(new EmailVM
                            {
                                EmailTo = questionVM?.forgotPassVM?.Email,
                                Subject = "getmeajob.com - Your password request",
                                Body = Body
                            });

                            ViewBag.success = true;

                        }
                    }
                }
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}