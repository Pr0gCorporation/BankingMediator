using Microsoft.AspNetCore.Mvc;

namespace Internship.TransactionService.API.Controllers
{
    public class TransactionController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}