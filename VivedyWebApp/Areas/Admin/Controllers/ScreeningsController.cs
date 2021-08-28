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
    /// Application Admin Controller for Screenings
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class ScreeningsController : Controller
    {
        /// <summary>
        /// The entities manager instance
        /// </summary>
        private readonly Entities Helper = new Entities();

        /// <summary>
        /// GET request action for Index page
        /// </summary>
        public ActionResult Index()
        {
            return View(Helper.Screenings.AllToList());
        }

        /// <summary>
        /// GET request action for Details page
        /// </summary>
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Screening screening = Helper.Screenings.Details(id);
            if (screening == null)
            {
                return HttpNotFound();
            }
            return View(screening);
        }

        /// <summary>
        /// GET request action for Create page
        /// </summary>
        public ActionResult Create()
        {
            ScreeningsCreateViewModel model = new ScreeningsCreateViewModel()
            {
                Movies = Helper.GetMovieSelectListItems(),
                Rooms = Helper.GetRoomSelectListItems()
            };
            return View(model);
        }

        /// <summary>
        /// POST request action for Create page
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ScreeningsCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Screening screening = new Screening
                {
                    Id = Guid.NewGuid().ToString(),
                    StartTime = model.StartTime,
                    MovieId = model.MovieId,
                    RoomId = model.RoomId
                };
                Helper.Screenings.Create(screening);
                //Generating Screenings
                //Later will be replaced with the generaion from list of days and list of times
                if (model.GenerateScreenings)
                {
                    List<Screening> screenings = new List<Screening>();
                    for(int i = 1; i < 7; i++)
                    {
                        //A Screening for each day starting from the newScreening.StartTime at the same time of the day
                        Screening autoScreening = new Screening
                        {
                            Id = Guid.NewGuid().ToString(),
                            StartTime = screening.StartTime.AddDays(i),
                            MovieId = screening.MovieId,
                            RoomId = screening.RoomId
                        };
                        screenings.Add(autoScreening);
                    }
                    Helper.Screenings.SaveRange(screenings);
                }
                return RedirectToAction("Index");
            }

            return View(model);
        }

        /// <summary>
        /// GET request action for Edit page
        /// </summary>
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Screening screening = Helper.Screenings.Details(id);
            if (screening == null)
            {
                return HttpNotFound();
            }
            ScreeningsViewModel model = new ScreeningsViewModel()
            {
                Id = screening.Id,
                StartTime = screening.StartTime,
                MovieId = screening.MovieId,
                RoomId = screening.MovieId,
                Movies = Helper.GetMovieSelectListItems(),
                Rooms = Helper.GetRoomSelectListItems()
            };
            return View(model);
        }

        /// <summary>
        /// POST request action for Edit page
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ScreeningsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Screening screening = new Screening()
            {
                Id = model.Id,
                StartTime = model.StartTime,
                MovieId = model.MovieId,
                RoomId = model.RoomId
            };
            Helper.Screenings.Edit(screening);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// GET request action for Delete page
        /// </summary>
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Screening screening = Helper.Screenings.Details(id);
            if (screening == null)
            {
                return HttpNotFound();
            }
            return View(screening);
        }

        /// <summary>
        /// POST request action for DeleteConfirmed page
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Helper.Screenings.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
