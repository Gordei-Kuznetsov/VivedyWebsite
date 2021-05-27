using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    public class Rotation
    {
        [Key]
        [Required]
        public string RotationId { get; set; }

        [Display(Name = "Start Time")]
        [Required]
        public Rotation StartTime { get; set; }

        [ForeignKey ("Movie")]
        [Required]
        public string MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}