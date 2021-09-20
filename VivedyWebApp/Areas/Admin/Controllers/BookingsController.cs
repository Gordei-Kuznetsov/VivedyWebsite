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
    [Authorize(Roles = "Admin")]
    public class BookingsController : Controller
    {
        /// <summary>
        /// The entities manager instance
        /// </summary>
        private readonly Entities Helper = new Entities();
        //Only Bookings

        /// <summary>
        /// GET request action for Index page
        /// </summary>
        public async Task<ActionResult> Index(string message = null)
        {
            ViewBag.Message = message;
            return View(await Helper.Bookings.AllToListWithScreeningsAndMovies());
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
            Booking booking = await Helper.Bookings.DetailsWithScreeningAndMovie(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        /// <summary>
        /// GET request action for Delete page
        /// </summary>
        public async Task<ActionResult> Delete(string id, string message = null)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await Helper.Bookings.DetailsWithScreeningAndMovie(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.Message = message;
            return View(booking);
        }

        /// <summary>
        /// POST request action for DeleteConfirmed page
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await Helper.Bookings.Details(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            int result = await Helper.Bookings.Delete(booking);
            if(result > 0)
            {
                return RedirectToAction("Index", new { message = Messages.Bookings.Deleted });
            }
            else
            {
                return View("Delete", "Bookings", new { id = id, message = Messages.Error });
            }
        }

        /// <summary>
        /// GET request action for DeleteAllOld page
        /// </summary>
        public async Task<ActionResult> DeleteAllOld(string message = null)
        {

            List<Booking> bookings = await Helper.Bookings.GetAllOld();
            if (bookings.Count == 0)
            {
                return RedirectToAction("Index", new { message = Messages.NoFinishedBookings });
            }
            ViewBag.Message = message;
            return View(bookings);
        }

        /// <summary>
        /// POST request action for DeleteAllOldConfirmed page
        /// </summary>
        [HttpPost, ActionName("DeleteAllOld")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAllOldConfirmed()
        {
            int result = await Helper.Bookings.DeleteAllOld();
            if (result <= 0)
            {
                return View("DeleteAllOld", "Bookings", new { message = Messages.FinishedBookingsFailedDelete });
            }
            return RedirectToAction("Index", new { message = Messages.FinishedBookingsDeleted });
        }
    }

    public partial class Messages
    {
        public static string Error = "Something went wrong while processing your request.\nPlease try again.";
        public static string NoFinishedBookings = "There are no bookings for finished screenings at the moment.";
        public static string FinishedBookingsFailedDelete = "Failed to delete bookings for finished screenings.\nPlease try again.";
        public static string FinishedBookingsDeleted = "All bookings for finished screenings deleted.";
        public static BasicMessages<Booking> Bookings = new BasicMessages<Booking>();
        public static BasicMessages<Cinema> Cinemas = new BasicMessages<Cinema>();
        public static BasicMessages<Movie> Movies = new BasicMessages<Movie>();
        public static BasicMessages<Room> Rooms = new BasicMessages<Room>();
        public static BasicMessages<Screening> Screenings = new BasicMessages<Screening>();

        public class BasicMessages<TEntity> where TEntity : BaseModel
        {
            public BasicMessages()
            {
                string name = Activator.CreateInstance<TEntity>().GetType().Name;
                string successfully = "The " + name + " was successfully ";
                string failed = " of the " + name + " failed due to an error.";
                Created = successfully + "created.";
                Edited = successfully + "edited.";
                Deleted = successfully + "deleted.";
                CreateFailed = "Creation" + failed;
                EditFailed = "Modification" + failed;
                DeleteFailed = "Deletion" + failed;
            }
            public string Created;
            public string Edited;
            public string Deleted;
            public string CreateFailed;
            public string EditFailed;
            public string DeleteFailed;
        }
    }
}
