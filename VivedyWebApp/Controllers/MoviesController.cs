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
    /// <summary>
    /// Application Movies Controller
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
        public async Task<ActionResult> Index()
        {
            List<Movie> movies = await db.Movies.ToListAsync();
            if(movies == null)
            {
                return View(movies);
            }
            //Getting lists of categories and ratings for the filters on the movies page
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

        /// <summary>
        /// GET request action for Details page
        /// </summary>
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
        /// GET request action for BookingTime page
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> BookingTime(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Getting available rotations for the movie
            List<Rotation> rotations = await db.Rotations.Where(rotation => rotation.MovieId == id).ToListAsync();
            if (rotations.Count == 0)
            {
                //If no rotaions found then send back to the Movies/Details page with a message
                ViewBag.ErrorMessage = "No rotations found for this movie.";
                return RedirectToAction("Details", "Movies", routeValues: new { id = id });
            }
            MoviesBookingTimeViewModel model = new MoviesBookingTimeViewModel { 
                AvailableRotations = rotations, 
                Movie = await db.Movies.FindAsync(id) 
            };
            return View(model);
        }

        /// <summary>
        /// POST request action for BookingTime page
        /// </summary>
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
                //Getting the movie from db to avoid depending on the object being sent through the request
                Movie = db.Movies.Find(db.Rotations.Find(timeModel.SelectedRotationId).MovieId), 
                OccupiedSeats = new List<int>(),
                SelectedSeats = ""
            };
            //Getting all bookings for the rotaion to later get all occupied saets from them
            List<Booking> bookings = db.Bookings.Where(booking => booking.RotationId == timeModel.SelectedRotationId).ToList();
            if(bookings != null)
            {
                //Getting a list of all occupied  seats
                foreach (Booking booking in bookings)
                {
                    foreach(string seat in booking.Seats.Split(','))
                    {
                        if(seat != null && seat != "") { seatsModel.OccupiedSeats.Add(Convert.ToInt32(seat)); }
                    }
                }
            }
            return View("BookingSeats", seatsModel);
        }

        /// <summary>
        /// POST request action for BookingSeats page
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ViewResult BookingSeats(MoviesBookingSeatsViewModel seatsModel)
        {
            if (!ModelState.IsValid)
            {
                return View(seatsModel);
            }
            //Parsing the SelectedSeats string into a list
            List<int> selectedSeats = new List<int>();
            foreach (string seat in seatsModel.SelectedSeats.Split(','))
            {
                if (seat != null && seat != "" ) { selectedSeats.Add(Convert.ToInt32(seat)); }
            }
            //Getting the movie from db to avoid depending on the object being sent through the request
            Movie movie = db.Movies.Find(db.Rotations.Find(seatsModel.SelectedRotationId).MovieId);
            MoviesBookingPayViewModel payModel = new MoviesBookingPayViewModel { 
                SelectedSeats = seatsModel.SelectedSeats, 
                SelectedRotationId = seatsModel.SelectedRotationId, 
                Movie = movie,
                TotalPrice = selectedSeats.Count() * movie.Price
            };
            return View("BookingPay", payModel);
        }

        /// <summary>
        /// POST request action for BookingPay page
        /// </summary>
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
                //Sending the email with the tickets to the email address provided
                //Will later be moved to the a method of EmailService class
                string htmlSeats = "";
                foreach (string seat in payModel.SelectedSeats.Split(','))
                {
                    if (seat != null && seat != "") { htmlSeats += $"<li>{seat}</li>"; }
                }
                //Generating content to put into the QR code for later validation
                //Includes BookingId and UserEmail
                string dataToEncode = "{\"bookingId\":\"" + booking.BookingId + "\",\"email\":\"" + booking.UserEmail + "\"}";
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(dataToEncode);
                string qrCodeData = "VIVEDYBOOKING_" + Convert.ToBase64String(plainTextBytes);
                Rotation rotation = db.Rotations.Find(payModel.SelectedRotationId);
                Movie movie = db.Movies.Find(rotation.MovieId);
                string subject = "Booking Confirmation";
                ApplicationUser User = db.Users.Where(user => user.Email == payModel.Email).First();
                string greeting = User != null ? "<b>Hi " + User.Name + "</b><br/>" : "";
                //Generating an HTML body for the email
                string mailbody = $"<div id=\"mainEmailContent\" style=\"-webkit-text-size-adjust: 100%; font-family: Verdana,sans-serif;\">" +
                                    $"<img style=\"display: block; margin-left: auto; margin-right: auto; height: 3rem; width: 3rem;\" src=\"http://vivedy.azurewebsites.net/favicon.ico\">" +
                                    greeting +
                                    $"<b><h2 style=\"text-align: center;\">Thank you for purchasing tickets at our website!</h2></b>" +
                                    $"<p>Below are details of your purchase.</p>" +
                                    $"<i><p>Please present this email when you arrive to the cinema to the our stuuf at the entrance to the auditorium.</p></i>" +
                                    $"<div style=\"box-sizing: inherit; padding: 0.01em 16px; margin-top: 16px; margin-bottom: 16px; box-shadow: 0 2px 5px 0 rgba(0,0,0,0.16),0 2px 10px 0 rgba(0,0,0,0.12);\">" +
                                        $"<h3>{movie.Name}</h3>" +
                                        $"<h4><b>Date:</b> {rotation.StartTime.ToString("dd MMMM yyyy")}</h4>" +
                                        $"<h4><b>Time:</b> {rotation.StartTime.ToString("hh:mm tt")}</h4>" +
                                        $"<h4><b>Your seats:</b> </h4>" +
                                        $"<ul>" +
                                            $"{htmlSeats}" +
                                        $"</ul>" +
                                        $"<h4><b>Total amount paid:</b> ${payModel.TotalPrice}</h4>" +
                                        $"<br>" +
                                        $"<img style=\"display: block; margin-left: auto; margin-right: auto;\" src=\"https://api.qrserver.com/v1/create-qr-code/?size=250&bgcolor=255-255-255&color=9-10-15&qzone=0&data=" + qrCodeData + "\" alt=\"Qrcode\">" +
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

        /// <summary>
        /// GET request action for BookingConfirmation page
        /// </summary>
        [AllowAnonymous]
        public ActionResult BookingConfirmation()
        {
            return View();
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
