using Getmeajob.Interface;
using Getmeajob.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Getmeajob.WebApp.Controllers
{
    public class JobController : Controller
    {
        private readonly IJob _iJob;
        public JobController(IJob iJob)
        {
            _iJob = iJob;
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
        public async Task<ActionResult> Confirm(JobM j)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int changes = await _iJob.Create(j);

                    if (changes > 0)
                    {
                        //_notyf.Success("Product Saved Successfully!!", 4);
                        return RedirectToAction(nameof(Index));
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
            return View(j);
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
                    return Verify(jobM);
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
