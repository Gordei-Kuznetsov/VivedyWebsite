using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models.ViewModels
{
    public class AdminBookingsNewViewModel
    {
        [Display(Name = "Seats")]
        [Required]
        public string Seats { get; set; }

        [Display(Name = "Creation Date")]
        public System.DateTime CreationDate { get; set; }

        [Display(Name = "User Email")]
        [Required]
        public string UserEmail { get; set; }

        [Display(Name = "Rotation Id")]
        [ForeignKey("Rotation")]
        [Required]
        public string RotationId { get; set; }
        public Rotation Rotation { get; set; }
    }
}