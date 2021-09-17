using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VivedyWebApp.Areas.Admin.Models.ViewModels
{
    /// <summary>
    /// Model specificly used for creating new User on Admin/Users/Create page
    /// </summary>
    public class UsersCreateViewModel : BaseUsersViewModel
    {
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
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// Model which has all ApplicationUser model's fields plus the user role
    /// </summary>
    public class UsersViewModel : BaseUsersViewModel
    {
        /// <summary>
        /// User GUID
        /// </summary>
        [Required]
        [MaxLength(36)]
        [RegularExpression(@"^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$")]
        [Display(Name = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// Boolean indicating wether the email ahs been confirmaed
        /// </summary>
        [Required]
        [Display(Name = "Email Conmfirmed")]
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Boolean indicating wether the phone number has been confirmed
        /// </summary>
        [Required]
        [Display(Name = "Phone Number Confirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// User username
        /// </summary>
        [Display(Name = "User Name")]
        public string UserName { get; set; }
    }

    public class BaseUsersViewModel
    {
        /// <summary>
        /// User name
        /// </summary>
        [Required]
        [MaxLength(20)]
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
        /// Role assigned to the user
        /// </summary>
        [Required]
        [MaxLength(7)]
        [Display(Name = "Role")]
        public string Role { get; set; }
        public List<SelectListItem> Roles;
    }
}