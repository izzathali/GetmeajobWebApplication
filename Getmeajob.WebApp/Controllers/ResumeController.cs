using Getmeajob.Interface;
using Getmeajob.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Getmeajob.WebApp.Controllers
{
    public class ResumeController : Controller
    {
        private readonly IResume _iResume;
        private readonly IUser _iUser;
        private readonly IJobSeeker _iJobSeeker;
        public ResumeController(IResume iResume, IUser iUser, IJobSeeker iJobSeeker)
        {
            _iResume = iResume;
            _iUser = iUser;
            _iJobSeeker = iJobSeeker;
        }
        // GET: ResumeController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ResumeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ResumeController/Create
        public ActionResult Create()
        {
            return View();
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
                    if (r.UserId == 0)
                    {
                        int userid = await _iUser.Create(r.user);
                        r.UserId = userid;
                        r.user = null;

                        int companyid = await _iJobSeeker.Create(r.jobseeker);

                        r.JobSeekerId = companyid;
                        r.jobseeker = null;

                    }

                    int changes = await _iResume.Create(r);

                    if (changes > 0)
                    {
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
            return RedirectToAction(nameof(Verify), r);
        }

        // POST: ResumeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ResumeM resumeM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!String.IsNullOrEmpty(resumeM.JobTitle) && !String.IsNullOrEmpty(resumeM.Resume))
                    {
                        return View("Verify", resumeM);
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ResumeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ResumeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ResumeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
