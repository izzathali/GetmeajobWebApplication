using Getmeajob.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Getmeajob.WebApp.Controllers
{
    public class DashboardController : Controller
    {
        IMemoryCache memoryCache;
        private UserM Usr = new UserM();

        public DashboardController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
            Usr = memoryCache.Get("LoggedUser") as UserM;
        }
        public IActionResult Index()
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Admin", "User");
            }

            return View();
        }
    }
}
