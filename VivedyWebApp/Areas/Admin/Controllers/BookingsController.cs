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
using VivedyWebApp.Areas.Admin.Models.ViewModels;

namespace VivedyWebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Application Admin Controller for Bookings
    /// </summary>
    public class BookingsController : Controller
    {
        /// <summary>
        /// ApplicationDbContext instance
        /// </summary>
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// GET request action for Index page
        /// </summary>
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Bookings.ToListAsync());
        }

        /// <summary>
        /// GET request action for Details page
        /// </summary>
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        /// <summary>
        /// GET request action for Create page
        /// </summary>
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST request action for Create page
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(BookingsNewViewModel newBooking)
        {
            if (ModelState.IsValid)
            {
                Booking booking = new Booking
                {
                    BookingId = Guid.NewGuid().ToString(),
                    Seats = newBooking.Seats,
                    CreationDate = DateTime.Now,
                    UserEmail = newBooking.UserEmail,
                    ScreeningId = newBooking.ScreeningId
                };
                db.Bookings.Add(booking);
                int result = await db.SaveChangesAsync();
                if (result > 0)
                {
                    //Sending the email with the tickets to the email address provided
                    //Will later be moved to the a method of EmailService class
                    Screening screening = db.Screenings.Find(booking.ScreeningId);
                    Movie movie = db.Movies.Find(screening.MovieId);
                    string htmlSeats = "";
                    int TotalPrice = 0;
                    foreach (string seat in booking.Seats.Split(','))
                    {
                        if (seat != null && seat != "") 
                        { 
                            htmlSeats += $"<li>{seat}</li>";
                            TotalPrice += movie.Price;
                        }
                    }
                    //Generating content to put into the QR code for later validation
                    //Includes BookingId and UserEmail
                    string dataToEncode = "{\"bookingId\":\"" + booking.BookingId + "\",\"email\":\"" + booking.UserEmail + "\"}";
                    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(dataToEncode);
                    string qrCodeData = "VIVEDYBOOKING_" + Convert.ToBase64String(plainTextBytes);
                    string subject = "Booking Confirmation";
                    List<ApplicationUser> Users = db.Users.Where(user => user.Email == booking.UserEmail).ToList();
                    string greeting = Users.Count() > 0? "<b>Hi " + Users[0].Name + "</b><br/>" : "";
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
                                            $"<h4><b>Total amount paid:</b> ${TotalPrice}</h4>" +
                                            $"<br>" +
                                            $"<img style=\"display: block; margin-left: auto; margin-right: auto;\" src=\"https://api.qrserver.com/v1/create-qr-code/?size=250&bgcolor=255-255-255&color=9-10-15&qzone=0&data={qrCodeData}\" alt=\"Qrcode\">" +
                                            $"<br>" +
                                        $"</div>" +
                                        $"<p>Go to our <a href=\"vivedy.azurewebsites.net\">website</a> to find more movies!</p>" +
                                      $"</div>";
                    EmailService mailService = new EmailService();
                    await mailService.SendAsync(booking.UserEmail, subject, mailbody);
                    return RedirectToAction("Index", "Bookings");
                }
                else
                {
                    ViewBag.ErrorMessage = "There was a problem processing your booking.";
                    return View("Error");
                }
            }

            return View(newBooking);
        }

        /// <summary>
        /// GET request action for Edit page
        /// </summary>
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        /// <summary>
        /// POST request action for Edit page
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "BookingId,CreationDate,ScreeningId,UserEmail")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(booking);
        }

        /// <summary>
        /// GET request action for Delete page
        /// </summary>
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        /// <summary>
        /// POST request action for DeleteConfirmed page
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Booking booking = await db.Bookings.FindAsync(id);
            db.Bookings.Remove(booking);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
