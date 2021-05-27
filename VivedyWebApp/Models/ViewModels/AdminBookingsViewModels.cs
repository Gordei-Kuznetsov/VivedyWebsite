using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models.ViewModels
{
    public class AdminBookingsNewViewModels
    {
        [Display(Name = "Seats")]
        [Required]
        public string Seats { get; set; }

        [Required]
        public string SelectedRotation { get; set; }

        [Display(Name = "Email")]
        [Required]
        public string UserEmail { get; set; }

        public List<Rotation> Rotations { get; set; }
    }
}