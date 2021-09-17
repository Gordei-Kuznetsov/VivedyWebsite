﻿using Microsoft.AspNet.Identity;
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
        //Movies. Screenings, Bookings, Cinemas

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
            Screening screening = await Helper.Screenings.DetailsWithMovie(id);
            if(screening != null && screening.Movie.ClosingDate > DateTime.Now && screening.StartDate.Add(screening.StartTime) > DateTime.Now)
            {
                BookingSeatsViewModel seatsModel = new BookingSeatsViewModel
                {
                    SelectedScreeningId = id,
                    Screening = screening,
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
        public async Task<ViewResult> Seats(BookingSeatsViewModel model)
        {
            List<int> seats = Helper.Bookings.ConvertSeatsToIntList(model.SelectedSeats);
            if (!ModelState.IsValid || seats.Count == 0 || seats.Count > 16)
            {
                ViewBag.Message = Messages.Error;
                model.Screening = await Helper.Screenings.DetailsWithMovieAndRoom(model.SelectedScreeningId);
                model.Movie = model.Screening.Movie;
                model.OccupiedSeats = await Helper.Bookings.GetSeatsForScreening(model.SelectedScreeningId);
                return View(model);
            }
            
            Screening screening = await Helper.Screenings.DetailsWithMovieAndRoom(model.SelectedScreeningId);
            if (screening != null && screening.Movie.ClosingDate > DateTime.Now && screening.StartDate.Add(screening.StartTime) > DateTime.Now
                && !await Helper.Bookings.AnySeatsOverlapWith(seats, screening.Id))
            {
                //If user is logged in, then take their email and pass to the model
                ApplicationUser user = User.Identity.IsAuthenticated
                    ? await HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindByIdAsync(User.Identity.GetUserId())
                    : null;
                BookingPayViewModel payModel = new BookingPayViewModel
                {
                    SelectedSeats = model.SelectedSeats,
                    SelectedScreeningId = model.SelectedScreeningId,
                    Screening = screening,
                    Movie = screening.Movie,
                    TotalPrice = seats.Count() * screening.Movie.Price,
                    Email = (user != null) ? user.Email : ""
                };
                return View("Pay", payModel);
            }
            ViewBag.Message = Messages.Error;
            model.Screening = screening;
            model.Movie = screening.Movie;
            model.OccupiedSeats = await Helper.Bookings.GetSeatsForScreening(screening.Id);
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
            List<int> seats = Helper.Bookings.ConvertSeatsToIntList(model.SelectedSeats);
            if (!ModelState.IsValid || seats.Count == 0 || seats.Count > 16)
            {
                ViewBag.Message = Messages.Error;
                model.Screening = await Helper.Screenings.DetailsWithMovie(model.SelectedScreeningId);
                model.Movie = model.Screening.Movie;
                return View(model);
            }
            Screening screening = await Helper.Screenings.DetailsWithMovie(model.SelectedScreeningId);
            if (screening != null && screening.Movie.ClosingDate > DateTime.Now && screening.StartDate.Add(screening.StartTime) > DateTime.Now
                && ! await Helper.Bookings.AnySeatsOverlapWith(seats, screening.Id))
            {
                Booking booking = new Booking
                {
                    Seats = model.SelectedSeats,
                    PayedAmout = seats.Count * screening.Movie.Price,
                    UserEmail = model.Email,
                    ScreeningId = screening.Id
                };

                Booking newBooking = await Helper.Bookings.Create(booking);
                if (newBooking != null)
                {
                    int result = await Helper.Bookings.SendConfirmationEmail(newBooking);
                    if(result < 0)
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
                    model.Screening = screening;
                    model.Movie = screening.Movie;
                    return View(model);
                }
            }
            ViewBag.Message = Messages.Error;
            model.Screening = screening;
            model.Movie = screening.Movie;
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