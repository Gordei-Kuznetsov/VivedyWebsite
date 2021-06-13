using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    /// <summary>
    /// Rotation model
    /// </summary>
    public class Rotation
    {
        /// <summary>
        /// Rotation GIUD
        /// </summary>
        [Display(Name = "Rotaion Id")]
        [Key]
        [Required]
        public string RotationId { get; set; }

        /// <summary>
        /// Rotation start date and time
        /// </summary>
        [Display(Name = "Start Time")]
        [Required]
        public System.DateTime StartTime { get; set; }

        /// <summary>
        /// ID of the movie for which the rotations is created
        /// </summary>
        [Display(Name = "Movie Id")]
        [ForeignKey ("Movie")]
        [Required]
        public string MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}