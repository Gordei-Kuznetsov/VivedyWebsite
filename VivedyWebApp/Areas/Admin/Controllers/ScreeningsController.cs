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
                Movies = Helper.Movies.GetSelectListItems(),
                Rooms = Helper.Rooms.GetSelectListItems()
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
            // Method needs improvement for cases when a screening cannot be creaed
            // Either terminate the process or return a message with the results

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Screening screening = new Screening
            {
                Id = Guid.NewGuid().ToString(),
                StartTime = model.StartTime,
                MovieId = model.MovieId,
                RoomId = model.RoomId
            };
            Movie movie = Helper.Movies.Details(screening.MovieId);
            Room room = Helper.Rooms.Details(model.RoomId);
            if (movie == null || room == null)
            {
                return View(model);
            }
            List<Screening> screenings = new List<Screening>();
            if (Helper.Screenings.IsDuringMovieShowing(screening, movie) && !Helper.Screenings.AnyOverlapWith(screening))
            {
                screenings.Add(screening);
            }
            //Generating Screenings
            //Later will be replaced with the generaion from list of days and list of times
            if (model.GenerateScreenings)
            {
                for (int i = 1; i < 7; i++)
                {
                    //A Screening for each day starting from the newScreening.StartTime at the same time of the day
                    Screening genScreening = new Screening
                    {
                        Id = Guid.NewGuid().ToString(),
                        StartTime = screening.StartTime.AddDays(i),
                        MovieId = screening.MovieId,
                        RoomId = screening.RoomId
                    };
                    //Doesn't have to be chacked against other generated screenings as they are at different days anyway
                    if (Helper.Screenings.IsDuringMovieShowing(genScreening, movie) && !Helper.Screenings.AnyOverlapWith(genScreening))
                    {
                        screenings.Add(genScreening);
                    }
                }
            }
            Helper.Screenings.SaveRange(screenings);
            return RedirectToAction("Index");
            
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
                Movies = Helper.Movies.GetSelectListItems(),
                Rooms = Helper.Rooms.GetSelectListItems()
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
            Screening screening = Helper.Screenings.DetailsWithMovie(model.Id);
            if (screening == null)
            {
                return HttpNotFound();
            }
            screening.Id = model.Id;
            screening.StartTime = model.StartTime;
            screening.MovieId = model.MovieId;
            screening.RoomId = model.RoomId;
            if (Helper.Screenings.IsDuringMovieShowing(screening, screening.Movie) && !Helper.Screenings.AnyOverlapWith(screening))
            {
                //possibly send notifying email to customers that there are changes
                Helper.Screenings.Edit(screening);
                return RedirectToAction("Index");
            }
            return View(model);
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Screening screening = Helper.Screenings.Details(id);
            if (screening == null)
            {
                return HttpNotFound();
            }
            Helper.Screenings.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
