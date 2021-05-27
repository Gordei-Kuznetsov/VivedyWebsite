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

        // GET: /Movies/Booking/5
        [AllowAnonymous]
        public async Task<ActionResult> Booking(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Rotation> rotations = await db.Rotations.Where(rotation => rotation.MovieId == id).ToListAsync();
            if (rotations.Count == 0)
            {
                ViewBag.ErrorMessage = "No rotations found for this movie";
                return RedirectToAction("Details", "Movies", id);
            }
            MoviesBookingViewModel model = new MoviesBookingViewModel { AvailableRotations = rotations };
            model.Movie = await db.Movies.FindAsync(id);
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
            var booking = new Booking
            { 
                BookingId = new Guid().ToString(), 
                CreationDate = System.DateTime.Now,
                RotationId = model.SelectedRotation.RotationId, 
                Seats = "need to implement the seat thingy",
                UserEmail = model.Email 
            };
            db.Bookings.Add(booking);
            int result = await db.SaveChangesAsync();
            return (result > 0) ? RedirectToAction("BookingConfirmation", "Movies") : RedirectToAction("Error");
        }

        // GET: /Movies/BookingConfirmation
        [AllowAnonymous]
        public ActionResult BookingConfirmation()
        {
            return View();
        }

        public JsonResult TakenSeats(string id)
        {
            if (id == null)
            {
                return Json(null);
            }
            List<Booking> allBookings = db.Bookings.Where(booking => booking.RotationId == id).ToList();
            string allseats = "";
            foreach(Booking booking in allBookings)
            {
                allseats += booking.Seats;
            }
            return Json(allseats);
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
