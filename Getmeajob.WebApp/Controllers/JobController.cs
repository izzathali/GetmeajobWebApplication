using Getmeajob.Interface;
using Getmeajob.Model;
using Getmeajob.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Getmeajob.WebApp.Controllers
{
    public class JobController : Controller
    {
        private readonly IJob _iJob;
        private readonly IUser _iUser;
        private readonly ICompany _iCompany;
        public JobController(IJob iJob, IUser iUser, ICompany iCompany)
        {
            _iJob = iJob;
            _iUser = iUser;
            _iCompany = iCompany;
        }
        // GET: JobController
        public async Task<ActionResult> Index(int uid,string page)
        {
            ViewBag.page = page;
            var jobs = await _iJob.GetAllByUserId(uid);
            return View(jobs);
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
            return View(result);
        }
        // GET: JobController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var job = await _iJob.GetById(id);
            return View(job);
        }

        // GET: JobController/Create
        public async Task<ActionResult> Create(UserM? userM)
        {
            JobM j = new JobM();
            try
            {

                if (userM != null && userM.UserId > 0)
                {
                    j.user = userM;
                    j.UserId = userM.UserId;
                    JobM Postedjob = await _iJob.GetByUserId(userM.UserId);
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
                    }
                    int changes = await _iJob.Create(j);

                    if (changes > 0)
                    {
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

                    int changes = await _iJob.Update(j);

                    if (changes > 0)
                    {
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
                    if (!string.IsNullOrEmpty(jobM.user.FullName) && jobM.UserId == 0 )
                    {
                        jobM.user.Type = "Employers";
                        var usr = await _iUser.GetByEmail(jobM.user);

                        if (usr != null)
                        {
                            //View("Create",jobM);
                            View();
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
                        var usr = await _iUser.GetByEmail(job.user);

                        if (usr != null)
                        {
                            //View("Create",jobM);
                            View();
                        }

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
