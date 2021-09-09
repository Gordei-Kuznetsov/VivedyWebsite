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
        public async Task<ActionResult> Index()
        {
            return View(await Helper.Screenings.AllToList());
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
            Screening screening = await Helper.Screenings.Details(id);
            if (screening == null)
            {
                return HttpNotFound();
            }
            return View(screening);
        }

        /// <summary>
        /// GET request action for Create page
        /// </summary>
        public async Task<ActionResult> Create()
        {
            ScreeningsCreateViewModel model = new ScreeningsCreateViewModel()
            {
                Movies = await Helper.Movies.GetSelectListItems(),
                Rooms = await Helper.Rooms.GetSelectListItems()
            };
            return View(model);
        }

        /// <summary>
        /// POST request action for Create page
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ScreeningsCreateViewModel model)
        {
            // Method needs improvement for cases when a screening cannot be creaed
            // Either terminate the process or return a message with the results

            if (!ModelState.IsValid)
            {
                model.Movies = await Helper.Movies.GetSelectListItems();
                model.Rooms = await Helper.Rooms.GetSelectListItems();
                return View(model);
            }
            Screening screening = new Screening
            {
                Id = Guid.NewGuid().ToString(),
                StartTime = model.StartTime,
                MovieId = model.MovieId,
                RoomId = model.RoomId
            };
            Movie movie = await Helper.Movies.Details(screening.MovieId);
            Room room = await Helper.Rooms.Details(model.RoomId);
            if (movie == null || room == null)
            {
                model.Movies = await Helper.Movies.GetSelectListItems();
                model.Rooms = await Helper.Rooms.GetSelectListItems();
                return View(model);
            }
            List<Screening> screenings = new List<Screening>();
            if (await Helper.Screenings.IsDuringMovieShowing(screening, movie) 
                && !(await Helper.Screenings.AnyOverlapWith(screening, movie.Duration)))
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
                    if (await Helper.Screenings.IsDuringMovieShowing(genScreening, movie) 
                        && !(await Helper.Screenings.AnyOverlapWith(genScreening, movie.Duration)))
                    {
                        screenings.Add(genScreening);
                    }
                }
            }
            await Helper.Screenings.SaveRange(screenings);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// GET request action for Edit page
        /// </summary>
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Screening screening = await Helper.Screenings.Details(id);
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
                Movies = await Helper.Movies.GetSelectListItems(),
                Rooms = await Helper.Rooms.GetSelectListItems()
            };
            return View(model);
        }

        /// <summary>
        /// POST request action for Edit page
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ScreeningsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Movies = await Helper.Movies.GetSelectListItems();
                model.Rooms = await Helper.Rooms.GetSelectListItems();
                return View(model);
            }
            Screening screening = await Helper.Screenings.DetailsWithMovie(model.Id);
            if (screening == null)
            {
                return HttpNotFound();
            }
            screening.Id = model.Id;
            screening.StartTime = model.StartTime;
            screening.MovieId = model.MovieId;
            screening.RoomId = model.RoomId;
            if (await Helper.Screenings.IsDuringMovieShowing(screening, screening.Movie) 
                && !(await Helper.Screenings.AnyOverlapWith(screening, screening.Movie.Duration)))
            {
                //possibly send notifying email to customers that there are changes
                await Helper.Screenings.Edit(screening);
                return RedirectToAction("Index");
            }
            model.Movies = await Helper.Movies.GetSelectListItems();
            model.Rooms = await Helper.Rooms.GetSelectListItems();
            return View(model);
        }

        /// <summary>
        /// GET request action for Delete page
        /// </summary>
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Screening screening = await Helper.Screenings.Details(id);
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
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Screening screening = await Helper.Screenings.Details(id);
            if (screening == null)
            {
                return HttpNotFound();
            }
            await Helper.Screenings.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
