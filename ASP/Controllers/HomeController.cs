using Data.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASP.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Loads the Index View
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Loads the About View
        /// </summary>
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Loads the Contact View
        /// </summary>
        public IActionResult Contact()
        {
            return View();
        }

        /// <summary>
        /// Loads the Error View
        /// </summary>
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}