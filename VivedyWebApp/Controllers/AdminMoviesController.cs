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
        public async Task<ActionResult> Create(AdminMoviesViewModel newMovie)
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
                    string content = newMovie.HorizontalImage.ContentType;
                    string format = content.Substring(content.IndexOf('/') + 1);
                    string fileName = movie.MovieId + "HorizontalPoster." + format;
                    var imagePath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    newMovie.HorizontalImage.SaveAs(imagePath);
                }
                if(newMovie.VerticalImage != null)
                {
                    string content = newMovie.VerticalImage.ContentType;
                    string format = content.Substring(content.IndexOf('/') + 1);
                    string fileName = movie.MovieId + "VerticalPoster." + format;
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
            return View(movie);
        }

        // POST: AdminMovies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(movie);
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
