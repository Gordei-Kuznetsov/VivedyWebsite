using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models.ViewModels
{
    /// <summary>
    /// Model specificly used for creating new Rotation on AdminRotations/Create page
    /// </summary>
    public class AdminRotationsCreateViewModel
    {
        /// <summary>
        /// Start day and time for the rotation
        /// </summary>
        [Display(Name = "Start Day and Time")]
        [Required]
        public System.DateTime StartTime { get; set; }

        /// <summary>
        /// ID of the movie for which the rotations is created
        /// </summary>
        [Display(Name = "Movie Id")]
        [ForeignKey("Movie")]
        [Required]
        public string MovieId { get; set; }
        public Movie Movie { get; set; }

        /// <summary>
        /// Boolean indicating whether a week of rotations is auto-generated
        /// </summary>
        [Display(Name = "Auto-generate a week of rotations?")]
        public bool GenerateRotations { get; set; }
    }
}