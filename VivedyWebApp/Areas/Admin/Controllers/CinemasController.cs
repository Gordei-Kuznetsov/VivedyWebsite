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
    /// Application Admin Controller for Cinemas
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class CinemasController : Controller
    {
        /// <summary>
        /// The entities manager instance
        /// </summary>
        private readonly Entities Helper = new Entities();

        // GET: Admin/Cinemas
        public async Task<ActionResult> Index()
        {
            return View(await Helper.Cinemas.AllToList());
        }

        // GET: Admin/Cinemas/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinema cinema = await Helper.Cinemas.Details(id);
            if (cinema == null)
            {
                return HttpNotFound();
            }
            return View(cinema);
        }

        // GET: Admin/Cinemas/Create
        public ActionResult Create()
        {
            return View();
        }

        //POST: Admin/Cinemas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Exclude = "Id")] Cinema cinema)
        {
            if (ModelState.IsValid)
            {
                await Helper.Cinemas.CreateFrom(cinema);
                return RedirectToAction("Index");
            }
            return View(cinema);
        }

        // GET: Admin/Cinemas/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinema cinema = await Helper.Cinemas.Details(id);
            if (cinema == null)
            {
                return HttpNotFound();
            }
            return View(cinema);
        }

        // POST: Admin/Cinemas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Cinema model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Cinema cinema = await Helper.Cinemas.Details(model.Id);
            if(cinema == null)
            {
                return View(model);
            }
            await Helper.Cinemas.Edit(cinema);
            return RedirectToAction("Index");
        }

        // GET: Admin/Cinemas/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinema cinema = await Helper.Cinemas.Details(id);
            if (cinema == null)
            {
                return HttpNotFound();
            }
            return View(cinema);
        }

        // POST: Admin/Cinemas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinema cinema = await Helper.Cinemas.Details(id);
            if (cinema == null)
            {
                return HttpNotFound();
            }
            await Helper.Cinemas.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
