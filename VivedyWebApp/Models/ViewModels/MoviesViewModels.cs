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
        public Rotation SelectedRotation { get; set; }

        public Movie Movie { get; set; }
    }

    public class MoviesBookingSeatsViewModel
    {
        public Rotation SelectedRotation { get; set; }

        public List<string> OccupiedSeats { get; set; }

        [Required]
        public string SelectedSeats { get; set; }

        public Movie Movie { get; set; }
    }
    public class MoviesBookingPayViewModel
    {
        public Rotation SelectedRotation { get; set; }

        public string SelectedSeats { get; set; }

        [Display(Name = "Emial")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Card Number")]
        [Required]
        [CreditCard]
        public int CardNumber { get; set;}

        [Display(Name = "CVC")]
        [Required]
        [RegularExpression("^\\d\\d\\d$")]
        public int CVC { get; set; }

        [Display(Name = "Expiry Date")]
        [Required]
        [RegularExpression("^\\d\\d[/,\\]\\d\\d$")]
        public string ExpDate { get; set; }

        [Display(Name = "Card Holder's Name")]
        [Required]
        public string CardHolder { get; set; }

        public Movie Movie { get; set; }
    }
}