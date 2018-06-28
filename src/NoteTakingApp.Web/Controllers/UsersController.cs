using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Web.ViewModels;

namespace NoteTakingApp.Web.Controllers
{
    public class UsersController: Controller
    {
        public UsersController() { }

        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginInputViewModel input)
        {
            return View();
        }
    }
}
