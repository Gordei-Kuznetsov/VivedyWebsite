using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using VivedyWebApp.Models;

namespace VivedyWebApp.Areas.Admin.Models.ViewModels
{

    /// <summary>
    /// Model specificly used for creating new Booking on AdminBookings/Create page
    /// </summary>
    public class BookingsNewViewModel
    {
        /// <summary>
        /// Seats that are booked
        /// </summary>
        [Display(Name = "Seats")]
        [Required]
        public string Seats { get; set; }

        /// <summary>
        /// Date and time when the booking was created
        /// </summary>
        [Display(Name = "Creation Date")]
        public System.DateTime CreationDate { get; set; }

        /// <summary>
        /// Email assosiated the booking
        /// </summary>
        [Display(Name = "User Email")]
        [Required]
        public string UserEmail { get; set; }

        /// <summary>
        /// Screening that is booked
        /// </summary>
        [Display(Name = "Screening Id")]
        [ForeignKey("Screening")]
        [Required]
        public string ScreeningId { get; set; }
        public Screening Screening { get; set; }
    }
}