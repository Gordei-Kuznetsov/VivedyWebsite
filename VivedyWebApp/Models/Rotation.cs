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
        [Required]
        public string RotationId { get; set; }

        [Display(Name = "Start Time")]
        [Required]
        public Rotation StartTime { get; set; }

        [Required]
        public string MovieId { get; set; }
    }
}