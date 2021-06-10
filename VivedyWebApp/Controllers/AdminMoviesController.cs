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

namespace VivedyWebApp.Controllers
{
    public class AdminMoviesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminMovies
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Movies.ToListAsync());
        }

        // GET: AdminMovies/Details/5
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

        // GET: AdminMovies/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminMovies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(AdminMoviesCreateViewModel newMovie)
        {
            if (ModelState.IsValid)
            {
                Movie movie = new Movie
                {
                    MovieId = Guid.NewGuid().ToString(),
                    Name = newMovie.Name,
                    Rating = newMovie.Rating,
                    Category = newMovie.Category,
                    Description = newMovie.Description,
                    Duration = newMovie.Duration,
                    Price = newMovie.Price,
                    TrailerUrl = newMovie.TrailerUrl
                };
                db.Movies.Add(movie);
                await db.SaveChangesAsync();
                if(newMovie.HorizontalImage != null)
                {
                    string fileName = movie.MovieId + "-HorizontalPoster.png";
                    var imagePath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    newMovie.HorizontalImage.SaveAs(imagePath);
                }
                if(newMovie.VerticalImage != null)
                {
                    string fileName = movie.MovieId + "-VerticalPoster.png";
                    var imagePath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    newMovie.VerticalImage.SaveAs(imagePath);
                }
                return RedirectToAction("Index");
            }

            return View(newMovie);
        }

        // GET: AdminMovies/Edit/5
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
            AdminMoviesViewModel model = new AdminMoviesViewModel
            {
                MovieId = movie.MovieId,
                Name = movie.Name,
                Rating = movie.Rating,
                Category = movie.Category,
                Description = movie.Description,
                Duration = movie.Duration,
                Price = movie.Price,
                TrailerUrl = movie.TrailerUrl,
            };
            return View(model);
        }

        // POST: AdminMovies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(AdminMoviesViewModel model)
        {
            if (ModelState.IsValid)
            {
                Movie movie = new Movie
                {
                    MovieId = model.MovieId,
                    Name = model.Name,
                    Rating = model.Rating,
                    Category = model.Category,
                    Description = model.Description,
                    Duration = model.Duration,
                    Price = model.Price,
                    TrailerUrl = model.TrailerUrl
                };
                db.Entry(movie).State = EntityState.Modified;
                await db.SaveChangesAsync();

                if (model.HorizontalImage != null)
                {
                    string path = Server.MapPath("/Content/Images/" + model.MovieId + "-HorizontalPoster.png");
                    FileInfo fi = new FileInfo(path);
                    if (fi.Exists)
                    {
                        fi.Delete();
                    }
                    model.HorizontalImage.SaveAs(path);
                }
                if (model.VerticalImage != null)
                {
                    string path = Server.MapPath("/Content/Images/" + model.MovieId + "-VerticalPoster.png");
                    FileInfo fi = new FileInfo(path);
                    if (fi.Exists)
                    {
                        fi.Delete();
                    }
                    model.VerticalImage.SaveAs(path);
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: AdminMovies/Delete/5
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

        // POST: AdminMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Movie movie = await db.Movies.FindAsync(id);
            db.Movies.Remove(movie);
            await db.SaveChangesAsync();
            string path = Server.MapPath("/Content/Images/" + id + "-HorizontalPoster.png");
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            path = Server.MapPath("/Content/Images/" + id + "-VerticalPoster.png");
            fi = new FileInfo(path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            return RedirectToAction("Index");
        }

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
