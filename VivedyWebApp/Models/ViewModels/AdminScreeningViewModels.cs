using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models.ViewModels
{
    /// <summary>
    /// Model specificly used for creating new Screening on AdminScreenings/Create page
    /// </summary>
    public class AdminScreeningsCreateViewModel
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
        /// Boolean indicating whether a week of Screenings is auto-generated
        /// </summary>
        [Display(Name = "Auto-generate a week of screenings?")]
        public bool GenerateScreenings { get; set; }
    }
}