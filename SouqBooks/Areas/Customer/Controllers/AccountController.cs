using Microsoft.AspNetCore.Mvc;

namespace SouqBooks.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
    }
}
