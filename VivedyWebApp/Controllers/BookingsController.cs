using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VivedyWebApp.Models;
using VivedyWebApp.Models.ViewModels;

namespace VivedyWebApp.Controllers
{
    /// <summary>
    /// Application Bookings Controller
    /// </summary>
    public class BookingsController : Controller
    {
        /// <summary>
        /// ApplicationDbContext instance
        /// </summary>
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// GET request action for Time page
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Time(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Getting available Screenings for the movie
            List<Screening> screenings = await db.Screenings.Where(screening => screening.MovieId == id).ToListAsync();
            if (screenings.Count == 0)
            {
                //If no Screenings found then send back to the Movies/Details page with a message
                ViewBag.ErrorMessage = "No screenings found for this movie.";
                return RedirectToAction("Details", "Movies", routeValues: new { id = id });
            }
            BookingTimeViewModel model = new BookingTimeViewModel
            {
                AvailableScreenings = screenings,
                Movie = await db.Movies.FindAsync(id)
            };
            return View(model);
        }

        /// <summary>
        /// POST request action for Time page
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ViewResult Time(BookingTimeViewModel timeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(timeModel);
            }
            BookingSeatsViewModel seatsModel = new BookingSeatsViewModel
            {
                SelectedScreeningId = timeModel.SelectedScreeningId,
                //Getting the movie from db to avoid depending on the object being sent through the request
                Movie = db.Movies.Find(db.Screenings.Find(timeModel.SelectedScreeningId).MovieId),
                OccupiedSeats = new List<int>(),
                SelectedSeats = ""
            };
            //Getting all bookings for the Screening to later get all occupied saets from them
            List<Booking> bookings = db.Bookings.Where(booking => booking.ScreeningId == timeModel.SelectedScreeningId).ToList();
            if (bookings != null)
            {
                //Getting a list of all occupied  seats
                foreach (Booking booking in bookings)
                {
                    foreach (string seat in booking.Seats.Split(','))
                    {
                        if (seat != null && seat != "") { seatsModel.OccupiedSeats.Add(Convert.ToInt32(seat)); }
                    }
                }
            }
            return View("Seats", seatsModel);
        }

        /// <summary>
        /// POST request action for Seats page
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ViewResult Seats(BookingSeatsViewModel seatsModel)
        {
            if (!ModelState.IsValid)
            {
                return View(seatsModel);
            }
            //Parsing the SelectedSeats string into a list
            List<int> selectedSeats = new List<int>();
            foreach (string seat in seatsModel.SelectedSeats.Split(','))
            {
                if (seat != null && seat != "") { selectedSeats.Add(Convert.ToInt32(seat)); }
            }
            //Getting the movie from db to avoid depending on the object being sent through the request
            Movie movie = db.Movies.Find(db.Screenings.Find(seatsModel.SelectedScreeningId).MovieId);
            BookingPayViewModel payModel = new BookingPayViewModel
            {
                SelectedSeats = seatsModel.SelectedSeats,
                SelectedScreeningId = seatsModel.SelectedScreeningId,
                Movie = movie,
                TotalPrice = selectedSeats.Count() * movie.Price
            };
            return View("Pay", payModel);
        }

        /// <summary>
        /// POST request action for Pay page
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ViewResult> Pay(BookingPayViewModel payModel)
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
                ScreeningId = payModel.SelectedScreeningId
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
                Screening screening = db.Screenings.Find(payModel.SelectedScreeningId);
                Movie movie = db.Movies.Find(screening.MovieId);
                string subject = "Booking Confirmation";
                List<ApplicationUser> Users = db.Users.Where(user => user.Email == payModel.Email).ToList();
                string greeting = Users.Count() > 0 ? "<b>Hi " + Users[0].Name + "</b><br/>" : "";
                //Generating an HTML body for the email
                string mailbody = $"<div id=\"mainEmailContent\" style=\"-webkit-text-size-adjust: 100%; font-family: Verdana,sans-serif;\">" +
                                    $"<img style=\"display: block; margin-left: auto; margin-right: auto; height: 3rem; width: 3rem;\" src=\"http://vivedy.azurewebsites.net/favicon.ico\">" +
                                    greeting +
                                    $"<b><h2 style=\"text-align: center;\">Thank you for purchasing tickets at our website!</h2></b>" +
                                    $"<p>Below are details of your purchase.</p>" +
                                    $"<i><p>Please present this email when you arrive to the cinema to the our stuuf at the entrance to the auditorium.</p></i>" +
                                    $"<div style=\"box-sizing: inherit; padding: 0.01em 16px; margin-top: 16px; margin-bottom: 16px; box-shadow: 0 2px 5px 0 rgba(0,0,0,0.16),0 2px 10px 0 rgba(0,0,0,0.12);\">" +
                                        $"<h3>{movie.Name}</h3>" +
                                        $"<h4><b>Date:</b> {screening.StartTime.ToString("dd MMMM yyyy")}</h4>" +
                                        $"<h4><b>Time:</b> {screening.StartTime.ToString("hh:mm tt")}</h4>" +
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
                return View("Confirmation");
            }
            else
            {
                ViewBag.ErrorMessage = "There was a problem processing your booking.";
                return View("Error");
            }
        }

        /// <summary>
        /// GET request action for Confirmation page
        /// </summary>
        [AllowAnonymous]
        public ActionResult Confirmation()
        {
            return View();
        }
    }
}