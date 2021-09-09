using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using VivedyWebApp.Models;
using VivedyWebApp.Models.ViewModels;

namespace VivedyWebApp.Controllers
{
    /// <summary>
    /// Application Accounts Controller
    /// </summary>
    public class AccountsController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        /// <summary>
        /// The entities manager instance
        /// </summary>
        private readonly Entities Helper = new Entities();

        public AccountsController()
        {
        }

        public AccountsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        /// <summary>
        /// Manager object used to manipulate sign-in/-out procceses
        /// </summary>
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        /// <summary>
        /// Manager object used to manipulate ApplicationUser data
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
        /// GET request action for Login page
        /// </summary>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// POST request action for Login page
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //Handling situations when the user got to the Login page directly and there is no return url
            returnUrl = returnUrl ?? "/Home/Index";

            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            //Prohibiting the user from login in without them confirming their email first
            else if (user.EmailConfirmed == true)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }
            else
            {
                ViewBag.ErrorMessage = "You have not confirmed your email. Please check your email box.";
                return View("Error");
            }
        }

        /// <summary>
        /// GET request action for Register page
        /// </summary>
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// POST request action for Register page
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Name = model.Name, Email = model.Email, PhoneNumber = model.PhoneNumber };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var currentUser = await UserManager.FindByEmailAsync(user.Email);
                    await UserManager.AddToRoleAsync(currentUser.Id, "Visitor");
                    string securityCode = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Accounts", new { userId = user.Id, code = securityCode }, protocol: Request.Url.Scheme);
                    UserManager.SendRegisterEmailTo(currentUser, callbackUrl);
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// GET request action for ConfirmEmail page
        /// </summary>
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            //Confirming that the code is valid for the email
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        /// <summary>
        /// GET request action for ForgotPassword page
        /// </summary>
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// POST request action for ForgotPassword page
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !user.EmailConfirmed)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }
                //Sending email to the user with the link to ResetPassword page
                string securityCode = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Accounts", new { userId = user.Id, code = securityCode }, protocol: Request.Url.Scheme);
                UserManager.SendForgotPasswordEmailTo(user, callbackUrl);
                return RedirectToAction("ForgotPasswordConfirmation", "Accounts");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// GET request action for ForgotPasswordConfirmation page
        /// </summary>
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// GET request action for ResetPassword page
        /// </summary>
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        /// <summary>
        /// POST request action for ResetPassword page
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Accounts");
            }

            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Accounts");
            }
            AddErrors(result);
            return View();
        }

        /// <summary>
        /// GET request action for ResetPasswordConfirmation page
        /// </summary>
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// POST request action for ResetPasswordConfirmation page
        /// </summary>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// GET request action for Index page
        /// </summary>
        [Authorize]
        public async Task<ActionResult> Index(AccountsMessageId? message)
        {

            //Adding message to display on the Account page
            ViewBag.StatusMessage =
                message == AccountsMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == AccountsMessageId.ChangeEmailSuccess ? "Your email has been changed."
                : message == AccountsMessageId.Error ? "An error has occurred."
                : "";

            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            var model = new IndexViewModel
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Bookings = await Helper.Bookings.GetAllComingForUser(user.Email)
            };
            return View(model);
        }

        /// <summary>
        /// GET request action for Edit page
        /// </summary>
        [Authorize]
        public async Task<ActionResult> Edit()
        {
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            EditViewModel model = new EditViewModel()
            {
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };
            return View(model);
        }


        /// <summary>
        /// POST request action for Edit page
        /// </summary>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            user.Name = model.Name;
            IdentityResult result = await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return View("Index");
            }
            return View(model);
        }

        /// <summary>
        /// GET request action for Delete page
        /// </summary>
        [Authorize]
        public async Task<ActionResult> Delete()
        {
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            var model = new IndexViewModel
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            return View(model);
        }

        /// <summary>
        /// POST request action for Delete page
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteConfirmed()
        {
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //The role is automatically removed by the UserManager
            await UserManager.DeleteAsync(user);
            return RedirectToAction("Index", "Home");
        }


        /// <summary>
        /// GET request action for ChangeEmail page
        /// </summary>
        [Authorize]
        public ActionResult ChangeEmail()
        {
            return View();
        }

        /// <summary>
        /// POST request action for ChangeEmail page
        /// </summary>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                if (user.Email != model.OldEmail)
                {
                    return View("Error");
                }
                //Saving the changes to the email
                user.UserName = model.NewEmail;
                UserManager.SetEmail(user.Id, model.NewEmail);
                IdentityResult result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    //Sending email to the user confirming the email address change
                    string securityCode = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Accounts", new { userId = user.Id, code = securityCode }, protocol: Request.Url.Scheme);
                    UserManager.SendChangedEmailEmailTo(user, callbackUrl);
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    return RedirectToAction("Login", new { Message = AccountsMessageId.ChangeEmailSuccess });
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// GET request action for ChangePassword page
        /// </summary>
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// POST request action for ChangePassword page
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = User.Identity.GetUserId();
            var result = await UserManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    //Signing the user out so that they login with the new password
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                }
                return RedirectToAction("Login", new { Message = AccountsMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        /// <summary>
        /// Method for disposing UserManager and SignInManager objects
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

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        public enum AccountsMessageId
        {
            ChangePasswordSuccess,
            Error,
            ChangeEmailSuccess
        }
        #endregion
    }
}