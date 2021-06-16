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

namespace VivedyWebApp.Controllers
{
    /// <summary>
    /// Application Admin Controller for Rotations
    /// </summary>
    public class AdminRotationsController : Controller
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
            return View(await db.Rotations.ToListAsync());
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
            Models.Rotation rotation = await db.Rotations.FindAsync(id);
            if (rotation == null)
            {
                return HttpNotFound();
            }
            return View(rotation);
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
        public async Task<ActionResult> Create(AdminRotationsCreateViewModel newRotation)
        {
            if (ModelState.IsValid)
            {
                Rotation rotation = new Rotation
                {
                    RotationId = Guid.NewGuid().ToString(),
                    StartTime = newRotation.StartTime,
                    MovieId = newRotation.MovieId
                };
                db.Rotations.Add(rotation);
                //Generating rotations
                if (newRotation.GenerateRotations)
                {
                    for(int i = 1; i < 7; i++)
                    {
                        //A rotation for each day starting from the newRotation.StartTime at the same time of the day
                        Rotation autoRotation = new Rotation
                        {
                            RotationId = Guid.NewGuid().ToString(),
                            StartTime = newRotation.StartTime.AddDays(i),
                            MovieId = newRotation.MovieId
                        };
                        db.Rotations.Add(autoRotation);
                    }
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(newRotation);
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
            Rotation rotation = await db.Rotations.FindAsync(id);
            if (rotation == null)
            {
                return HttpNotFound();
            }
            return View(rotation);
        }

        /// <summary>
        /// POST request action for Edit page
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(Rotation rotation)
        {
            if (!ModelState.IsValid)
            {
                return View(rotation);
            }
            db.Entry(rotation).State = EntityState.Modified;
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
            Rotation rotation = await db.Rotations.FindAsync(id);
            if (rotation == null)
            {
                return HttpNotFound();
            }
            return View(rotation);
        }

        /// <summary>
        /// POST request action for DeleteConfirmed page
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Rotation rotation = await db.Rotations.FindAsync(id);
            db.Rotations.Remove(rotation);
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
