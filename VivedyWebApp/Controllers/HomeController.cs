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
using VivedyWebApp.Models;
using VivedyWebApp.Models.ViewModels;

namespace VivedyWebApp.Controllers
{
    /// <summary>
    /// Application Home Controller
    /// </summary>
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public HomeController()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Movies = new MoviesManager(db);
        }

        private readonly MoviesManager Movies;

        /// <summary>
        /// GET request action for Index page
        /// </summary>
        public async Task<ActionResult> Index(string message = null)
        {
            ViewBag.Message = message;
            MoviesHomeViewModel model = new MoviesHomeViewModel()
            {
                ComingSoonMovies = await Movies.AllCommingAsync(),
                TopMovies = await Movies.TopShowingAsync(4)
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