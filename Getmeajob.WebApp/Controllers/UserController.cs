using Getmeajob.Interface;
using Getmeajob.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Getmeajob.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUser _iUser;
        private readonly IJob _iJob;
        public UserController(IUser iUser, IJob iJob)
        {
            _iUser = iUser;
            _iJob = iJob;
        }
        // GET: UserController
        public ActionResult Index()
        {
            return View();
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

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
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
