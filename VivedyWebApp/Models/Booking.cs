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
        [Key]
        [Required]
        public string BookingId { get; set; }
        [Required]
        public string Seats { get; set; }
        [Required]
        public System.DateTime CreationDate { get; set; }

        [Required]
        public string UserEmail { get; set; }

        [ForeignKey("Rotation")]
        [Required]
        public string RotationId { get; set; }
        public Rotation Rotation { get; set; }
    }
}