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
using VivedyWebApp.Models.ViewModels;
using System.IO;
using VivedyWebApp.Areas.Admin.Models.ViewModels;

namespace VivedyWebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Application Admin Controller for Movies
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class MoviesController : Controller
    {
        public MoviesController()
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
            return View(await Movies.AllToList());
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
            Movie movie = await Movies.Details(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        /// <summary>
        /// GET request action for Create page
        /// </summary>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST request action for Create page
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MoviesCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = Messages.Error;
                return View(model);
            }
            Movie movie = new Movie()
            {
                Name = model.Name,
                Rating = model.Rating,
                ViewerRating = model.ViewerRating,
                Category = model.Category,
                Description = model.Description,
                Duration = model.Duration,
                Price = model.Price,
                TrailerUrl = model.TrailerUrl,
                ReleaseDate = model.ReleaseDate,
                ClosingDate = model.ClosingDate
            };
            if(movie.ReleaseDate >= movie.ClosingDate)
            {
                ViewBag.Message = Messages.WrongMovieDates;
                return View(model);
            }
            var result = await Movies.Create(movie);
            if(result == null)
            {
                ViewBag.Message = Messages.Movies.CreateFailed;
                return View(model);
            }
            //Saving the images uploaded for the posters
            if(model.HorizontalImage != null)
            {
                //Saving horizontal poster
                string fileName = movie.Id + "-HorizontalPoster.png";
                var imagePath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                model.HorizontalImage.SaveAs(imagePath);
            }
            if(model.VerticalImage != null)
            {
                //Saving vertical poster
                string fileName = movie.Id + "-VerticalPoster.png";
                var imagePath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                model.VerticalImage.SaveAs(imagePath);
            }
            return RedirectToAction("Index", new { message = Messages.Movies.Created });
        }

        /// <summary>
        /// GET request action for Edit page
        /// </summary>
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = await Movies.Details(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            MoviesAdminViewModel model = new MoviesAdminViewModel
            {
                Name = movie.Name,
                Rating = movie.Rating,
                ViewerRating = movie.ViewerRating,
                Category = movie.Category,
                Description = movie.Description,
                Duration = movie.Duration,
                Price = movie.Price,
                TrailerUrl = movie.TrailerUrl,
                ReleaseDate = movie.ReleaseDate,
                ClosingDate = movie.ClosingDate
            };
            return View(model);
        }

        /// <summary>
        /// POST request action for Edit page
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MoviesAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = Messages.Error;
                return View(model);
            }
            Movie movie = await Movies.Details(model.Id);
            if(movie == null)
            {
                ViewBag.Message = Messages.Error;
                return View(model);
            }
            //Possibly requires a check on all screenings
            //As change in start date, close date, or duration can screw up timing
            //And possibly sending emails to everyone who made bookings for the affected screenings
            movie.Name = model.Name;
            movie.Rating = model.Rating;
            movie.ViewerRating = model.ViewerRating;
            movie.Category = model.Category;
            movie.Description = model.Description;
            movie.Duration = model.Duration;
            movie.Price = model.Price;
            movie.TrailerUrl = model.TrailerUrl;
            movie.ReleaseDate = model.ReleaseDate;
            movie.ClosingDate = model.ClosingDate;

            if (movie.ReleaseDate >= movie.ClosingDate)
            {
                ViewBag.Message = Messages.WrongMovieDates;
                return View(model);
            }

            var result = await Movies.Edit(movie);
            if(result == null)
            {
                ViewBag.Message = Messages.Movies.EditFailed;
                return View(model);
            }
            //Saving the images for the posters if re-uploaded 
            if (model.HorizontalImage != null)
            {
                //Deleting existing one
                string path = Server.MapPath("/Content/Images/" + movie.Id + "-HorizontalPoster.png");
                FileInfo fi = new FileInfo(path);
                if (fi.Exists)
                {
                    fi.Delete();
                }
                //Saving horizontal poster
                model.HorizontalImage.SaveAs(path);
            }
            if (model.VerticalImage != null)
            {
                //Deleting existing one
                string path = Server.MapPath("/Content/Images/" + movie.Id + "-VerticalPoster.png");
                FileInfo fi = new FileInfo(path);
                if (fi.Exists)
                {
                    fi.Delete();
                }
                //Saving vertical poster
                model.VerticalImage.SaveAs(path);
            }
            return RedirectToAction("Index", new { message = Messages.Movies.Edited });
        }

        /// <summary>
        /// GET request action for Delete page
        /// </summary>
        public async Task<ActionResult> Delete(string id, string message = null)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = await Movies.Details(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            ViewBag.Message = message;
            return View(movie);
        }

        /// <summary>
        /// POST request action for DeleteConfirmed page
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = await Movies.Details(id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            int result = await Movies.Delete(movie);
            if(result <= 0)
            {
                return View("Delete", "Movies", new { id = id, message = Messages.Movies.DeleteFailed });
            }
            //Deleting poster images
            //Deleting horizontal poster
            string path = Server.MapPath("/Content/Images/" + id + "-HorizontalPoster.png");
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            //Deleting vertical poster
            path = Server.MapPath("/Content/Images/" + id + "-VerticalPoster.png");
            fi = new FileInfo(path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            return RedirectToAction("Index", new { message =  Messages.Movies.Deleted });
        }

        /// <summary>
        /// GET request action for DeleteAllClosed page
        /// </summary>
        public async Task<ActionResult> DeleteAllClosed(string message = null)
        {
            
            List<Movie> movies = await Movies.AllOld();
            if(movies.Count == 0)
            {
                return RedirectToAction("Index", new { message = Messages.NoClosedMovies });
            }
            ViewBag.Message = message;
            return View(movies);
        }

        /// <summary>
        /// POST request action for DeleteAllCLosedConfirmed page
        /// </summary>
        [HttpPost, ActionName("DeleteAllClosed")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAllCLosedConfirmed()
        {
            int result = await Movies.DeleteAllOld();
            if (result <= 0)
            {
                return View("DeleteAllClosed", "Movies", new {message = Messages.ClosedMoviesFailedDelete });
            }
            List<Movie> movies = await Movies.AllOld();
            foreach(Movie movie in movies)
            {
                //Deleting poster images
                //Deleting horizontal poster
                string path = Server.MapPath("/Content/Images/" + movie.Id + "-HorizontalPoster.png");
                FileInfo fi = new FileInfo(path);
                if (fi.Exists)
                {
                    fi.Delete();
                }
                //Deleting vertical poster
                path = Server.MapPath("/Content/Images/" + movie.Id + "-VerticalPoster.png");
                fi = new FileInfo(path);
                if (fi.Exists)
                {
                    fi.Delete();
                }
            }
            return RedirectToAction("Index", new { message = Messages.ClosedMoviesDeleted });
        }
    }
    
    public partial class Messages
    {
        public static string WrongMovieDates = "The release date cannot be after the closing date.";
        public static string NoClosedMovies = "There are no closed movies at the moment.";
        public static string ClosedMoviesFailedDelete = "Failed to delete closed movie.\nPlease try again.";
        public static string ClosedMoviesDeleted = "All closed movies deleted.";
    }
}
