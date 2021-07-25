using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VivedyWebApp.Models;
using Microsoft.AspNet.Identity;
using VivedyWebApp.Models.ViewModels;

namespace VivedyWebApp.Controllers
{
    /// <summary>
    /// Application Movies Controller
    /// </summary>
    public class MoviesController : Controller
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
            List<Movie> movies = await db.Movies.ToListAsync();
            if(movies == null)
            {
                return View(movies);
            }
            //Getting lists of categories and ratings for the filters on the movies page
            ViewBag.Categories = new List<string>();
            ViewBag.Ratings = new List<string>();
            foreach (Movie movie in movies) {
                if (ViewBag.Categories.Contains(movie.Category))
                {
                    continue;
                }
                else
                {
                    ViewBag.Categories.Add(movie.Category);
                }
                if (ViewBag.Ratings.Contains("+" + movie.Rating))
                {
                    continue;
                }
                else
                {
                    ViewBag.Ratings.Add("+" + movie.Rating);
                }
            }
            ViewBag.Categories.Sort();
            ViewBag.Ratings.Sort();

            return View(movies);
        }

        /// <summary>
        /// GET request action for Details page
        /// </summary>
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = await db.Movies.FindAsync(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        /// <summary>
        /// GET request action for all Movies data for public api
        /// </summary>
        [AllowAnonymous]
        public JsonResult All()
        {
            return Json(db.Movies.ToList(), JsonRequestBehavior.AllowGet);
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
