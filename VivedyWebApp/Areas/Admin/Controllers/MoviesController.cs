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
using System.IO;
using VivedyWebApp.Areas.Admin.Models.ViewModels;

namespace VivedyWebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Application Admin Controller for Movies
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class MoviesController : Controller
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
            return View(await Helper.Movies.AllToList());
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
            Movie movie = await Helper.Movies.Details(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        /// <summary>
        /// GET request action for Create page
        /// </summary>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST request action for Create page
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MoviesCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Movie movie = new Movie()
            {
                Name = model.Name,
                Rating = model.Rating,
                ViewerRating = model.ViewerRating,
                Category = model.Category,
                Description = model.Description,
                Duration = model.Duration,
                Price = model.Price,
                TrailerUrl = model.TrailerUrl,
                ReleaseDate = model.ReleaseDate,
                ClosingDate = model.ClosingDate
            };
            if(movie.ReleaseDate >= movie.ClosingDate)
            {
                return View(model);
            }
            await Helper.Movies.CreateFrom(movie);
            //Saving the images uploaded for the posters
            if(model.HorizontalImage != null)
            {
                //Saving horizontal poster
                string fileName = movie.Id + "-HorizontalPoster.png";
                var imagePath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                model.HorizontalImage.SaveAs(imagePath);
            }
            if(model.VerticalImage != null)
            {
                //Saving vertical poster
                string fileName = movie.Id + "-VerticalPoster.png";
                var imagePath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                model.VerticalImage.SaveAs(imagePath);
            }
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
            Movie movie = await Helper.Movies.Details(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            MoviesAdminViewModel model = new MoviesAdminViewModel
            {
                Name = movie.Name,
                Rating = movie.Rating,
                ViewerRating = movie.ViewerRating,
                Category = movie.Category,
                Description = movie.Description,
                Duration = movie.Duration,
                Price = movie.Price,
                TrailerUrl = movie.TrailerUrl,
                ReleaseDate = movie.ReleaseDate,
                ClosingDate = movie.ClosingDate
            };
            return View(model);
        }

        /// <summary>
        /// POST request action for Edit page
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MoviesAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Movie movie = await Helper.Movies.Details(model.Id);
            if(movie == null)
            {
                return View(model);
            }
            movie.Name = model.Name;
            movie.Rating = model.Rating;
            movie.ViewerRating = model.ViewerRating;
            movie.Category = model.Category;
            movie.Description = model.Description;
            movie.Duration = model.Duration;
            movie.Price = model.Price;
            movie.TrailerUrl = model.TrailerUrl;
            movie.ReleaseDate = model.ReleaseDate;
            movie.ClosingDate = model.ClosingDate;

            if (movie.ReleaseDate >= movie.ClosingDate)
            {
                return View(model);
            }

            await Helper.Movies.Edit(movie);
            //Saving the images for the posters if re-uploaded 
            if (model.HorizontalImage != null)
            {
                //Deleting existing one
                string path = Server.MapPath("/Content/Images/" + movie.Id + "-HorizontalPoster.png");
                FileInfo fi = new FileInfo(path);
                if (fi.Exists)
                {
                    fi.Delete();
                }
                //Saving horizontal poster
                model.HorizontalImage.SaveAs(path);
            }
            if (model.VerticalImage != null)
            {
                //Deleting existing one
                string path = Server.MapPath("/Content/Images/" + movie.Id + "-VerticalPoster.png");
                FileInfo fi = new FileInfo(path);
                if (fi.Exists)
                {
                    fi.Delete();
                }
                //Saving vertical poster
                model.VerticalImage.SaveAs(path);
            }
            return RedirectToAction("Index");
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
            Movie movie = await Helper.Movies.Details(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
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
            Movie room = await Helper.Movies.Details(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            await Helper.Movies.Delete(id);

            //Deleting poster images
            //Deleting horizontal poster
            string path = Server.MapPath("/Content/Images/" + id + "-HorizontalPoster.png");
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            //Deleting vertical poster
            path = Server.MapPath("/Content/Images/" + id + "-VerticalPoster.png");
            fi = new FileInfo(path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            return RedirectToAction("Index");
        }
    }
}
