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

namespace VivedyWebApp.Controllers
{
    public class AdminUsersController : Controller
    {

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

        // GET: AdminUsers
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            List<ApplicationUser> allUsers = await UserManager.Users.ToListAsync();
            List<AdminUsersViewModel> allViewModels = new List<AdminUsersViewModel>();
            foreach(ApplicationUser user in allUsers)
            {
                allViewModels.Add(new AdminUsersViewModel
                {
                     Name = user.Name,
                     UserName = user.UserName,
                     Id = user.Id,
                     Role = UserManager.GetRoles(user.Id).First(),
                     PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                     PhoneNumber  = user.PhoneNumber,
                     EmailConfirmed = user.EmailConfirmed,
                     Email = user.Email
                });
            };
            return View(allViewModels);
        }

        // GET: AdminUsers/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = await UserManager.FindByIdAsync(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            AdminUsersViewModel model = new AdminUsersViewModel
            {
                Name = applicationUser.Name,
                UserName = applicationUser.UserName,
                Id = applicationUser.Id,
                Role = UserManager.GetRoles(applicationUser.Id).First(),
                PhoneNumberConfirmed = applicationUser.PhoneNumberConfirmed,
                PhoneNumber = applicationUser.PhoneNumber,
                EmailConfirmed = applicationUser.EmailConfirmed,
                Email = applicationUser.Email
            };
            return View(model);
        }

        // GET: AdminUsers/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminUsers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(AdminUsersCreateViewModel newUser)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { 
                    UserName = newUser.Email, 
                    Email = newUser.Email,
                    Name = newUser.Name,
                    PhoneNumber = newUser.PhoneNumber,
                };
                var result = await UserManager.CreateAsync(user, newUser.Password);
                if (result.Succeeded)
                {
                    var currentUser = UserManager.FindByEmail(user.Email);
                    switch (newUser.Role) 
                    {
                        case "Admin":
                            UserManager.AddToRole(currentUser.Id, "Admin");
                            break;
                        case "Visitor":
                        default:
                            UserManager.AddToRole(currentUser.Id, "Visitor");
                            break;
                    }

                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Accounts", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    string subject = "Email Confirmation";
                    string mailbody = "Thank you for regestering on our website. Please follow the <a href=\"" + @callbackUrl + "\">link<a/> to confirm your email address.";
                    EmailService mailService = new EmailService();
                    await mailService.SendAsync(user.Email, subject, mailbody);
                    return RedirectToAction("Index");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(newUser);
        }

        // GET: AdminUsers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = await UserManager.FindByIdAsync(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            AdminUsersViewModel model = new AdminUsersViewModel
            {
                Name = applicationUser.Name,
                UserName = applicationUser.UserName,
                Id = applicationUser.Id,
                Role = UserManager.GetRoles(applicationUser.Id).First(),
                PhoneNumberConfirmed = applicationUser.PhoneNumberConfirmed,
                PhoneNumber = applicationUser.PhoneNumber,
                EmailConfirmed = applicationUser.EmailConfirmed,
                Email = applicationUser.Email
            };
            return View(model);
        }

        // POST: AdminUsers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(AdminUsersViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = UserManager.FindById(model.Id);
                applicationUser.Name = model.Name;
                applicationUser.UserName = model.UserName;
                UserManager.AddToRole(model.Id, model.Role);
                applicationUser.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
                applicationUser.PhoneNumber = model.PhoneNumber;
                applicationUser.EmailConfirmed = model.EmailConfirmed;
                applicationUser.Email = model.Email;
                await UserManager.UpdateAsync(applicationUser);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: AdminUsers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = await UserManager.FindByIdAsync(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            AdminUsersViewModel model = new AdminUsersViewModel
            {
                Name = applicationUser.Name,
                UserName = applicationUser.UserName,
                Id = applicationUser.Id,
                Role = UserManager.GetRoles(applicationUser.Id).First(),
                PhoneNumberConfirmed = applicationUser.PhoneNumberConfirmed,
                PhoneNumber = applicationUser.PhoneNumber,
                EmailConfirmed = applicationUser.EmailConfirmed,
                Email = applicationUser.Email
            };
            return View(model);
        }

        // POST: AdminUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = await UserManager.FindByIdAsync(id);
            await UserManager.DeleteAsync(applicationUser);
            return RedirectToAction("Index");
        }

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
