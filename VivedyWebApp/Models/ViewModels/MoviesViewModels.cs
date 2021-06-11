using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    public class MoviesBookingTimeViewModel
    {
        public List<Rotation> AvailableRotations { get; set; }

        [Required]
        public string SelectedRotationId { get; set; }

        public Movie Movie { get; set; }
    }

    public class MoviesBookingSeatsViewModel
    {
        [Display(Name = "Selected Rotation Id")]
        [Required]
        public string SelectedRotationId { get; set; }

        public List<int> OccupiedSeats { get; set; }

        [Display(Name = "Selected Seats")]
        [Required]
        [MinLength(2, ErrorMessage = "Please select a seat")]
        public string SelectedSeats { get; set; }

        public Movie Movie { get; set; }
    }
    public class MoviesBookingPayViewModel
    {
        [Display(Name = "Selected Rotation Id")]
        [Required]
        public string SelectedRotationId { get; set; }

        [Display(Name = "Selected Seats")]
        [Required]
        public string SelectedSeats { get; set; }

        [Display(Name = "Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Card Number")]
        [Required]
        [CreditCard]
        public string CardNumber { get; set;}

        [Display(Name = "CVC")]
        [Required]
        public int CVC { get; set; }

        [Display(Name = "Expiry Date")]
        [Required]
        public string ExpDate { get; set; }

        [Display(Name = "Card Holder's Name")]
        [Required]
        public string CardHolder { get; set; }

        public int TotalPrice { get; set; }
        public Movie Movie { get; set; }
    }
}