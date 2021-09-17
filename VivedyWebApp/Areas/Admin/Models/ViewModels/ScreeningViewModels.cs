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
    public class ScreeningsCreateViewModel
    {
        /// <summary>
        /// String for a json array of start dates for the Screenings
        /// </summary>
        [Required]
        [Display(Name = "Start Dates")]
        public string StartDates { get; set; }

        /// <summary>
        /// String for a json array of start times for the Screenings
        /// </summary>
        [Required]
        [Display(Name = "Start Times")]
        public string StartTimes { get; set; }

        /// <summary>
        /// ID of the movie for which the Screenings is created
        /// </summary>
        [Required]
        [MaxLength(36)]
        [RegularExpression(@"^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$")]
        [Display(Name = "Movie")]
        public string MovieId { get; set; }
        public List<SelectListItem> Movies;

        /// <summary>
        /// ID of the room where the screening is happeneing
        /// </summary>
        [Required]
        [MaxLength(36)]
        [RegularExpression(@"^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$")]
        [Display(Name = "Room")]
        public string RoomId { get; set; }
        public List<SelectListItem> Rooms;
    }

    /// <summary>
    /// Model used for editing screenings
    /// </summary>
    public class ScreeningsViewModel
    {
        /// <summary>
        /// Screening GUID
        /// </summary>
        [Required]
        [MaxLength(36)]
        [RegularExpression(@"^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$")]
        [Display(Name = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// Start date for the Screening
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Start time for the Screening
        /// </summary>
        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// ID of the movie for which the Screenings is created
        /// </summary>
        [Required]
        [MaxLength(36)]
        [RegularExpression(@"^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$")]
        [Display(Name = "Movie")]
        public string MovieId { get; set; }
        public List<SelectListItem> Movies;

        /// <summary>
        /// ID of the room where the screening is happeneing
        /// </summary>
        [Required]
        [MaxLength(36)]
        [RegularExpression(@"^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$")]
        [Display(Name = "Room")]
        public string RoomId { get; set; }
        public List<SelectListItem> Rooms;
    }
}