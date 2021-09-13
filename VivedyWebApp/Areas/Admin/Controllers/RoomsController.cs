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
        public async Task<ActionResult> Index(string message = null)
        {
            ViewBag.Message = message;
            return View(await Helper.Rooms.AllToListWithCinemas());
        }

        // GET: Admin/Rooms/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = await Helper.Rooms.DetailsWithCinema(id);
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
                Cinemas = await Helper.Cinemas.GetSelectListItems(),
                SeatsLayouts = Helper.Rooms.GetSelectLayoutListItems()
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
                ViewBag.Message = Messages.Error;
                model.Cinemas = await Helper.Cinemas.GetSelectListItems(model.CinemaId);
                model.SeatsLayouts = Helper.Rooms.GetSelectLayoutListItems(model.SeatsLayout);
                return View(model);
            }
            Room room = new Room()
            {
                Name = model.Name,
                SeatsLayout = model.SeatsLayout,
                CinemaId = model.CinemaId
            };
            Cinema cinema = await Helper.Cinemas.Details(model.CinemaId);
            if (cinema == null)
            {
                ViewBag.Message = Messages.Error;
                model.Cinemas = await Helper.Cinemas.GetSelectListItems(model.CinemaId);
                model.SeatsLayouts = Helper.Rooms.GetSelectLayoutListItems(model.SeatsLayout);
                return View(model);
            }
            var result = await Helper.Rooms.Create(room);
            if(result != null)
            {
                return RedirectToAction("Index", new { message = Messages.Rooms.Created });
            }
            else
            {
                ViewBag.Message = Messages.Rooms.CreateFailed;
                model.Cinemas = await Helper.Cinemas.GetSelectListItems(model.CinemaId);
                model.SeatsLayouts = Helper.Rooms.GetSelectLayoutListItems(model.SeatsLayout);
                return View(model);
            }
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
                SeatsLayouts = Helper.Rooms.GetSelectLayoutListItems(room.SeatsLayout),
                Cinemas = await Helper.Cinemas.GetSelectListItems(room.CinemaId)
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
                ViewBag.Message = Messages.Error;
                model.Cinemas = await Helper.Cinemas.GetSelectListItems(model.CinemaId);
                model.SeatsLayouts = Helper.Rooms.GetSelectLayoutListItems(model.SeatsLayout);
                return View(model);
            }
            Room room = await Helper.Rooms.Details(model.Id);
            Cinema cinema = await Helper.Cinemas.Details(model.CinemaId);
            if (room == null || cinema == null)
            {
                ViewBag.Message = Messages.Error;
                model.Cinemas = await Helper.Cinemas.GetSelectListItems(model.CinemaId);
                model.SeatsLayouts = Helper.Rooms.GetSelectLayoutListItems(model.SeatsLayout);
                return View(model);
            }

            room.Name = model.Name;
            room.SeatsLayout = model.SeatsLayout;
            room.CinemaId = model.CinemaId;

            var result = await Helper.Rooms.Edit(room);
            if (result != null)
            {
                return RedirectToAction("Index", new { message = Messages.Rooms.Edited });
            }
            else
            {
                ViewBag.Message = Messages.Rooms.EditFailed;
                model.Cinemas = await Helper.Cinemas.GetSelectListItems(model.CinemaId);
                model.SeatsLayouts = Helper.Rooms.GetSelectLayoutListItems(model.SeatsLayout);
                return View(model);
            }
        }

        // GET: Admin/Rooms/Delete/5
        public async Task<ActionResult> Delete(string id, string message = null)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = await Helper.Rooms.DetailsWithCinema(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.Message = message;
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
            int result = await Helper.Rooms.Delete(room);
            if(result > 0)
            {
                return RedirectToAction("Index", new { message = Messages.Rooms.Deleted });
            }
            else
            {
                return View("Delete", "Rooms", new { message = Messages.Rooms.DeleteFailed });
            }
        }
    }
}
