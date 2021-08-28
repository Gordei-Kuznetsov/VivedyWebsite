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
        public ActionResult Index()
        {
            return View(Helper.Movies.AllToList());
        }

        /// <summary>
        /// GET request action for Details page
        /// </summary>
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = Helper.Movies.Details(id);
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
        public ActionResult Create(MoviesCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
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
                Helper.Movies.CreateFrom(movie);
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
            return View(model);
        }

        /// <summary>
        /// GET request action for Edit page
        /// </summary>
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = Helper.Movies.Details(id);
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
        public ActionResult Edit(MoviesAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                Movie movie = new Movie()
                {
                    Id = model.Id,
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
                Helper.Movies.Edit(movie);
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
            return View(model);
        }

        /// <summary>
        /// GET request action for Delete page
        /// </summary>
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = Helper.Movies.Details(id);
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
        public ActionResult DeleteConfirmed(string id)
        {
            Helper.Movies.Delete(id);
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
