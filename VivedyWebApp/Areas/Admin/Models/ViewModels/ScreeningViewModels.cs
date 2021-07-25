using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using VivedyWebApp.Models;

namespace VivedyWebApp.Areas.Admin.Models.ViewModels
{
    /// <summary>
    /// Model specificly used for creating new Screening on Admin/Screenings/Create page
    /// </summary>
    public class ScreeningsCreateViewModel
    {
        /// <summary>
        /// Start day and time for the Screening
        /// </summary>
        [Display(Name = "Start Day and Time")]
        [Required]
        public System.DateTime StartTime { get; set; }

        /// <summary>
        /// ID of the movie for which the Screening is created
        /// </summary>
        [Display(Name = "Movie Id")]
        [ForeignKey("Movie")]
        [Required]
        public string MovieId { get; set; }
        public Movie Movie { get; set; }

        /// <summary>
        /// ID of the room where the screening is happening
        /// </summary>
        [Display(Name = "Room Id")]
        [ForeignKey("Room")]
        [Required]
        public string RoomId { get; set; }
        public Room Room { get; set; }

        /// <summary>
        /// Boolean indicating whether a week of Screenings is auto-generated
        /// </summary>
        [Display(Name = "Auto-generate a week of screenings?")]
        public bool GenerateScreenings { get; set; }
    }
}