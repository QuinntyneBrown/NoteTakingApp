using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Web.ViewModels;

namespace NoteTakingApp.Web.Controllers
{
    [ApiController]
    public class UsersController: Controller
    {
        public ViewResult Login() {
            return View();
        }

        [HttpPost]
        public ViewResult Login(LoginInputViewModel inputViewModel)
        {
            return View();
        }
    }
}
