using System.Diagnostics;
using EAPD7111_Part2.Models;
using Microsoft.AspNetCore.Mvc;

namespace EAPD7111_Part2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // ? Landing page
        public IActionResult Index()
        {
            return View();
        }

        // ? Optional privacy page (remove if not needed)
        public IActionResult Privacy()
        {
            return View();
        }

        // ? Error handling
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
