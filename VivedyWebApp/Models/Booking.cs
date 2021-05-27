using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    public class Booking
    {
        [Required]
        public string BookingId { get; set; }

        [Display (Name = "Seats")]
        [Required]
        public string Seats { get; set; }

        [Display(Name = "Date Created")]
        [Required]
        public System.DateTime CreationDate { get; set; }

        [Required]
        public string RotationId { get; set; }

        [EmailAddress]
        [Required]
        public string UserEmail { get; set; }
    }
}