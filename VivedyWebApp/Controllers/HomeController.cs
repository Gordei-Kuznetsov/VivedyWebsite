using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VivedyWebApp.Models.ViewModels;

namespace VivedyWebApp.Controllers
{
    /// <summary>
    /// Application Home Controller
    /// </summary>
    [AllowAnonymous]
    public class HomeController : Controller
    {
        /// <summary>
        /// The entities manager instance
        /// </summary>
        private readonly Entities Helper = new Entities();

        /// <summary>
        /// GET request action for Index page
        /// </summary>
        public async Task<ActionResult> Index(string message = null)
        {
            ViewBag.Message = message;
            MoviesHomeViewModel model = new MoviesHomeViewModel()
            {
                ComingSoonMovies = await Helper.Movies.GetAllComming(),
                TopMovies = await Helper.Movies.GetTopShowing(4)
            };
            return View(model);
        }

        /// <summary>
        /// GET request action for About page
        /// </summary>
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// GET request action for Privacy page
        /// </summary>
        public ActionResult Privacy()
        {
            return View();
        }
    }
}