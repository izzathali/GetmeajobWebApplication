using AspNetCoreHero.ToastNotification.Abstractions;
using Getmeajob.Interface;
using Getmeajob.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Getmeajob.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUser _iUser;
        private readonly IJob _iJob;
        IMemoryCache memoryCache;
        public INotyfService _notifyService { get; }


        public UserController(IUser iUser, IJob iJob, IMemoryCache memoryCache, INotyfService notifyService)
        {
            _iUser = iUser;
            _iJob = iJob;
            this.memoryCache = memoryCache;
            _notifyService = notifyService;

        }
        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Admin()
        {
            return View();
        }
        public async Task<ActionResult> AdminLogin(UserM u)
        {
            if (!string.IsNullOrEmpty(u.Email) && !string.IsNullOrEmpty(u.Password))
            {
                u.Type = "admin";
                UserM usr = await _iUser.GetByEmailAndPass(u);

                if (usr != null)
                {
                    memoryCache.Set("LoggedUser", u);

                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    _notifyService.Error("Invalid Email or Password");
                }

            }
            return RedirectToAction("Admin");
        }
        public ActionResult Login(string? stype)
        {
            UserM user = new UserM();
            user.page = stype;

            return View(user);
        }
        // GET: UserController/Invalid
        public ActionResult Invalid(UserM? user)
        {
            return View(user);
        }
        // POST: UserController/Login
        [HttpPost]
        public async Task<ActionResult> LoginAuth(UserM userM)
        {
            if (userM == null) return View();

            if (userM.page == "submitjob" || userM.page == "modifyjob" || userM.page == "deletejob")
            {
                userM.Type = "Employers";
            }
            if (userM.page == "submitresume" || userM.page == "modifyresume" || userM.page == "deleteresume")
            {
                userM.Type = "Employees";
            }

            UserM usr = await _iUser.GetByEmailAndPass(userM);

            if (userM.page == "submitjob")
            {

                if (usr == null)
                {
                    UserM u = new UserM();
                    u.IsInvalidUser = true;

                    return RedirectToAction("Create", "Job", u);
                }
                else
                {
                    return RedirectToAction("Create", "Job", usr);
                }
            }
            else if (userM.page == "modifyjob" || userM.page == "deletejob")
            {
                if (usr != null)
                {
                    return RedirectToAction("Index", "Job", new {uid = usr.UserId,page = userM.page});
                }
            }
            else if (userM.page == "submitresume")
            {

                if (usr == null)
                {
                    UserM u = new UserM();
                    u.IsInvalidUser = true;

                    return RedirectToAction("Create", "Resume", u);
                }
                else
                {
                    return RedirectToAction("Create", "Resume", usr);
                }
            }
            else if (userM.page == "modifyresume" || userM.page == "deleteresume")
            {
                if (usr != null)
                {
                    return RedirectToAction("Index", "Resume", new { uid = usr.UserId, page = userM.page });
                }
            }

            return RedirectToAction("Invalid",userM);

        }
        
        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
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
        // GET: UserController/Delete/
        public async Task<ActionResult> DeleteAll(Guid code)
        {
            var usr = await _iUser.GetByCode(code);
            if (usr == null)
            {
                ViewBag.Error = true;
            }

            return View(usr);
        }
        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAllConfirm(UserM userM)
        {
            if (userM == null)
            {
                    ViewBag.Error = true;
                    return View();
            }

            try
            {
                int jobs = await _iJob.DeleteAllByUserId(userM.UserId);
                int resumes = await _iJob.DeleteAllByUserId(userM.UserId);

                if (jobs == 0 && resumes == 0)
                {
                    ViewBag.Error = true;
                }

            }
            catch
            {
            }
            return View();
        }
    }
}
