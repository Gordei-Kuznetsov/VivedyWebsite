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
        /// <summary>
        /// The entities manager instance
        /// </summary>
        private readonly Entities Helper = new Entities();

        /// <summary>
        /// GET request action for Index page
        /// </summary>
        public ActionResult Index()
        {
            List<Movie> movies = Helper.Movies.GetAllReleased().OrderByDescending(m => m.ViewerRating).ToList();
            ViewBag.Categories = Helper.Movies.GetAllCategories();
            ViewBag.Ratings = Helper.Movies.GetAllRatingss();
            return View(movies);
        }

        /// <summary>
        /// GET request action for Details page
        /// </summary>
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Movie movie = Helper.Movies.Details(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            MoviesDetailsViewModel model = new MoviesDetailsViewModel()
            {
                Movie = movie,
                Cinemas = Helper.GetCinemasForMovie(id)
            };
            return View(model);
        }

        /// <summary>
        /// GET request action for all Movies data for public api
        /// </summary>
        [AllowAnonymous]
        public JsonResult All()
        {
            return Json(Helper.Movies.AllToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
