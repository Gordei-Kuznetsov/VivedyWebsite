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
            List<Movie> movies = await db.Movies.ToListAsync();
            if(movies == null)
            {
                return View(movies);
            }
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
            if (!ModelState.IsValid)
            {
                return View(timeModel);
            }
            MoviesBookingSeatsViewModel seatsModel = new MoviesBookingSeatsViewModel { 
                SelectedRotationId = timeModel.SelectedRotationId, 
                Movie = timeModel.Movie, 
                OccupiedSeats = "" };
            List<Booking> bookings = db.Bookings.Where(booking => booking.RotationId == timeModel.SelectedRotationId).ToList();
            if(bookings != null)
            {
                foreach (Booking booking in bookings)
                {
                    seatsModel.OccupiedSeats += booking.Seats;
                }
            }

            return View("BookingSeats", seatsModel);
        }

        // POST: /Movies/BookingSeats
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ViewResult BookingSeats(MoviesBookingSeatsViewModel seatsModel)
        {
            if (!ModelState.IsValid)
            {
                return View(seatsModel);
            }
            MoviesBookingPayViewModel payModel = new MoviesBookingPayViewModel { 
                SelectedSeats = seatsModel.SelectedSeats, 
                SelectedRotationId = seatsModel.SelectedRotationId, 
                Movie = seatsModel.Movie };
            return View("BookingPay", payModel);
        }

        // POST: /Movies/BookingPay
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ViewResult> BookingPay(MoviesBookingPayViewModel payModel)
        {
            if (!ModelState.IsValid)
            {
                return View(payModel);
            }
            Booking booking = new Booking
            {
                BookingId = Guid.NewGuid().ToString(),
                Seats = payModel.SelectedSeats,
                CreationDate = DateTime.Now,
                UserEmail = payModel.Email,
                RotationId = payModel.SelectedRotationId
            };
            db.Bookings.Add(booking);
            int result = db.SaveChanges();
            if (result > 0)
            {
                string htmlSeats = "";
                foreach(string seat in payModel.SelectedSeats.Split(','))
                {
                    htmlSeats += $"<li>{seat}</li>";
                }
                string dataToEncode = "{\"bookingId\":\"" + booking.BookingId + "\",\"email\":\"" + booking.UserEmail + "\"}";
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(dataToEncode);
                string qrCodeData = Convert.ToBase64String(plainTextBytes);
                DateTime StartTime = db.Rotations.Find(payModel.SelectedRotationId).StartTime;
                string subject = "Booking Confirmation";
                string mailbody = $"<div id=\"mainEmailContent\" style=\"-webkit-text-size-adjust: 100%; font-family: Verdana,sans-serif;\">" +
                                    $"<img style=\"display: block; margin-left: auto; margin-right: auto; height: 3rem; width: 3rem;\" src=\"http://vivedy.azurewebsites.net/favicon.ico\">" +
                                    $"<b><h2 style=\"text-align: center;\">Thank you for purchasing tickets at our website!</h2></b>" +
                                    $"<p>Below are details of your purchase.</p>" +
                                    $"<i><p>Please present this email when you arrive to the cinema to the our stuuf at the entrance to the auditorium.</p></i>" +
                                    $"<div style=\"box-sizing: inherit; padding: 0.01em 16px; margin-top: 16px; margin-bottom: 16px; box-shadow: 0 2px 5px 0 rgba(0,0,0,0.16),0 2px 10px 0 rgba(0,0,0,0.12);\">" +
                                        $"<h3>{payModel.Movie.Name}</h3>" +
                                        $"<h4><b>Date:</b> {StartTime.Date}</h4>" +
                                        $"<h4><b>Time:</b> {StartTime.TimeOfDay}</h4>" +
                                        $"<h4><b>Your seats:</b> </h4>" +
                                        $"<ul>" +
                                            $"{htmlSeats}" +
                                        $"</ul>" +
                                        $"<br>" +
                                        $"<img style=\"display: block; margin-left: auto; margin-right: auto;\" src=\"https://api.qrserver.com/v1/create-qr-code/?size=250&bgcolor=255-255-255&color=9-10-15&qzone=0&data={qrCodeData}\" alt=\"Qrcode\">" +
                                        $"<br>" +
                                    $"</div>" +
                                    $"<p>Go to our <a href=\"vivedy.azurewebsites.net\">website</a> to find more movies!</p>" +
                                  $"</div>";
                EmailService mailService = new EmailService();
                await mailService.SendAsync(payModel.Email, subject, mailbody);
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
