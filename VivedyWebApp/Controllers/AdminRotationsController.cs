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
    public class AdminRotationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminRotations
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Rotations.ToListAsync());
        }

        // GET: AdminRotations/Details/5
        [Authorize(Roles = "Admin")]
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

        // GET: AdminRotations/New
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            return View();
        }

        // POST: AdminRotations/New
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> New([Bind(Include = "RotationId,StartTime,MovieId")] Rotation rotation)
        {
            if (ModelState.IsValid)
            {
                db.Rotations.Add(rotation);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(rotation);
        }

        // GET: AdminRotations/Edit/5
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

        // POST: AdminRotations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "RotationId,StartTime,MovieId")] Rotation rotation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rotation).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(rotation);
        }

        // GET: AdminRotations/Delete/5
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

        // POST: AdminRotations/Delete/5
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
