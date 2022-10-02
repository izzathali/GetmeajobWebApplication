using Getmeajob.Interface;
using Getmeajob.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Getmeajob.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUser _iUser;
        public UserController(IUser iUser)
        {
            _iUser = iUser;
        }
        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(string? stype)
        {
            ViewBag.stype = stype;
            return View();
        }
        // POST: UserController/Login
        [HttpPost]
        public async Task<ActionResult> LoginAuth(UserM userM)
        {
            UserM usr = await _iUser.GetByEmailAndPass(userM);

            if (usr == null)
            {
                UserM u = new UserM();
                u.IsInvalidUser = true;

                return RedirectToAction("Create","Job",u);
            }
            else
            {
                return RedirectToAction("Create", "Job",usr);
            }
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
