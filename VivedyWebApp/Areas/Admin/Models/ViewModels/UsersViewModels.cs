using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Areas.Admin.Models.ViewModels
{
    /// <summary>
    /// Model specificly used for creating new User on AdminUsers/Create page
    /// </summary>
    public class UsersCreateViewModel
    {
        /// <summary>
        /// User name
        /// </summary>
        [Required]
        [Display(Name = "User Name")]
        public string Name { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// User phone number
        /// </summary>
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// User passwsord
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
        /// Role assigned to the user
        /// </summary>
        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
    }

    /// <summary>
    /// Model which has all ApplicationUser model's fields plus the user role
    /// </summary>
    public class UsersViewModel
    {
        /// <summary>
        /// User GUID
        /// </summary>
        [Required]
        [Display(Name = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Boolean indicating wether the email ahs been confirmaed
        /// </summary>
        [Required]
        [Display(Name = "Email Conmfirmed")]
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// User phone number
        /// </summary>
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Boolean indicating wether the phone number has been confirmed
        /// </summary>
        [Required]
        [Display(Name = "Phone Number Confirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Role assigned to user
        /// </summary>
        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }

        /// <summary>
        /// User username
        /// </summary>
        [Required]
        [Compare("Email", ErrorMessage = "The Email and User Name do not match.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
    }
}