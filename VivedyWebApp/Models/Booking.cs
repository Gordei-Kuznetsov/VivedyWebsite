using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    /// <summary>
    /// Booking model
    /// </summary>
    public class Booking
    {
        /// <summary>
        /// Booking GUID
        /// </summary>
        [Key]
        [Required]
        [Display(Name = "Booking Id")]
        public string BookingId { get; set; }

        /// <summary>
        /// Selected seats for the booking
        /// </summary>
        [Display(Name = "Seats")]
        [Required]
        public string Seats { get; set; }

        /// <summary>
        /// Date and time when the booking was created
        /// </summary>
        [Display(Name = "Creation Date")]
        [Required]
        public System.DateTime CreationDate { get; set; }

        /// <summary>
        /// Email assosiated with the booking
        /// </summary>
        [Display(Name = "User Email")]
        [Required]
        public string UserEmail { get; set; }

        /// <summary>
        /// Rotation that is booked
        /// </summary>
        [Display(Name = "Rotation Id")]
        [ForeignKey("Rotation")]
        [Required]
        public string RotationId { get; set; }
        public Rotation Rotation { get; set; }
    }
}