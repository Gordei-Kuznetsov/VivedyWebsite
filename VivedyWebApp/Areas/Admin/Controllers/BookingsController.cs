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
    }

    public partial class Messages
    {
        public static string Error = "Something went wrong while processing your request.\nPlease try again.";
        public static BasicMessages<Booking> Bookings = new BasicMessages<Booking>();
        public static BasicMessages<Booking> Cinemas = new BasicMessages<Booking>();
        public static BasicMessages<Booking> Movies = new BasicMessages<Booking>();
        public static BasicMessages<Booking> Rooms = new BasicMessages<Booking>();
        public static BasicMessages<Booking> Screenings = new BasicMessages<Booking>();
        public static BasicMessages<Booking> Users = new BasicMessages<Booking>();

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
