using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models.ViewModels
{
    /// <summary>
    /// Model specificly used for getting booking time on Movies/BookingTime page
    /// </summary>
    public class BookingTimeViewModel
    {
        /// <summary>
        /// Screenings available for the selected movie
        /// </summary>
        public List<ScreeningDetails> AvailableScreenings { get; set; }

        /// <summary>
        /// Movie selected for the booking
        /// </summary>
        public Movie Movie { get; set; }
    }

    public class ScreeningDetails
    {
        public string Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }
        public int BookedSeats { get; set; }
    }

    /// <summary>
    /// Model specificly used for getting booking seats on Movies/BookingSeats page
    /// </summary>
    public class BookingSeatsViewModel
    {
        /// <summary>
        /// ID of the selected Screening
        /// </summary>
        [Required]
        [MaxLength(36)]
        [RegularExpression(@"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$")]
        [Display(Name = "Selected Screening Id")]
        public string SelectedScreeningId { get; set; }

        /// <summary>
        /// Seats occupied for this Screening
        /// </summary>
        public List<int> OccupiedSeats { get; set; }

        /// <summary>
        /// Seats selected for the booking
        /// </summary>
        [Required]
        [MaxLength(64, ErrorMessage = "Sorry but you can book maximum 16 seats")]
        [Display(Name = "Selected Seats")]
        public string SelectedSeats { get; set; }

        /// <summary>
        /// Movie selected for the booking
        /// </summary>
        public Movie Movie { get; set; }
    }

    /// <summary>
    /// Model specificly used for getting booking payment details on Movies/BookingPay page
    /// </summary>
    public class BookingPayViewModel
    {
        /// <summary>
        /// ID of the selected Screening
        /// </summary>
        [Required]
        [MaxLength(36)]
        [RegularExpression(@"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$")]
        [Display(Name = "Selected Screening Id")]
        public string SelectedScreeningId { get; set; }

        /// <summary>
        /// Seats selected for the Screening
        /// </summary>
        [Required]
        [MaxLength(64, ErrorMessage = "Sorry but you can book maximum 16 seats")]
        [Display(Name = "Selected Seats")]
        public string SelectedSeats { get; set; }

        /// <summary>
        /// Email assotiated with the booking
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Card number for the payment
        /// </summary>
        [Required]
        [DataType(DataType.CreditCard)]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        /// <summary>
        /// Card CCV number for the payment
        /// </summary>
        [Required]
        [Display(Name = "CCV")]
        [MaxLength(3)]
        public string CCV { get; set; }

        /// <summary>
        /// Card expiry date for the payment
        /// </summary>
        [Required]
        [MaxLength(5)]
        [Display(Name = "Expiry Date")]
        public string ExpDate { get; set; }

        /// <summary>
        /// Card holder's name for the payment
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Display(Name = "Card Holder's Name")]
        public string CardHolder { get; set; }

        /// <summary>
        /// Total price for the booking
        /// </summary>
        public float TotalPrice { get; set; }

        /// <summary>
        /// Movie selected for the booking
        /// </summary>
        public Movie Movie { get; set; }
    }
}