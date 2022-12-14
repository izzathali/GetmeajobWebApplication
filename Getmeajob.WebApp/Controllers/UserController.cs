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
                    memoryCache.Set("LoggedUser", usr);

                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    _notifyService.Error("Invalid Email or Password");
                }

            }
            return RedirectToAction("Admin");
        }
        public ActionResult AdminLogout()
        {
            UserM user = new UserM();
            memoryCache.Set("LoggedUser", user);

            return RedirectToAction(nameof(AdminLogin));
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

            IEnumerable<UserM> usr_all = await _iUser.GetAllByEmailAndPass(userM);
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
                    if (usr_all.Count() > 0)
                    {
                        int[] uids = new int[usr_all.Count()];

                        int n = 0;
                        foreach (var u in usr_all)
                        {
                            uids[n] = u.UserId;
                            n++;
                        }

                        return RedirectToAction("Index", "Job", new { uid = uids, page = userM.page});
                    }

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
                    if (usr_all.Count() > 0)
                    {
                        int[] uids = new int[usr_all.Count()];

                        int n = 0;
                        foreach (var u in usr_all)
                        {
                            uids[n] = u.UserId;
                            n++;
                        }

                        return RedirectToAction("Index", "Resume", new { uid = uids, page = userM.page });
                    }
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
        public async Task<ActionResult> DeleteAll(Guid[] code)
        {
            List<UserM> users = new List<UserM>();
            for (int i = 0; i < code.Length; i++)
            {
                var usr = await _iUser.GetByCode(code[i]);
                users.Add(usr);

            }
            if (users == null)
            {
                ViewBag.Error = true;
            }

            return View(users);
        }
        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAllConfirm(List<UserM> users)
        {
            if (users == null)
            {
                    ViewBag.Error = true;
                    return View();
            }

            try
            {
                foreach (var u in users)
                {
                    int jobs = await _iJob.DeleteAllByUserId(u.UserId);
                    int resumes = await _iJob.DeleteAllByUserId(u.UserId);

                    if (jobs == 0 && resumes == 0)
                    {
                        ViewBag.Error = true;
                    }
                }


            }
            catch
            {
            }
            return View();
        }
    }
}
