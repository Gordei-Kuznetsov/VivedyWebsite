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
using Microsoft.AspNet.Identity.Owin;
using VivedyWebApp.Models.ViewModels;
using Microsoft.AspNet.Identity;
using VivedyWebApp.Areas.Admin.Models.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace VivedyWebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Application Admin Controller for ApplicationUsers
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        /// <summary>
        /// UserManager instance
        /// </summary>
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        /// <summary>
        /// GET request action for Index page
        /// </summary>
        public ActionResult Index()
        {
            List<ApplicationUser> users = UserManager.Users.ToList();
            List<UsersViewModel> models = new List<UsersViewModel>();
            foreach(ApplicationUser user in users)
            {
                models.Add(new UsersViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    Role = user.Roles.First().ToString(),
                    UserName = user.UserName
                });
            }
            return View(models);
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
            ApplicationUser user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        /// <summary>
        /// GET request action for Create page
        /// </summary>
        public ActionResult Create()
        {
            UsersCreateViewModel model = new UsersCreateViewModel()
            {
                Roles = GetRoleSelectListItems()
            };
            return View(model);
        }

        /// <summary>
        /// POST request action for Create page
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsersCreateViewModel newUser)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { 
                    UserName = newUser.Email, 
                    Email = newUser.Email,
                    Name = newUser.Name,
                    PhoneNumber = newUser.PhoneNumber,
                };
                var result = UserManager.Create(user, newUser.Password);
                if (result.Succeeded)
                {
                    var currentUser = UserManager.FindByEmail(user.Email);
                    //Assigning the role
                    switch (newUser.Role) 
                    {
                        case "Admin":
                            if(UserManager.IsInRole(currentUser.Id, "Visitor"))
                            {
                                UserManager.RemoveFromRole(currentUser.Id, "Visitor");
                            }
                            UserManager.AddToRole(currentUser.Id, "Admin");
                            break;
                        case "Visitor":
                        default:
                            if (UserManager.IsInRole(currentUser.Id, "Admin"))
                            {
                                UserManager.RemoveFromRole(currentUser.Id, "Admin");
                            }
                            UserManager.AddToRole(currentUser.Id, "Visitor");
                            break;
                    }
                    string securityCode = UserManager.GenerateEmailConfirmationToken(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Accounts", new { userId = user.Id, code = securityCode }, protocol: Request.Url.Scheme);
                    UserManager.SendRegisterEmailTo(user, callbackUrl);
                    return RedirectToAction("Index");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(newUser);
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
            ApplicationUser user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            UsersViewModel model = new UsersViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Role = user.Roles.First().ToString(),
                UserName = user.UserName,
                Roles = GetRoleSelectListItems(user.Roles.First().ToString())
            };
            return View(model);
        }

        /// <summary>
        /// POST request action for Edit page
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UsersViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = UserManager.FindById(model.Id);
                user.Name = model.Name;
                if(user.PhoneNumber != model.PhoneNumber)
                {
                    user.PhoneNumber = model.PhoneNumber;
                }
                UserManager.Update(user);
                if (user.Email != model.Email)
                {
                    user.UserName = model.UserName;
                    UserManager.SetEmail(user.Id, model.Email);
                    string securityCode = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Accounts", new { userId = user.Id, code = securityCode }, protocol: Request.Url.Scheme);
                    UserManager.SendChangedEmailEmailTo(user, callbackUrl);
                }
                //Updating the role
                switch (model.Role)
                {
                    case "Admin":
                        if (UserManager.IsInRole(user.Id, "Visitor"))
                        {
                            UserManager.RemoveFromRole(user.Id, "Visitor");
                        }
                        UserManager.AddToRole(user.Id, "Admin");
                        break;
                    case "Visitor":
                    default:
                        if (UserManager.IsInRole(user.Id, "Admin"))
                        {
                            UserManager.RemoveFromRole(user.Id, "Admin");
                        }
                        UserManager.AddToRole(user.Id, "Visitor");
                        break;
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
            ApplicationUser user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            UsersViewModel model = new UsersViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Role = user.Roles.First().ToString(),
                UserName = user.UserName
            };
            return View(model);
        }

        /// <summary>
        /// POST request action for DeleteConfirmed page
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser user = UserManager.FindById(id);
            //The role is automatically removed by the UserManager
            UserManager.Delete(user);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method for disposing UserManager objects
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Returns List of items for dropdown with roles
        /// </summary>
        public List<SelectListItem> GetRoleSelectListItems(string selectedRole = null)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<IdentityRole> roles = db.Roles.ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (IdentityRole role in roles)
            {
                if(selectedRole == null)
                {
                    items.Add(new SelectListItem()
                    {
                        Value = role.Id,
                        Text = role.Name
                    });
                }
                else
                {
                    items.Add(new SelectListItem()
                    {
                        Value = role.Id,
                        Text = role.Name,
                        Selected = (selectedRole == role.Name)? true : false
                    });
                }
            }
            return items;
        }
    }
}
