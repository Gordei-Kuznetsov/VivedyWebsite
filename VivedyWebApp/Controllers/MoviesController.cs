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
    [AllowAnonymous]
    public class MoviesController : Controller
    {
        public MoviesController()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Movies = new MoviesManager(db);
            Cinemas = new CinemasManager(db);
        }

        private readonly MoviesManager Movies;
        private readonly CinemasManager Cinemas;

        /// <summary>
        /// GET request action for Index page
        /// </summary>
        public async Task<ActionResult> Index()
        {
            MoviesViewModel model = new MoviesViewModel()
            {
                Movies = (await Movies.AllNotClosedAsync()).OrderByDescending(m => m.ViewerRating).ToList(),
                Categories = await Movies.CategoriesSelectListItemsAsync(),
                Ratings = await Movies.RatingsSelectListItemsAsync()
            };
            return View(model);
        }

        /// <summary>
        /// GET request action for Details page
        /// </summary>
        public async Task<ActionResult> Details(string id, string message = null)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = await Movies.DetailsAsync(id);
            if (movie == null || movie.ClosingDate < DateTime.Now)
            {
                return HttpNotFound();
            }
            MoviesDetailsViewModel model = new MoviesDetailsViewModel()
            {
                Movie = movie,
                Cinemas = await Cinemas.CinemasForMovieAsync(id)
            };
            ViewBag.Message = message;
            return View(model);
        }

        /// <summary>
        /// GET request action for all Movies data for public api
        /// </summary>
        [AllowAnonymous]
        public async Task<JsonResult> All()
        {
            return Json(await Movies.AllAsync(), JsonRequestBehavior.AllowGet);
        }
    }
}
