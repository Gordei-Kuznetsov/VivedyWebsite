﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models.ViewModels
{

    /// <summary>
    /// Model specificly used for creating new Booking on AdminBookings/Create page
    /// </summary>
    public class AdminBookingsNewViewModel
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
        /// Rotation that is booked
        /// </summary>
        [Display(Name = "Rotation Id")]
        [ForeignKey("Rotation")]
        [Required]
        public string RotationId { get; set; }
        public Rotation Rotation { get; set; }
    }
}