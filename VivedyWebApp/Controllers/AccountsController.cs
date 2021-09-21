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
        public AccountsController()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Bookings = new BookingsManager(db);
        }

        public AccountsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly BookingsManager Bookings;

        /// <summary>
        /// Manager property used to manipulate sign-in/-out procceses
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
        /// Manager property used to manipulate ApplicationUser data
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
        public ActionResult Login(string returnUrl, string message = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Message = message;
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
                ViewBag.Message = Messages.Error;
                return View(model);
            }
            //Handling situations when the user got to the Login page directly and there is no return url
            returnUrl = returnUrl ?? "/Home/Index";

            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ViewBag.Message = Messages.InvalidLogin;
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
                        ViewBag.Message = Messages.LockedOut;
                        return View("Lockout");
                    case SignInStatus.Failure:
                    default:
                        ViewBag.Message = Messages.InvalidLogin;
                        return View(model);
                }
            }
            else
            {
                ViewBag.Message = Messages.UnconfirmedEmail;
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
            if (!ModelState.IsValid)
            {
                ViewBag.Message = Messages.Error;
                return View(model);
            }
            var user = new ApplicationUser { UserName = model.Email, Name = model.Name, Email = model.Email, PhoneNumber = model.PhoneNumber };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var currentUser = await UserManager.FindByEmailAsync(user.Email);
                await UserManager.AddToRoleAsync(currentUser.Id, "Visitor");
                string securityCode = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Accounts", new { userId = user.Id, code = securityCode }, protocol: Request.Url.Scheme);
                int result2 = await UserManager.SendRegisterEmailTo(currentUser, callbackUrl);
                if(result2 > 0)
                {
                    return RedirectToAction("Index", "Home", new { message = Messages.Registered });
                }
                else
                {
                    ViewBag.Message = Messages.FailedRegEmail;
                    return View("Error");
                }
            }
            ViewBag.Message = Messages.Error;
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
                ViewBag.Message = Messages.Error;
                return View("Error");
            }
            //Confirming that the code is valid for the email
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                return View();
            }
            else
            {
                ViewBag.Message = Messages.Error;
                return View("Error");
            }
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
            if (!ModelState.IsValid)
            {
                ViewBag.Message = Messages.Error;
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                ViewBag.Message = Messages.Error;
                return View("Error");
            }
            if (!user.EmailConfirmed)
            {
                ViewBag.Message = Messages.UnconfirmedEmail;
                return View(model);
            }
            //Sending email to the user with the link to ResetPassword page
            string securityCode = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Accounts", new { userId = user.Id, code = securityCode }, protocol: Request.Url.Scheme);
            int result = await UserManager.SendForgotPasswordEmailTo(user, callbackUrl);
            if(result > 0)
            {
                return RedirectToAction("ForgotPasswordConfirmation", "Accounts");
            }
            else
            {
                ViewBag.ErrorMesage = Messages.FailedForgPassEmail;
                return View("Error");
            }
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
            if(code == null)
            {
                ViewBag.Message = Messages.Error;
                return View("Error");
            }
            else
            {
                return View();
            }
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
                ViewBag.Message = Messages.Error;
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                ViewBag.Message = Messages.Error;
                return View("Error");
            }

            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Accounts");
            }
            ViewBag.Message = Messages.Error;
            return View(model);
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
        public async Task<ActionResult> Index(string message = null)
        {
            //Adding message to display on the Account page
            ViewBag.Message = message;

            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            var model = new IndexViewModel
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Bookings = await Bookings.AllComingForUser(user.Email)
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
                ViewBag.Message = Messages.Error;
                return View(model);
            }
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            user.Name = model.Name;
            var result = await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Accounts", new { message = Messages.ModifiedAcc });
            }
            ViewBag.Message = Messages.Error;
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
            var result = await UserManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home", new { message = Messages.DeletedAcc });
            }
            ViewBag.Message = Messages.Error;
            return View("Error");
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
                ViewBag.Message = Messages.Error;
                return View(model);
            }
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.Message = Messages.Error;
                return View("Error");
            }
            if (user.Email != model.OldEmail)
            {
                ViewBag.Message = Messages.OldEmailNotNew;
                return View(model);
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
                int result2 = await UserManager.SendChangedEmailEmailTo(user, callbackUrl);
                if(result2 > 0)
                {
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    return RedirectToAction("Login", new { message = Messages.ChangedEmail });
                }
                else
                {
                    ViewBag.Message = Messages.ChangeEmailFailedEmail;
                    return View("Error");
                }
            }
            else
            {
                ViewBag.Message = Messages.Error;
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
                ViewBag.Message = Messages.Error;
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
                return RedirectToAction("Login", new { message = Messages.ChangedPass });
            }
            ViewBag.Message = Messages.Error;
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
        #endregion
    }

    public partial class Messages
    {
        public static string InvalidLogin = "Invalid Login attempt.\nPlease Try again.";
        public static string LockedOut = "Your account has been locked out due to too many failed login attempts.";
        public static string UnconfirmedEmail= "You have not confirmed your email. Please check your email box.";
        public static string Registered = "You have successfuly registered. Please check your email box to verify the email address";
        public static string FailedRegEmail = "Something went wrong while sending your registration email.\nPlease contact our service desk.";
        public static string FailedForgPassEmail = "Something went wrong while sending the email with instructions on reviving the password.\nPlease try again, or contact our service desk.";
        public static string ModifiedAcc = "Your account details have been successfuly modified.";
        public static string DeletedAcc = "Your account has been successfuly deleted.";
        public static string OldEmailNotNew = "Provided old email is incorrect.\nPlease try again.";
        public static string ChangedEmail = "Your email address has been successfuly updated.\nPlease check your email box to verify the address";
        public static string ChangeEmailFailedEmail = "Something went wrong while sending your email with instruction on verifying your email address.\nPlease try again, or contact our service desk.";
        public static string ChangedPass = "Your password has been successfuly changed.\nPlease login to access your account.";
    }
}