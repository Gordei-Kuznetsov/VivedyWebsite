using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VivedyWebApp.Models;
using VivedyWebApp.Areas.Admin.Models.ViewModels;
using System.Threading.Tasks;

namespace VivedyWebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Application Admin Controller for Rooms
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class RoomsController : Controller
    {
        /// <summary>
        /// The entities manager instance
        /// </summary>
        private readonly Entities Helper = new Entities();

        // GET: Admin/Rooms
        public ActionResult Index()
        {
            return View(Helper.Rooms.AllToList());
        }

        // GET: Admin/Rooms/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = await Helper.Rooms.Details(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Admin/Rooms/Create
        public async Task<ActionResult> Create()
        {
            RoomsCreateViewModel model = new RoomsCreateViewModel()
            {
                Cinemas = await Helper.Cinemas.GetSelectListItems()
            };
            return View(model);
        }

        // POST: Admin/Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoomsCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Cinemas = await Helper.Cinemas.GetSelectListItems();
                return View(model);
            }
            Room room = new Room()
            {
                Name = model.Name,
                SeatsLayout = model.SeatsLayout,
                CinemaId = model.CinemaId
            };
            Cinema cinema = await Helper.Cinemas.Details(model.CinemaId);
            if (cinema != null)
            {
                await Helper.Rooms.CreateFrom(room);
                return RedirectToAction("Index");
            }
            model.Cinemas = await Helper.Cinemas.GetSelectListItems();
            return View(model);
        }

        // GET: Admin/Rooms/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = await Helper.Rooms.Details(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            RoomsViewModel model = new RoomsViewModel()
            {
                Id = room.Id,
                Name = room.Name,
                SeatsLayout = room.SeatsLayout,
                Cinemas = await Helper.Cinemas.GetSelectListItems()
            };
            return View(model);
        }

        // POST: Admin/Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoomsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Cinemas = await Helper.Cinemas.GetSelectListItems();
                return View(model);
            }
            Room room = await Helper.Rooms.Details(model.Id);
            Cinema cinema = await Helper.Cinemas.Details(model.CinemaId);
            if (room != null || cinema != null)
            {
                room.Name = model.Name;
                room.SeatsLayout = model.SeatsLayout;
                room.CinemaId = model.CinemaId;

                await Helper.Rooms.Edit(room);
                return RedirectToAction("Index");
            }
            model.Cinemas = await Helper.Cinemas.GetSelectListItems();
            return View(model);
        }

        // GET: Admin/Rooms/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = await Helper.Rooms.Details(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Admin/Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = await Helper.Rooms.Details(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            await Helper.Rooms.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
