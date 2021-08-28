using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VivedyWebApp.Models;

namespace VivedyWebApp.Areas.Admin.Models.ViewModels
{
    /// <summary>
    /// Model specificly used for creating new Screening on Admin/Screenings/Create page
    /// </summary>
    public class ScreeningsCreateViewModel : BaseScreeningViewModel
    {
        /// <summary>
        /// Boolean indicating whether a week of Screenings is auto-generated
        /// </summary>
        [Display(Name = "Auto-generate a week of screenings?")]
        public bool GenerateScreenings { get; set; }
    }

    /// <summary>
    /// Model used for editing screenings
    /// </summary>
    public class ScreeningsViewModel : BaseScreeningViewModel
    {
        /// <summary>
        /// Screening GUID
        /// </summary>
        [Required]
        [MaxLength(36)]
        [RegularExpression(@"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$")]
        [Display(Name = "Id")]
        public string Id { get; set; }
    }

    /// <summary>
    /// Model used as a base for the other screening view models
    /// </summary>
    public class BaseScreeningViewModel
    {
        /// <summary>
        /// Start day and time for the Screening
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// ID of the movie for which the Screenings is created
        /// </summary>
        [Required]
        [MaxLength(36)]
        [RegularExpression(@"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$")]
        [Display(Name = "Movie Id")]
        public string MovieId { get; set; }
        public List<SelectListItem> Movies = new List<SelectListItem>();

        /// <summary>
        /// ID of the room where the screening is happeneing
        /// </summary>
        [Required]
        [MaxLength(36)]
        [RegularExpression(@"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$")]
        [Display(Name = "Room Id")]
        public string RoomId { get; set; }
        public List<SelectListItem> Rooms = new List<SelectListItem>();
    }
}