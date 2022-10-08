using Microsoft.AspNetCore.Mvc;

namespace Getmeajob.WebApp.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
