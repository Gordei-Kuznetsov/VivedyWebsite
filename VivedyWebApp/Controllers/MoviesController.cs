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

        // GET: /Movies/BookingTime/5
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> BookingTime(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Rotation> rotations = await db.Rotations.Where(rotation => rotation.MovieId == id).ToListAsync();
            if (rotations.Count == 0)
            {
                ViewBag.ErrorMessage = "No rotations found for this movie.";
                return RedirectToAction("Details", "Movies", routeValues: new { id = id });
            }
            MoviesBookingTimeViewModel model = new MoviesBookingTimeViewModel { AvailableRotations = rotations };
            model.Movie = await db.Movies.FindAsync(id);
            return View(model);
        }

        // POST: /Movies/BookingTime
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ViewResult BookingTime(MoviesBookingTimeViewModel timeModel)
        {
            MoviesBookingSeatsViewModel seatsModel = new MoviesBookingSeatsViewModel { SelectedRotation = timeModel.SelectedRotation, Movie = timeModel.Movie };
            List<Booking> bookings = db.Bookings.Where(booking => booking.RotationId == timeModel.SelectedRotation.RotationId).ToList();
            foreach(Booking booking in bookings)
            {
                char separator = ',';
                seatsModel.OccupiedSeats.AddRange(booking.Seats.Split(separator));
            }
            return View("BookingSeats", seatsModel);
        }

        // POST: /Movies/BookingSeats
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ViewResult BookingSeats(MoviesBookingSeatsViewModel seatsModel)
        {
            MoviesBookingPayViewModel payModel = new MoviesBookingPayViewModel { SelectedSeats = seatsModel.SelectedSeats, SelectedRotation = seatsModel.SelectedRotation, Movie = seatsModel.Movie };
            return View("BookingPay", payModel);
        }

        // POST: /Movies/BookingPay
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ViewResult BookingPayAsync(MoviesBookingPayViewModel payModel)
        {
            Booking booking = new Booking
            {
                BookingId = new Guid().ToString(),
                Seats = payModel.SelectedSeats,
                CreationDate = DateTime.Now,
                UserEmail = payModel.Email,
                RotationId = payModel.SelectedRotation.RotationId
            };
            db.Bookings.Add(booking);
            int result = db.SaveChanges();
            if (result > 0)
            {
                return View("BookingConfirmation");
            }
            else
            {
                ViewBag.ErrorMessage = "There was a problem processing your booking.";
                return View("Error");
            }
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
