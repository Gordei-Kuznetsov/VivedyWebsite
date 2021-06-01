using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    [DisplayColumn("StartTime")]
    public class Rotation
    {
        [Display(Name = "Rotaion Id")]
        [Key]
        [Required]
        public string RotationId { get; set; }

        [Display(Name = "Start Time")]
        [Required]
        public System.DateTime StartTime { get; set; }

        [Display(Name = "Movie Id")]
        [ForeignKey ("Movie")]
        [Required]
        public string MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}