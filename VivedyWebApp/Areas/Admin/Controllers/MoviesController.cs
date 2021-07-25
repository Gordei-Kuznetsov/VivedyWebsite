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
    public class MoviesController : Controller
    {
        /// <summary>
        /// ApplicationDbContext instance
        /// </summary>
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// GET request action for Index page
        /// </summary>
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Movies.ToListAsync());
        }

        /// <summary>
        /// GET request action for Details page
        /// </summary>
        [Authorize(Roles = "Admin")]
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
        /// GET request action for Create page
        /// </summary>
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST request action for Create page
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(MoviesCreateViewModel newMovie)
        {
            if (ModelState.IsValid)
            {
                Movie movie = new Movie
                {
                    MovieId = Guid.NewGuid().ToString(),
                    Name = newMovie.Name,
                    Rating = newMovie.Rating,
                    UserRating = newMovie.UserRating,
                    Category = newMovie.Category,
                    Description = newMovie.Description,
                    Duration = newMovie.Duration,
                    Price = newMovie.Price,
                    TrailerUrl = newMovie.TrailerUrl
                };
                db.Movies.Add(movie);
                await db.SaveChangesAsync();
                //Saving the images uploaded for the posters
                if(newMovie.HorizontalImage != null)
                {
                    //Saving horizontal poster
                    string fileName = movie.MovieId + "-HorizontalPoster.png";
                    var imagePath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    newMovie.HorizontalImage.SaveAs(imagePath);
                }
                if(newMovie.VerticalImage != null)
                {
                    //Saving vertical poster
                    string fileName = movie.MovieId + "-VerticalPoster.png";
                    var imagePath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    newMovie.VerticalImage.SaveAs(imagePath);
                }
                return RedirectToAction("Index");
            }

            return View(newMovie);
        }

        /// <summary>
        /// GET request action for Edit page
        /// </summary>
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(string id)
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
            
            MoviesViewModel model = new MoviesViewModel
            {
                MovieId = movie.MovieId,
                Name = movie.Name,
                Rating = movie.Rating,
                UserRating = movie.UserRating,
                Category = movie.Category,
                Description = movie.Description,
                Duration = movie.Duration,
                Price = movie.Price,
                TrailerUrl = movie.TrailerUrl,
                //Not sending the name of the saved poster images yet
                //Will be added later
            };
            return View(model);
        }

        /// <summary>
        /// POST request action for Edit page
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(MoviesViewModel model)
        {
            if (ModelState.IsValid)
            {
                Movie movie = new Movie
                {
                    MovieId = model.MovieId,
                    Name = model.Name,
                    Rating = model.Rating,
                    UserRating = model.UserRating,
                    Category = model.Category,
                    Description = model.Description,
                    Duration = model.Duration,
                    Price = model.Price,
                    TrailerUrl = model.TrailerUrl
                };
                db.Entry(movie).State = EntityState.Modified;
                await db.SaveChangesAsync();

                //Saving the images for the posters if re-uploaded 
                if (model.HorizontalImage != null)
                {
                    //Deleting existing one
                    string path = Server.MapPath("/Content/Images/" + model.MovieId + "-HorizontalPoster.png");
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
                    string path = Server.MapPath("/Content/Images/" + model.MovieId + "-VerticalPoster.png");
                    FileInfo fi = new FileInfo(path);
                    if (fi.Exists)
                    {
                        fi.Delete();
                    }
                    //Saving vertical poster
                    model.VerticalImage.SaveAs(path);
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

        /// <summary>
        /// GET request action for Delete page
        /// </summary>
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
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
        /// POST request action for DeleteConfirmed page
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Movie movie = await db.Movies.FindAsync(id);
            db.Movies.Remove(movie);
            await db.SaveChangesAsync();
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
            return RedirectToAction("Index");
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
