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
        /// <summary>
        /// The entities manager instance
        /// </summary>
        private readonly Entities Helper = new Entities();

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
            Movie movie = await Helper.Movies.Details(movieId);
            Cinema cinema = await Helper.Cinemas.Details(cinemaId);
            if( movie == null || cinema == null || movie.ClosingDate < DateTime.Now)
            {
                return HttpNotFound();
            }
            //Getting available Screenings for the movie
            List<ScreeningDetails> screenings = await Helper.Screenings.GetAllForMovieInCinema(movieId, cinemaId);
            if (screenings.Count == 0)
            {
                //If no Screenings found then send back to the Movies/Details page with a message
                ViewBag.ErrorMessage = "No screenings found for this movie in the selected cinema.";
                return RedirectToAction("Details", "Movies", routeValues: new { id = movieId });
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
            Screening screening = await Helper.Screenings.DetailsWithMovie(id);
            if(screening != null && screening.Movie.ClosingDate > DateTime.Now && screening.StartTime > DateTime.Now)
            {
                BookingSeatsViewModel seatsModel = new BookingSeatsViewModel
                {
                    SelectedScreeningId = id,
                    //Getting the movie from db to avoid depending on the object being sent through the request
                    Movie = screening.Movie,
                    OccupiedSeats = await Helper.Bookings.GetSeatsForScreening(id),
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
        public async Task<ViewResult> Seats(BookingSeatsViewModel seatsModel)
        {
            if (!ModelState.IsValid)
            {
                return View(seatsModel);
            }
            List<int> selectedSeats = Helper.Bookings.ConvertSeatsToIntList(seatsModel.SelectedSeats);
            Screening screening = await Helper.Screenings.DetailsWithMovie(seatsModel.SelectedScreeningId);
            if (screening != null && screening.Movie.ClosingDate > DateTime.Now && screening.StartTime > DateTime.Now
                && !(await Helper.Bookings.AnySeatsOverlapWith(selectedSeats, screening.Id)))
            {
                //If user is logged in, then take their email and pass to the model
                ApplicationUser user = User.Identity.IsAuthenticated
                    ? await HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindByIdAsync(User.Identity.GetUserId())
                    : null;
                BookingPayViewModel payModel = new BookingPayViewModel
                {
                    SelectedSeats = seatsModel.SelectedSeats,
                    SelectedScreeningId = seatsModel.SelectedScreeningId,
                    Movie = screening.Movie,
                    TotalPrice = selectedSeats.Count() * screening.Movie.Price,
                    Email = (user != null) ? user.Email : ""
                };
                return View("Pay", payModel);
            }
            seatsModel.Movie = screening.Movie;
            seatsModel.OccupiedSeats = await Helper.Bookings.GetSeatsForScreening(screening.Id);
            return View(seatsModel);
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
            List<int> seats = Helper.Bookings.ConvertSeatsToIntList(payModel.SelectedSeats);
            Screening screening = await Helper.Screenings.DetailsWithMovie(payModel.SelectedScreeningId);
            if (screening != null && screening.Movie.ClosingDate > DateTime.Now && screening.StartTime > DateTime.Now
                && !(await Helper.Bookings.AnySeatsOverlapWith(seats, screening.Id)))
            {
                Booking booking = new Booking
                {
                    Seats = payModel.SelectedSeats,
                    PayedAmout = seats.Count * screening.Movie.Price,
                    UserEmail = payModel.Email,
                    ScreeningId = screening.Id
                };

                Booking newBooking = await Helper.Bookings.CreateFrom(booking);
                if (newBooking != null)
                {
                    Helper.Bookings.SendConfirmationEmail(newBooking);
                    return View("Confirmation");
                }
                else
                {
                    ViewBag.ErrorMessage = "There was a problem processing your booking.";
                    return View("Error");
                }
            }
            payModel.Movie = screening.Movie;
            return View(payModel);
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