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
        public async Task<ActionResult> Index()
        {
            List<ApplicationUser> users = await UserManager.Users.ToListAsync();
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
                    Role = UserManager.GetRoleName(user.Roles.First().RoleId),
                    UserName = user.UserName
                });
            }
            return View(models);
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
            ApplicationUser user = await UserManager.FindByIdAsync(id);
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
                Role = UserManager.GetRoleName(user.Roles.First().RoleId),
                UserName = user.UserName
            };
            return View(model);
        }

        /// <summary>
        /// GET request action for Create page
        /// </summary>
        public async Task<ActionResult> Create()
        {
            UsersCreateViewModel model = new UsersCreateViewModel()
            {
                Roles = await UserManager.GetRoleSelectListItems()
            };
            return View(model);
        }

        /// <summary>
        /// POST request action for Create page
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UsersCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { 
                    UserName = model.Email, 
                    Email = model.Email,
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var currentUser = await UserManager.FindByEmailAsync(user.Email);
                    //Assigning the role
                    switch (model.Role) 
                    {
                        case "Admin":
                            if(await UserManager.IsInRoleAsync(currentUser.Id, "Visitor"))
                            {
                                await UserManager.RemoveFromRoleAsync(currentUser.Id, "Visitor");
                            }
                            await UserManager.AddToRoleAsync(currentUser.Id, "Admin");
                            break;
                        case "Visitor":
                        default:
                            if (await UserManager.IsInRoleAsync(currentUser.Id, "Admin"))
                            {
                                await UserManager.RemoveFromRoleAsync(currentUser.Id, "Admin");
                            }
                            await UserManager.AddToRoleAsync(currentUser.Id, "Visitor");
                            break;
                    }
                    string securityCode = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Accounts", new { userId = user.Id, code = securityCode }, protocol: Request.Url.Scheme);
                    UserManager.SendRegisterEmailTo(user, callbackUrl);
                    return RedirectToAction("Index");
                }
            }
            // If we got this far, something failed, redisplay form
            model.Roles = await UserManager.GetRoleSelectListItems(model.Role);
            return View(model);
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
            ApplicationUser user = await UserManager.FindByIdAsync(id);
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
                Roles = await UserManager.GetRoleSelectListItems(UserManager.GetRoleName(user.Roles.First().RoleId))
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
                ApplicationUser user = await UserManager.FindByIdAsync(model.Id);
                user.Name = model.Name;
                if(user.PhoneNumber != model.PhoneNumber)
                {
                    user.PhoneNumber = model.PhoneNumber;
                }
                await UserManager.UpdateAsync(user);
                if (user.Email != model.Email)
                {
                    user.UserName = model.UserName;
                    await UserManager.SetEmailAsync(user.Id, model.Email);
                    string securityCode = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Accounts", new { userId = user.Id, code = securityCode }, protocol: Request.Url.Scheme);
                    UserManager.SendChangedEmailEmailTo(user, callbackUrl);
                }
                //Updating the role
                switch (model.Role)
                {
                    case "Admin":
                        if (await UserManager.IsInRoleAsync(user.Id, "Visitor"))
                        {
                            await UserManager.RemoveFromRoleAsync(user.Id, "Visitor");
                        }
                        await UserManager.AddToRoleAsync(user.Id, "Admin");
                        break;
                    case "Visitor":
                    default:
                        if (await UserManager.IsInRoleAsync(user.Id, "Admin"))
                        {
                            await UserManager.RemoveFromRoleAsync(user.Id, "Admin");
                        }
                        await UserManager.AddToRoleAsync(user.Id, "Visitor");
                        break;
                }
                return RedirectToAction("Index");
            }
            else
            {
                ApplicationUser user = await UserManager.FindByIdAsync(model.Id);
                model.Roles = await UserManager.GetRoleSelectListItems(model.Role);
                return View(model);
            }
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
            ApplicationUser user = await UserManager.FindByIdAsync(id);
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
                Role = UserManager.GetRoleName(user.Roles.First().RoleId),
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
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            //The role is automatically removed by the UserManager
            await UserManager.DeleteAsync(user);
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
    }
}
