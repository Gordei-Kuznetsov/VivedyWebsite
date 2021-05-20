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

namespace VivedyWebApp.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Movies
        public async Task<ActionResult> Index()
        {
            return View(await db.Movies.ToListAsync());
        }

        // GET: Movies/Details/5
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

        // GET: Movies/CreateMovie
        [AllowAnonymous]
        public ActionResult CreateMovie()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        // POST: Movies/CreateMovie
        public async Task<ActionResult> CreateMovie(Movie model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            db.Movies.Add(model);
            var result = await db.SaveChangesAsync();
            return RedirectToAction("Index", "Movies");
        }

        // GET: Movies/CreateRotation
        [AllowAnonymous]
        public ActionResult CreateRotation()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        // POST: Movies/CreateRotation
        public async Task<ActionResult> CreateRotation(Rotation model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            db.Rotations.Add(model);
            var result = await db.SaveChangesAsync();
            return RedirectToAction("Index", "Movies");
        }

        // GET: /Movies/Booking/5
        [AllowAnonymous]
        public async Task<ActionResult> Booking(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Rotation> rotations = await db.Rotations.Where(rotation => rotation.MovieId == id).ToListAsync();
            if (rotations == null)
            {
                ViewBag.ErrorMessage = "No rotations found for this movie";
                return RedirectToAction("Details", "Movies", id);
            }
            MoviesBookingViewModel model = new MoviesBookingViewModel { Rotations = rotations };
            return View(model);
        }

        // POST: /Movies/Booking
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Booking(MoviesBookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var booking = new Booking { 
                BookingId = new Guid().ToString(), 
                CreationDate = DateTime.Now, 
                RotationId = model.PickedRotation.RotationId, 
                Seats = new List<int> { model.Seat }, 
                UserEmail = model.Email };
            db.Bookings.Add(booking);
            var result = await db.SaveChangesAsync();
            return (result > 0) ? RedirectToAction("BookingConfirmation", "Movies") : RedirectToAction("Error");
        }

        // GET: /Movies/BookingConfirmation
        [AllowAnonymous]
        public ActionResult BookingConfirmation()
        {
            return View();
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
