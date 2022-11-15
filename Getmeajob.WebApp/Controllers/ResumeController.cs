using AspNetCoreHero.ToastNotification.Abstractions;
using Getmeajob.Interface;
using Getmeajob.Model;
using Getmeajob.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;

namespace Getmeajob.WebApp.Controllers
{
    public class ResumeController : Controller
    {
        private readonly IResume _iResume;
        private readonly IUser _iUser;
        private readonly IJobSeeker _iJobSeeker;
        private readonly IEmail _iEmail;
        IMemoryCache memoryCache;
        public INotyfService _notifyService { get; }


        private UserM Usr = new UserM();

        public ResumeController(IResume iResume, IUser iUser, IJobSeeker iJobSeeker, IEmail iEmail, IMemoryCache memoryCache,INotyfService notyfService)
        {
            _iResume = iResume;
            _iUser = iUser;
            _iJobSeeker = iJobSeeker;
            _iEmail = iEmail;
            this.memoryCache = memoryCache;
            _notifyService = notyfService;

            Usr = memoryCache.Get("LoggedUser") as UserM;

        }
        // GET: ResumeController
        public async Task<ActionResult> Index(int[] uid, string page)
        {
            //ViewBag.page = page;
            //var jobs = await _iResume.GetAllByUserId(uid);

            ViewBag.page = page;
            List<ResumeM> resumes = new List<ResumeM>();

            for (int i = 0; i < uid.Count(); i++)
            {
                IList<ResumeM> res = await _iResume.GetAllByUid(uid[i]);
                resumes.AddRange(res);
            }
            return View(resumes);
        }
        public async Task<ActionResult> Unapproved()
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Admin", "User");
            }

            var resumes = await _iResume.GetAllUnapproved();
            return View(resumes);
        }
        
        public async Task<ActionResult> approve(int rid)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Admin", "User");
            }

            if (rid > 0)
            {
                ResumeM res = await _iResume.GetById(rid);

                if (res != null)
                {
                    if (res.IsEmailVerified == true)
                    {

                        res.IsApproved = true;
                        int changes = await _iResume.Update(res);
                        if (changes > 0)
                        {
                            _notifyService.Success(res.JobTitle + " Resume Approved ");
                        }
                        else
                        {
                            _notifyService.Error("Error! Something went wrong!");
                        }
                    }
                    else
                    {
                        _notifyService.Error("Email not verified!");
                    }
                }
                else
                {
                    _notifyService.Error("Resume not found!");
                }

            }
            return RedirectToAction(nameof(Unapproved));
        }

        // GET: ResumeController/Details/5
        public async Task<ActionResult> Details(int id, string? search, string? location)
        {
            var res = await _iResume.GetById(id);

            if (res != null)
            {
                if (search != null)
                {
                    res.JobTitle = Regex.Replace(res.JobTitle, search, "<b><u> " + search + "</u></b>", RegexOptions.IgnoreCase);
                    res.Resume = Regex.Replace(res.Resume, search, "<b><u> " + search + "</u></b>", RegexOptions.IgnoreCase);
                }
                
                if (location != null)
                {
                    if (res.jobseeker != null)
                    {
                        res.jobseeker.StreetAddress = Regex.Replace(res.jobseeker.StreetAddress, location, "<b><u> " + location + "</u></b>", RegexOptions.IgnoreCase);
                        res.jobseeker.City = Regex.Replace(res.jobseeker.City, location, "<b><u> " + location + "</u></b>", RegexOptions.IgnoreCase);
                        res.jobseeker.State = Regex.Replace(res.jobseeker.State, location, "<b><u> " + location + "</u></b>", RegexOptions.IgnoreCase);
                        res.jobseeker.Zip = Regex.Replace(res.jobseeker.Zip, location, "<b><u> " + location + "</u></b>", RegexOptions.IgnoreCase);
                        res.jobseeker.Country = Regex.Replace(res.jobseeker.Country, location, "<b><u> " + location + "</u></b>", RegexOptions.IgnoreCase);
                    }
                }
            }

            return View(res);
        }

        // GET: ResumeController/Create
        public async Task<ActionResult> Create(UserM? userM)
        {
            ResumeM j = new ResumeM();
            try
            {

                if (userM != null && userM.UserId > 0)
                {
                    j.user = userM;
                    j.UserId = userM.UserId;
                    ResumeM PostedResume = await _iResume.GetByUserId(userM.UserId);

                    if (PostedResume != null)
                    {
                        j.jobseeker = PostedResume.jobseeker;
                        j.JobSeekerId = PostedResume.JobSeekerId;
                    }
                }
                else if (userM != null && userM.IsInvalidUser == true)
                {
                    ViewBag.InvalidUser = true;
                }

            }
            catch (Exception ex)
            {

            }

            return View(j);
        }
        // GET: ResumeController/Verify
        public ActionResult Verify(ResumeM r)
        {
            return View(r);
        }
        // GET: ResumeController/Confirm 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(ResumeM r)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (r.UserId > 0)
                    {
                        r.user = null;

                    }
                    else
                    {
                        r.user.Type = "Employees";
                        r.user.UrlCode = Guid.NewGuid();
                    }

                    r.ResumeCode = Guid.NewGuid();

                    int resumeId = await _iResume.Create(r);

                    if (resumeId > 0)
                    {
                        var resume = await _iResume.GetById(resumeId);

                        string host = Request.Scheme + "://" + Request.Host.Value;
                        string Url = host + "/Resume/Verified?jcode=" + r.ResumeCode;

                        string Body = "<p> To continue your submission process, please go to the following URL:</p> <br/>" +
                            " <p> " + Url + " </p> <br/>" +
                            "<p> Find out about FREE magazine subscriptions at http://www.getmeajob.com/magazinesBrowse through our extensive list of Business, Computer, Engineering and Trade magazines and kick off 2009! </p> <br/>" +
                            "<p> New!  Get more exposure for your listing on getmeajob.com with our new Front Page Listings feature.  Visit https://www.getmeajob.com/order/default.aspx?sitem=FPJOB&recordid=-15306861 to sign up today!  Full details of Front Page Listings are available at http://www.getmeajob.com/advertise/ </p> <br/> " +
                            "<p> Please do not reply to this email.  Replies to this mailbox may be ignored.  If you need to contact us, please visit http://www.getmeajob.com and click Contact Us. </p> <br/> " +
                            "<p> You have received this email because you have submitted a job or a resume on our web site, http://www.getmeajob.com/.  Submitting a job or a resume does not place you on a mailing list. If this is not what you have intended to do, do not reply to this email and do not visit the URL above. </p> <br/> " +
                            "<p> Security check:  </ p> <br/> " +
                            " <p> IP of submitter: [0000] </p> <br/>";

                        _iEmail.SendEmail(new EmailVM
                        {
                            EmailTo = resume.user.Email,
                            Subject = "ACTION REQUIRED: Please confirm getmeajob.com posting",
                            Body = Body
                        });

                        return View(r);

                    }
                    else
                    {

                    }
                }
            }
            catch
            {
            }
            return RedirectToAction(nameof(Create), r);
        }
        public async Task<ActionResult> Verified(Guid jcode)
        {
            var res = await _iResume.GetByJobCode(jcode);

            if (res != null)
            {
                res.IsEmailVerified = true;
                int changes = await _iResume.Update(res);

                if (changes > 0)
                {
                    ViewBag.success = true;
                }
            }



            return View();
        }
        // POST: ResumeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ResumeM resumeM)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(resumeM.user.FullName) && resumeM.UserId == 0)
                    {
                        //resumeM.user.Type = "Employees";
                        //var usr = await _iUser.GetByEmail(resumeM.user);

                        //if (usr != null)
                        //{
                        //    //View("Create",jobM);
                        //    View();
                        //}

                    }
                    if (!String.IsNullOrEmpty(resumeM.JobTitle) && !String.IsNullOrEmpty(resumeM.Resume))
                    {
                        ViewBag.page = "Verify";
                        View(resumeM);
                    }
                    else
                    {
                        View(resumeM);
                    }
                }
            }
            catch
            {
                //_notyf.Error("Something went wrong!!", 4);
            }

            return View(resumeM);
        }

        // GET: ResumeController/Edit/5
        public async Task<ActionResult> Edit(int rid)
        {
            var resume = await _iResume.GetById(rid);

            return View(resume);
        }

        // POST: ResumeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ResumeM resM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(resM.user.FullName) && resM.UserId == 0)
                    {
                        //var usr = await _iUser.GetByEmail(resM.user);

                        //if (usr != null)
                        //{
                        //    //View("Create",jobM);
                        //    View();
                        //}

                    }
                    if (!String.IsNullOrEmpty(resM.JobTitle) && !String.IsNullOrEmpty(resM.Resume))
                    {
                        //jobM.JobDescription.Replace(Environment.NewLine, "<br/>");
                        //return RedirectToAction("Verify", jobM);
                        ViewBag.page = "Verify";
                        View(resM);
                    }
                    else
                    {
                        View(resM);
                    }
                }
            }
            catch
            {
                //_notyf.Error("Something went wrong!!", 4);
            }

            return View(resM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditConfirm(ResumeM r)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    r.user.UserId = r.UserId;
                    r.jobseeker.JobSeekerId = r.JobSeekerId;

                    r.IsEmailVerified = false;
                    r.IsApproved = false;

                    int changes = await _iResume.Update(r);

                    if (changes > 0)
                    {
                        var res = await _iResume.GetById(r.ResumeId);

                        string host = Request.Scheme + "://" + Request.Host.Value;
                        string Url = host + "/Resume/Verified?jcode=" + res.ResumeCode;

                        string Body = "<p> To continue your submission process, please go to the following URL:</p> <br/>" +
                            " <p> " + Url + " </p> <br/>" +
                            "<p> Find out about FREE magazine subscriptions at http://www.getmeajob.com/magazinesBrowse through our extensive list of Business, Computer, Engineering and Trade magazines and kick off 2009! </p> <br/>" +
                            "<p> New!  Get more exposure for your listing on getmeajob.com with our new Front Page Listings feature.  Visit https://www.getmeajob.com/order/default.aspx?sitem=FPJOB&recordid=-15306861 to sign up today!  Full details of Front Page Listings are available at http://www.getmeajob.com/advertise/ </p> <br/> " +
                            "<p> Please do not reply to this email.  Replies to this mailbox may be ignored.  If you need to contact us, please visit http://www.getmeajob.com and click Contact Us. </p> <br/> " +
                            "<p> You have received this email because you have submitted a job or a resume on our web site, http://www.getmeajob.com/.  Submitting a job or a resume does not place you on a mailing list. If this is not what you have intended to do, do not reply to this email and do not visit the URL above. </p> <br/> " +
                            "<p> Security check:  </ p> <br/> " +
                            " <p> IP of submitter: [0000] </p> <br/>";

                        _iEmail.SendEmail(new EmailVM
                        {
                            EmailTo = res.user.Email,
                            Subject = "ACTION REQUIRED: Please confirm getmeajob.com posting",
                            Body = Body
                        });

                        return View();

                    }
                    else
                    {

                    }
                }
            }
            catch
            {
            }
            return RedirectToAction(nameof(Edit), r);
        }
        public ActionResult Search()
        {
            return View();
        }
        // POST: JobController/SearchResult/
        public async Task<ActionResult> SearchResult(JobSearchVM jobSearch)
        {
            IEnumerable<ResumeM> result = await _iResume.GetByJobTitleOrLocation(jobSearch);
            ViewBag.search = jobSearch.Name;
            ViewBag.location = jobSearch.Location;

            return View(result);
        }


        // GET: ResumeController/Delete/5
        public async Task<ActionResult> Delete(int rid)
        {
            var job = await _iResume.GetById(rid);
            return View(job);
        }

        // POST: ResumeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirm(ResumeM res)
        {
            try
            {

                int changes = await _iResume.Delete(res.ResumeId);

                if (changes > 0)
                {
                    return View();

                }
            }
            catch
            {
            }
            ViewBag.Error = true;
            return View();
        }
    }
}
