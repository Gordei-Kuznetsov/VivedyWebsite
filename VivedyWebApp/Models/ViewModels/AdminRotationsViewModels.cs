using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models.ViewModels
{
    public class AdminRotationsViewModel
    {
        [Display(Name = "Start Day and Time")]
        [Required]
        public System.DateTime StartTime { get; set; }

        [Display(Name = "Movie Id")]
        [ForeignKey("Movie")]
        [Required]
        public string MovieId { get; set; }
        public Movie Movie { get; set; }

        [Display(Name = "Auto-generate a week of rotations?")]
        public bool GenerateRotations { get; set; }

        [Display(Name = "First day for the week and time")]
        public System.DateTime StartDay { get; set; }
    }
}