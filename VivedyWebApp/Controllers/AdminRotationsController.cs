﻿using System;
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
            Models.Rotation rotation = await db.Rotations.FindAsync(id);
            if (rotation == null)
            {
                return HttpNotFound();
            }
            return View(rotation);
        }

        // GET: AdminRotations/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminRotations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(AdminRotationsViewModel newRotation)
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
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(newRotation);
        }

        // GET: AdminRotations/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(string id)
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

        // POST: AdminRotations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "RotationId,StartTime,MovieId")] Models.Rotation rotation)
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
            Models.Rotation rotation = await db.Rotations.FindAsync(id);
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
            Models.Rotation rotation = await db.Rotations.FindAsync(id);
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
