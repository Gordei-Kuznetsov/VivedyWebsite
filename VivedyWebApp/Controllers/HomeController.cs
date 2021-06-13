using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VivedyWebApp.Models;

namespace VivedyWebApp.Controllers
{
    /// <summary>
    /// Application Home Controller
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// ApplicationDbContext instance
        /// </summary>
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// GET request action for Index page
        /// </summary>
        public async Task<ActionResult> Index()
        {
            //Sending only top 4 movies to the home page
            var movies = await db.Movies.ToListAsync();
            return View(movies.Take(4));
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
        /// <summary>
        /// Method for disposing ApplicationDbContext objects
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}