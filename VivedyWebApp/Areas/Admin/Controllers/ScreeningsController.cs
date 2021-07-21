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
    public class ScreeningsController : Controller
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
            return View(await db.Screenings.ToListAsync());
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
            Screening screening = await db.Screenings.FindAsync(id);
            if (screening == null)
            {
                return HttpNotFound();
            }
            return View(screening);
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
        public async Task<ActionResult> Create(ScreeningsCreateViewModel newScreening)
        {
            if (ModelState.IsValid)
            {
                Screening screening = new Screening
                {
                    ScreeningId = Guid.NewGuid().ToString(),
                    StartTime = newScreening.StartTime,
                    MovieId = newScreening.MovieId
                };
                db.Screenings.Add(screening);
                //Generating Screenings
                if (newScreening.GenerateScreenings)
                {
                    for(int i = 1; i < 7; i++)
                    {
                        //A Screening for each day starting from the newScreening.StartTime at the same time of the day
                        Screening autoScreening = new Screening
                        {
                            ScreeningId = Guid.NewGuid().ToString(),
                            StartTime = newScreening.StartTime.AddDays(i),
                            MovieId = newScreening.MovieId
                        };
                        db.Screenings.Add(autoScreening);
                    }
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(newScreening);
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
            Screening screening = await db.Screenings.FindAsync(id);
            if (screening == null)
            {
                return HttpNotFound();
            }
            return View(screening);
        }

        /// <summary>
        /// POST request action for Edit page
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(Screening screening)
        {
            if (!ModelState.IsValid)
            {
                return View(screening);
            }
            db.Entry(screening).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
            Screening screening = await db.Screenings.FindAsync(id);
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Screening screening = await db.Screenings.FindAsync(id);
            db.Screenings.Remove(screening);
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
