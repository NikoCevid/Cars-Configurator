using Microsoft.AspNetCore.Mvc;

namespace Cars_MVC.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
       
    }
}
