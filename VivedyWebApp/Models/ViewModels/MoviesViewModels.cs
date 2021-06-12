using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    /// <summary>
    /// Model specificly used for getting booking time on Movies/BookingTime page
    /// </summary>
    public class MoviesBookingTimeViewModel
    {
        /// <summary>
        /// Rotations available for the selected movie
        /// </summary>
        public List<Rotation> AvailableRotations { get; set; }

        /// <summary>
        /// ID of the selected rotation
        /// </summary>
        [Required]
        public string SelectedRotationId { get; set; }

        /// <summary>
        /// Movie selected for the booking
        /// </summary>
        public Movie Movie { get; set; }
    }

    /// <summary>
    /// Model specificly used for getting booking seats on Movies/BookingSeats page
    /// </summary>
    public class MoviesBookingSeatsViewModel
    {
        /// <summary>
        /// ID of the selected rotation
        /// </summary>
        [Display(Name = "Selected Rotation Id")]
        [Required]
        public string SelectedRotationId { get; set; }

        /// <summary>
        /// Seats occupied for this rotation
        /// </summary>
        public List<int> OccupiedSeats { get; set; }

        /// <summary>
        /// Seats selected for the booking
        /// </summary>
        [Display(Name = "Selected Seats")]
        [Required(ErrorMessage = "Please select a seat")]
        public string SelectedSeats { get; set; }

        /// <summary>
        /// Movie selected for the booking
        /// </summary>
        public Movie Movie { get; set; }
    }

    /// <summary>
    /// Model specificly used for getting booking payment details on Movies/BookingPay page
    /// </summary>
    public class MoviesBookingPayViewModel
    {
        /// <summary>
        /// ID of the selected rotation
        /// </summary>
        [Display(Name = "Selected Rotation Id")]
        [Required]
        public string SelectedRotationId { get; set; }

        /// <summary>
        /// Seats selected for the booking
        /// </summary>
        [Display(Name = "Selected Seats")]
        [Required]
        public string SelectedSeats { get; set; }

        /// <summary>
        /// Email assotiated with the booking
        /// </summary>
        [Display(Name = "Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Card number for the payment
        /// </summary>
        [Display(Name = "Card Number")]
        [Required]
        [DataType(DataType.CreditCard)]
        public string CardNumber { get; set; }

        /// <summary>
        /// Card CCV number for the payment
        /// </summary>
        [Display(Name = "CCV")]
        [Required]
        public int CCV { get; set; }

        /// <summary>
        /// Card expiry date for the payment
        /// </summary>
        [Display(Name = "Expiry Date")]
        [Required]
        public string ExpDate { get; set; }

        /// <summary>
        /// Card holder's name for the payment
        /// </summary>
        [Display(Name = "Card Holder's Name")]
        [Required]
        public string CardHolder { get; set; }

        /// <summary>
        /// Total price for the booking
        /// </summary>
        public int TotalPrice { get; set; }

        /// <summary>
        /// Movie selected for the booking
        /// </summary>
        public Movie Movie { get; set; }
    }
}