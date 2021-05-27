using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models.ViewModels
{
    public class AdminRotationsViewModels
    {
        [Display(Name = "Start Time")]
        [Required]
        public Rotation StartTime { get; set; }

        [Required]
        public string MovieId { get; set; }
    }
}