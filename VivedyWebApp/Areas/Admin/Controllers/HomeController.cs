using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VivedyWebApp.Models;

namespace VivedyWebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Application Admin Home Controller
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// ApplicationDbContext instance
        /// </summary>
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// GET request action for Index page
        /// </summary>
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET request action for Index page
        /// </summary>
        [Authorize(Roles = "Admin")]
        public ActionResult BookingScanner()
        {
            return View();
        }

        /// <summary>
        /// GET request action for booking verification api action
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult VerifyBookings(string data)
        {
            if (data == null)
            {
                return Json(new VerifyBookingsResult("HTTP 400: Bad request"), JsonRequestBehavior.AllowGet);
            }
            VerifyBookingsJsonResult bookingObject;
            try
            {
                byte[] decodedData = Convert.FromBase64String(data);
                string decodedString = Encoding.UTF8.GetString(decodedData);
                bookingObject = JsonConvert.DeserializeObject<VerifyBookingsJsonResult>(decodedString);
            }
            catch
            {
                return Json(new VerifyBookingsResult("Internal error occured while decoding the requested QR code data."), JsonRequestBehavior.AllowGet);
            }
            try
            {
                Booking booking = db.Bookings.Find(bookingObject.bookingId);
                if(booking == null)
                {
                    return Json(new VerifyBookingsResult(false), JsonRequestBehavior.AllowGet);
                }
                if(booking.UserEmail != bookingObject.email)
                {
                    return Json(new VerifyBookingsResult(false), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new VerifyBookingsResult(true), JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new VerifyBookingsResult("Internal error occured while verifying the booking."), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Class for the result of VerifyBookings api action
        /// </summary>
        private class VerifyBookingsResult
        {
            public VerifyBookingsResult(string error)
            {
                this.error = error;
                verified = false;
            }

            public VerifyBookingsResult(bool verified)
            {
                this.verified = verified;
            }

            public VerifyBookingsResult()
            {

            }

            /// <summary>
            /// Boolean field indicating wether the verification has successeded or not
            /// </summary>
            public bool verified;

            /// <summary>
            /// Field for any error messages that occure during the request proccesing
            /// </summary>
            public string error;
        }

        /// <summary>
        /// Class for the result of converting requested data to VerifyBookings api action from Json 
        /// </summary>
        private class VerifyBookingsJsonResult
        {
            public VerifyBookingsJsonResult(string bookingId, string email)
            {
                this.email = email;
                this.bookingId = bookingId;
            }

            public VerifyBookingsJsonResult()
            {

            }

            /// <summary>
            /// Field for the booking GUID
            /// </summary>
            public string bookingId;

            /// <summary>
            /// Field for the email of the user who made the booking
            /// </summary>
            public string email;
        }
    }
}