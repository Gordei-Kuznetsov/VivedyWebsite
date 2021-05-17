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

namespace VivedyWebApp.Controllers
{
    public class RotationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rotations
        public async Task<ActionResult> Index()
        {
            var rotations = db.Rotations.Include(r => r.Movie);
            return View(await rotations.ToListAsync());
        }

        // GET: Rotations/Details/5
        public async Task<ActionResult> Details(string id)
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

        // GET: Rotations/Create
        public ActionResult Create()
        {
            ViewBag.MovieId = new SelectList(db.Movies, "MovieId", "Name");
            return View();
        }

        // POST: Rotations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RotationId,StartTime,MovieId")] Rotation rotation)
        {
            if (ModelState.IsValid)
            {
                db.Rotations.Add(rotation);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MovieId = new SelectList(db.Movies, "MovieId", "Name", rotation.MovieId);
            return View(rotation);
        }

        // GET: Rotations/Edit/5
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
            ViewBag.MovieId = new SelectList(db.Movies, "MovieId", "Name", rotation.MovieId);
            return View(rotation);
        }

        // POST: Rotations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RotationId,StartTime,MovieId")] Rotation rotation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rotation).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MovieId = new SelectList(db.Movies, "MovieId", "Name", rotation.MovieId);
            return View(rotation);
        }

        // GET: Rotations/Delete/5
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

        // POST: Rotations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Rotation rotation = await db.Rotations.FindAsync(id);
            db.Rotations.Remove(rotation);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

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
