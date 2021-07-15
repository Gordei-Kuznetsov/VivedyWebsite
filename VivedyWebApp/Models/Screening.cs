using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    /// <summary>
    /// Screening model
    /// </summary>
    public class Screening
    {
        /// <summary>
        /// Screening GIUD
        /// </summary>
        [Display(Name = "Screening Id")]
        [Key]
        [Required]
        public string ScreeningId { get; set; }

        /// <summary>
        /// Screening start date and time
        /// </summary>
        [Display(Name = "Start Time")]
        [Required]
        public System.DateTime StartTime { get; set; }

        /// <summary>
        /// ID of the movie for which the Screenings is created
        /// </summary>
        [Display(Name = "Movie Id")]
        [ForeignKey ("Movie")]
        [Required]
        public string MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}