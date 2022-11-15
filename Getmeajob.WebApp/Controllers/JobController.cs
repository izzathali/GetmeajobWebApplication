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
    public class JobController : Controller
    {
        private readonly IJob _iJob;
        private readonly IUser _iUser;
        private readonly ICompany _iCompany;
        private readonly IEmail _iEmail;
        IMemoryCache memoryCache;
        private UserM Usr = new UserM();

        public INotyfService _notifyService { get; }

        public JobController(IJob iJob, IUser iUser, ICompany iCompany, IEmail iEmail, IMemoryCache memoryCache, INotyfService notyfService)
        {
            _iJob = iJob;
            _iUser = iUser;
            _iCompany = iCompany;
            _iEmail = iEmail;
            this.memoryCache = memoryCache;
            _notifyService = notyfService;

            Usr = memoryCache.Get("LoggedUser") as UserM;
        }
        // GET: JobController

        public async Task<ActionResult> Index(int[] uid, string page)
        {
            ViewBag.page = page;
            List<JobM> jobs = new List<JobM>();

            for (int i = 0; i < uid.Count(); i++)
            {
                IList<JobM> jb = await _iJob.GetAllByUid(uid[i]);
                jobs.AddRange(jb);
            }
            return View(jobs);
        }
        public async Task<ActionResult> Unapproved()
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Admin", "User");
            }

            return View(await _iJob.GetAllUnapproved());
        }

        public async Task<ActionResult> approve(int jid)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Admin", "User");
            }

            if (jid > 0)
            {
                JobM job = await _iJob.GetById(jid);

                if (job != null)
                {
                    if (job.IsEmailVerified == true)
                    {

                        job.IsApproved = true;
                        int changes = await _iJob.Update(job);
                        if (changes > 0)
                        {
                            _notifyService.Success(job.JobTitle + " Job Approved ");
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

        // GET: JobController/Search
        public ActionResult Search()
        {
            return View();
        }
        // POST: JobController/SearchResult/
        public async Task<ActionResult> SearchResult(JobSearchVM jobSearch)
        {
            var result = await _iJob.GetByJobTitleOrLocation(jobSearch);
            ViewBag.search = jobSearch.Name;
            ViewBag.location = jobSearch.Location;

            return View(result);
        }
        public async Task<ActionResult> QuickSearchResult(string search)
        {
            JobSearchVM se = new JobSearchVM();
            se.Name = search;

            return RedirectToAction(nameof(SearchResult), se);
        }
        // GET: JobController/Details/5
        public async Task<ActionResult> Details(int id, string? search, string? location)
        {
            var job = await _iJob.GetById(id);

            if (job != null)
            {
                if (search != null)
                {
                    job.JobTitle = Regex.Replace(job.JobTitle, search, "<b><u> " + search + "</u></b>", RegexOptions.IgnoreCase);
                    job.JobDescription = Regex.Replace(job.JobDescription, search, "<b><u> " + search + "</u></b>", RegexOptions.IgnoreCase);

                }
                if (location != null)
                {
                    if (job.company != null)
                    {

                        job.company.StreetAddress = Regex.Replace(job.company.StreetAddress, location, "<b><u> " + location + "</u></b>", RegexOptions.IgnoreCase);
                        job.company.City = Regex.Replace(job.company.City, location, "<b><u> " + location + "</u></b>", RegexOptions.IgnoreCase);
                        job.company.State = Regex.Replace(job.company.State, location, "<b><u> " + location + "</u></b>", RegexOptions.IgnoreCase);
                        job.company.Zip = Regex.Replace(job.company.Zip, location, "<b><u> " + location + "</u></b>", RegexOptions.IgnoreCase);
                        job.company.Country = Regex.Replace(job.company.Country, location, "<b><u> " + location + "</u></b>", RegexOptions.IgnoreCase);
                    }

                }
            }

            return View(job);
        }
        // GET: JobController/Create
        [HttpGet]
        public async Task<ActionResult> Create(UserM? userM)
        {
            JobM j = new JobM();
            try
            {

                if (userM != null && userM.UserId > 0)
                {
                    j.user = userM;
                    j.UserId = userM.UserId;
                    JobM Postedjob = await _iJob.GetUnapprovedByUserId(userM.UserId);
                    if (Postedjob != null)
                    {
                        j.company = Postedjob.company;
                        j.CompanyId = Postedjob.CompanyId;
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
        // GET: JobController/Verify
        public async Task<ActionResult> Verify(JobM jobM)
        {
            //j.user = await _iUser.GetById(j.UserId);
            //j.company = await _iCompany.GetById(j.CompanyId);
            return View(jobM);
        }
        // GET: JobController/Confirm 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(JobM j)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (j.UserId > 0)
                    {
                        j.user = null;

                    }
                    else
                    {
                        j.user.Type = "Employers";
                        j.user.UrlCode = Guid.NewGuid();
                    }

                    j.JobCode = Guid.NewGuid();

                    int jobid = await _iJob.Create(j);

                    if (jobid > 0)
                    {
                        var job = await _iJob.GetById(jobid);

                        string host = Request.Scheme + "://" + Request.Host.Value;
                        string Url = host + "/Job/Verified?jcode=" + j.JobCode;

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
                            EmailTo = job.user.Email,
                            Subject = "ACTION REQUIRED: Please confirm getmeajob.com posting",
                            Body = Body
                        });
                        return View(j);

                    }
                    else
                    {

                    }
                }
            }
            catch
            {
            }
            return RedirectToAction(nameof(Create), j);
        }
        public async Task<ActionResult> Verified(Guid jcode)
        {
            var job = await _iJob.GetByJobCode(jcode);

            if (job != null)
            {
                job.IsEmailVerified = true;
                int changes = await _iJob.Update(job);

                if (changes > 0)
                {
                    ViewBag.success = true;
                }
            }



            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditConfirm(JobM j)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    j.user.UserId = j.UserId;
                    j.company.CompanyId = j.CompanyId;

                    j.IsEmailVerified = false;
                    j.IsApproved = false;
                    int changes = await _iJob.Update(j);

                    if (changes > 0)
                    {
                        var job = await _iJob.GetById(j.JobId);

                        string host = Request.Scheme + "://" + Request.Host.Value;
                        string Url = host + "/Job/Verified?jcode=" + job.JobCode;

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
                            EmailTo = job.user.Email,
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
            return RedirectToAction(nameof(Edit), j);
        }
        // POST: JobController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JobM jobM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(jobM.user.FullName) && jobM.UserId == 0)
                    {
                        jobM.user.Type = "Employers";
                        //var usr = await _iUser.GetByEmail(jobM.user);

                        //if (usr != null)
                        //{
                        //    //View("Create",jobM);
                        //    View();
                        //}

                    }
                    if (jobM.user != null && jobM.user.UserId > 0)
                    {
                        jobM.user = jobM.user;
                        jobM.UserId = jobM.user.UserId;
                        JobM Postedjob = await _iJob.GetUnapprovedByUserId(jobM.user.UserId);
                        if (Postedjob != null)
                        {
                            jobM.company = Postedjob.company;
                            jobM.CompanyId = Postedjob.CompanyId;
                        }
                    }

                    if (!String.IsNullOrEmpty(jobM.JobTitle) && !String.IsNullOrEmpty(jobM.JobDescription))
                    {
                        //jobM.JobDescription.Replace(Environment.NewLine, "<br/>");
                        //return RedirectToAction("Verify", jobM);
                        ViewBag.page = "Verify";
                        View(jobM);
                    }
                    else
                    {
                        View(jobM);
                    }
                }
            }
            catch
            {
                //_notyf.Error("Something went wrong!!", 4);
            }

            return View(jobM);
        }


        // GET: JobController/Edit/5
        public async Task<ActionResult> Edit(int jid)
        {
            var job = await _iJob.GetById(jid);

            return View(job);
        }

        // POST: JobController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(JobM job)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(job.user.FullName) && job.UserId == 0)
                    {
                        //var usr = await _iUser.GetByEmail(job.user);

                        //if (usr != null)
                        //{
                        //    View("Create",jobM);
                        //    View();
                        //}

                    }
                    if (!String.IsNullOrEmpty(job.JobTitle) && !String.IsNullOrEmpty(job.JobDescription))
                    {
                        //jobM.JobDescription.Replace(Environment.NewLine, "<br/>");
                        //return RedirectToAction("Verify", jobM);
                        ViewBag.page = "Verify";
                        View(job);
                    }
                    else
                    {
                        View(job);
                    }
                }
            }
            catch
            {
                //_notyf.Error("Something went wrong!!", 4);
            }

            return View(job);
        }

        // GET: JobController/Delete/5
        public async Task<ActionResult> Delete(int jid)
        {
            var job = await _iJob.GetById(jid);
            return View(job);
        }

        // POST: JobController/Delete/5
        public async Task<ActionResult> DeleteConfirm(int jid)
        {
            try
            {

                int changes = await _iJob.Delete(jid);

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
