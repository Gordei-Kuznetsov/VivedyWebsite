using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
        public BookingsController()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Movies = new MoviesManager(db);
            Screenings = new ScreeningsManager(db);
            Bookings = new BookingsManager(db);
            Cinemas = new CinemasManager(db);
        }

        private readonly MoviesManager Movies;
        private readonly ScreeningsManager Screenings;
        private readonly BookingsManager Bookings;
        private readonly CinemasManager Cinemas;

        /// <summary>
        /// GET request action for Time page
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Time(string movieId, string cinemaId)
        {
            if (movieId == null || cinemaId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = await Movies.DetailsAsync(movieId);
            Cinema cinema = await Cinemas.DetailsAsync(cinemaId);
            if( movie == null || cinema == null || movie.ClosingDate < DateTime.Now)
            {
                return HttpNotFound();
            }
            //Getting available Screenings for the movie
            List<ScreeningDetails> screenings = await Screenings.AllForMovieInCinemaAsync(movieId, cinemaId);
            if (screenings.Count == 0)
            {
                //If no Screenings found then send back to the Movies/Details page with a message
                return RedirectToAction("Details", "Movies", routeValues: new { id = movieId, message = Messages.NoScreenings });
            }
            BookingTimeViewModel model = new BookingTimeViewModel
            {
                AvailableScreenings = screenings,
                Movie = movie
            };
            return View(model);
        }

        /// <summary>
        /// POST request action for Time page
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Seats(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Screening screening = await Screenings.DetailsWithMovieAndRoomAsync(id);
            if(screening != null && screening.Movie.ClosingDate > DateTime.Now && screening.StartDate.Add(screening.StartTime) > DateTime.Now)
            {
                BookingSeatsViewModel seatsModel = new BookingSeatsViewModel
                {
                    SelectedScreeningId = id,
                    Screening = screening,
                    OccupiedSeats = await Bookings.SeatsForScreeningAsync(id),
                    SelectedSeats = ""
                };
                return View("Seats", seatsModel);
            }
            else
            {
                return HttpNotFound();
            }
        }

        /// <summary>
        /// POST request action for Seats page
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ViewResult> Seats(BookingSeatsViewModel model)
        {
            List<string> seats = Bookings.ConvertSeats(model.SelectedSeats);
            if (!ModelState.IsValid || seats.Count == 0 || seats.Count > 16)
            {
                ViewBag.Message = Messages.Error;
                model.Screening = await Screenings.DetailsWithMovieAndRoomAsync(model.SelectedScreeningId);
                model.OccupiedSeats = await Bookings.SeatsForScreeningAsync(model.SelectedScreeningId);
                return View(model);
            }
            
            Screening screening = await Screenings.DetailsWithMovieAndRoomAsync(model.SelectedScreeningId);
            if (screening != null && screening.Movie.ClosingDate > DateTime.Now && screening.StartDate.Add(screening.StartTime) > DateTime.Now
                && !await Bookings.AnySeatsOverlapWithAsync(seats, screening.Id))
            {
                //If user is logged in, then take their email and pass to the model
                ApplicationUser user = User.Identity.IsAuthenticated
                    ? await HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindByIdAsync(User.Identity.GetUserId())
                    : null;
                BookingPayViewModel payModel = new BookingPayViewModel
                {
                    SelectedSeats = model.SelectedSeats,
                    SeparateSeats = seats,
                    SelectedScreeningId = model.SelectedScreeningId,
                    Screening = screening,
                    TotalPrice = seats.Count() * screening.Movie.Price,
                    Email = (user != null) ? user.Email : ""
                };
                return View("Pay", payModel);
            }
            ViewBag.Message = Messages.Error;
            model.Screening = screening;
            model.OccupiedSeats = await Bookings.SeatsForScreeningAsync(model.SelectedScreeningId);
            return View(model);
        }

        /// <summary>
        /// POST request action for Pay page
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ViewResult> Pay(BookingPayViewModel model)
        {
            List<string> seats = Bookings.ConvertSeats(model.SelectedSeats);
            if (!ModelState.IsValid || seats.Count == 0 || seats.Count > 16)
            {
                ViewBag.Message = Messages.Error;
                model.SeparateSeats = seats;
                model.Screening = await Screenings.DetailsWithMovieAsync(model.SelectedScreeningId);
                return View(model);
            }
            Screening screening = await Screenings.DetailsWithMovieAsync(model.SelectedScreeningId);
            if (screening != null && screening.Movie.ClosingDate > DateTime.Now && screening.StartDate.Add(screening.StartTime) > DateTime.Now
                && ! await Bookings.AnySeatsOverlapWithAsync(seats, screening.Id))
            {
                Booking booking = new Booking
                {
                    Seats = model.SelectedSeats,
                    PayedAmout = seats.Count * screening.Movie.Price,
                    UserEmail = model.Email,
                    ScreeningId = screening.Id
                };

                Booking newBooking = await Bookings.CreateAsync(booking);
                if (newBooking != null)
                {
                    newBooking.SeparateSeats = seats;
                    newBooking.Screening = screening;
                    int result = await Bookings.SendConfirmationEmailAsync(newBooking, this);
                    if(result == 0)
                    {
                        ViewBag.Message = Messages.FailedBookingEmail;
                        return View("Error");
                    }
                    else
                    {
                        return View("Confirmation");
                    }
                }
                else
                {
                    ViewBag.Message = Messages.FailedBooking;
                    model.SeparateSeats = seats;
                    model.Screening = screening;
                    return View(model);
                }
            }
            ViewBag.Message = Messages.Error;
            model.SeparateSeats = seats;
            model.Screening = screening;
            return View(model);
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

    public partial class Messages
    {
        public static string Error = "Something went wrong while processing your request.\nPlease try again.";
        public static string NoScreenings = "No screenings found for this movie in the selected cinema.";
        public static string FailedBooking = "Something went wrong while processing your booking.\nPlease try again.";
        public static string FailedBookingEmail = "Something went wrong while sending your booking confirmation email.\nPlease contact our service desk.";
    }
}