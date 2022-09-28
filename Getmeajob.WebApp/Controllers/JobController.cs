using Getmeajob.Interface;
using Getmeajob.Model;
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
        public ActionResult Index()
        {
            return View();
        }

        // GET: JobController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: JobController/Create
        public ActionResult Create()
        {
            JobM j = new JobM();
            return View(j);
        }
        // GET: JobController/Verify
        public ActionResult Verify(JobM j)
        {
            return View(j);
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
                    if (j.UserId == 0)
                    {
                        int userid = await _iUser.Create(j.user);
                        j.UserId = userid;
                        j.user = null;

                        int companyid = await _iCompany.Create(j.company);

                        j.CompanyId = companyid;
                        j.company = null;

                    }

                    int changes = await _iJob.Create(j);

                    if (changes > 0)
                    {
                        //_notyf.Success("Product Saved Successfully!!", 4);
                        return View(j);

                    }
                    else
                    {
                        //_notyf.Error("Something went wrong!!", 4);
                    }
                }
            }
            catch
            {
                //_notyf.Error("Something went wrong!!", 4);
            }
            return RedirectToAction(nameof(Verify), j);
        }

        // POST: JobController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobM jobM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!String.IsNullOrEmpty(jobM.JobTitle) && !String.IsNullOrEmpty(jobM.JobDescription))
                    {
                        return View("Verify", jobM);
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: JobController/Edit/5
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

        // GET: JobController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: JobController/Delete/5
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
