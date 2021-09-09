﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VivedyWebApp.Areas.Admin.Models.ViewModels;
using VivedyWebApp.Models;

namespace VivedyWebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Application Admin Home Controller
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// The entities manager instance
        /// </summary>
        private readonly Entities Helper = new Entities();

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
        public async Task<ActionResult> BookingScanner()
        {
            HomeViewModel model = new HomeViewModel()
            {
                Screenings = await Helper.Screenings.GetSelectListItems()
            };
            return View(model);
        }

        /// <summary>
        /// GET request action for booking verification api action
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<JsonResult> VerifyBookings(string bookingId, string screeningId)
        {
            if (bookingId == null || screeningId == null)
            {
                return Json(new VerifyBookingsResult("HTTP 400: Bad request"), JsonRequestBehavior.AllowGet);
            }
            try
            {
                var result = new VerifyBookingsResult();
                Booking booking = await Helper.Bookings.Details(bookingId);
                if(booking == null || screeningId != booking.ScreeningId || booking.VerificationTime != null)
                {
                    result.verified = false;
                }
                else
                {
                    booking.VerificationTime = DateTime.Now;
                    await Helper.Bookings.Edit(booking);
                    result.verified = true;
                }
                return Json(result, JsonRequestBehavior.AllowGet);
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
    }
}