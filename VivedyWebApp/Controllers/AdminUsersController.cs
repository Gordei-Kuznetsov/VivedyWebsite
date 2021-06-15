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
    /// <summary>
    /// Application Admin Controller for ApplicationUsers
    /// </summary>
    public class AdminUsersController : Controller
    {
        private ApplicationUserManager _userManager;

        /// <summary>
        /// UserManager instance
        /// </summary>
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
                     //Getting the role for each user
                     Role = UserManager.GetRoles(user.Id).First(),
                     PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                     PhoneNumber  = user.PhoneNumber,
                     EmailConfirmed = user.EmailConfirmed,
                     Email = user.Email
                });
            };
            return View(allViewModels);
        }

        /// <summary>
        /// GET request action for Details page
        /// </summary>
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
                //Updating the role
                Role = UserManager.GetRoles(applicationUser.Id).First(),
                PhoneNumberConfirmed = applicationUser.PhoneNumberConfirmed,
                PhoneNumber = applicationUser.PhoneNumber,
                EmailConfirmed = applicationUser.EmailConfirmed,
                Email = applicationUser.Email
            };
            return View(model);
        }

        /// <summary>
        /// GET request action for Create page
        /// </summary>
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST request action for Create page
        /// </summary>
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
                    //Assigning the role
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

                    //Sending the email to the user to confirm the email address
                    //Will later be moved to the a method of EmailService class
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Accounts", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    string subject = "Email Confirmation";
                    string mailbody = "<b>Hi " + user.Name + "</b><br/>Thank you for regestering on our website. Please follow the <a href=\"" + @callbackUrl + "\">link<a/> to confirm your email address.";
                    EmailService mailService = new EmailService();
                    await mailService.SendAsync(user.Email, subject, mailbody);
                    return RedirectToAction("Index");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(newUser);
        }

        /// <summary>
        /// GET request action for Edit page
        /// </summary>
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
                //Getting the role
                Role = UserManager.GetRoles(applicationUser.Id).First(),
                PhoneNumberConfirmed = applicationUser.PhoneNumberConfirmed,
                PhoneNumber = applicationUser.PhoneNumber,
                EmailConfirmed = applicationUser.EmailConfirmed,
                Email = applicationUser.Email
            };
            return View(model);
        }

        /// <summary>
        /// POST request action for Edit page
        /// </summary>
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
                //Updating the role
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

        /// <summary>
        /// GET request action for Delete page
        /// </summary>
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
                //Getting the role
                Role = UserManager.GetRoles(applicationUser.Id).First(),
                PhoneNumberConfirmed = applicationUser.PhoneNumberConfirmed,
                PhoneNumber = applicationUser.PhoneNumber,
                EmailConfirmed = applicationUser.EmailConfirmed,
                Email = applicationUser.Email
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
            ApplicationUser applicationUser = await UserManager.FindByIdAsync(id);
            //The role is automatically removed by the UserManager
            await UserManager.DeleteAsync(applicationUser);
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
