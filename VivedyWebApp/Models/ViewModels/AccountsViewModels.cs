using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;

namespace VivedyWebApp.Models
{
    /// <summary>
    /// Model used on Login page
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// User email
        /// </summary>
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Boolean indicating whether the user has to be remembered
        /// </summary>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// Model used on Register page
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Name to register the user under
        /// </summary>
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Email to register the user under
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Phone number to register the user with
        /// </summary>
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Password to register the user with
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Password confirmation field
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// Model used on ForgotPassword page
    /// </summary>
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// Email of the user
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    /// <summary>
    /// Model used on ResetPassword page
    /// </summary>
    public class ResetPasswordViewModel
    {
        /// <summary>
        /// Email of the user to reset password for
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// New password for the user
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Password confirmation field
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Code sent with the model
        /// </summary>
        public string Code { get; set; }
    }

    /// <summary>
    /// Model used on Account page
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Boolean indicating wether the browser that passed the verification is remembered
        /// </summary>
        public bool BrowserRemembered { get; set; }
    }

    /// <summary>
    /// Model used on ChangeEmail page
    /// </summary>
    public class ChangeEmailViewModel
    {
        /// <summary>
        /// User's old email
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Current email")]
        public string OldEmail { get; set; }

        /// <summary>
        /// User's new email
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "New email")]
        public string NewEmail { get; set; }

        /// <summary>
        /// Email confirmation field
        /// </summary>
        [EmailAddress]
        [Display(Name = "Confirm new email")]
        [Compare("NewEmail", ErrorMessage = "The new email and confirmation email do not match.")]
        public string ConfirmEmail { get; set; }
    }

    /// <summary>
    /// Model used on ChangePassword page
    /// </summary>
    public class ChangePasswordViewModel
    {
        /// <summary>
        /// User's old password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        /// <summary>
        /// User's new password
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Password confirmation field
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
