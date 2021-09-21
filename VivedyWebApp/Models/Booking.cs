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
    public class Booking : BaseModel
    {
        /// <summary>
        /// Selected seats for the booking
        /// </summary>
        [Required]
        [MaxLength(48)]
        [RegularExpression(@"^([A-Z]\d{1,2}){1,16}$")]
        [Display(Name = "Seats")]
        public string Seats { get; set; }
        public List<string> SeparateSeats { get; set; }

        /// <summary>
        /// Amount paid for the booking
        /// </summary>
        [Required]
        [Display(Name = "Payed Amout")]
        public float PayedAmout { get; set; }

        /// <summary>
        /// Email assosiated with the booking
        /// </summary>
        [Required]
        [EmailAddress]
        [MaxLength(64)]
        [Display(Name = "User Email")]
        public string UserEmail { get; set; }

        /// <summary>
        /// Datetime when the booking has been scanned and verified
        /// </summary>
        [DataType(DataType.DateTime)]
        [Display(Name = "Verification time")]
        public DateTime? VerificationTime { get; set; }

        /// <summary>
        /// Screening that is booked
        /// </summary>
        [ForeignKey("Screening")]
        [Required]
        [MaxLength(36)]
        [RegularExpression(@"^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$")]
        [Display(Name = "Screening")]
        public string ScreeningId { get; set; }
        public Screening Screening { get; set; }
    }
}