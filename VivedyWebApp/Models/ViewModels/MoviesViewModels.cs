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

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^(?:(4[0 - 9]{12}(?:[0-9]{3})?)|(5[1-5] [0-9]{14})|(6(?:011|5[0-9]{2})[0-9]{12})|(3[47] [0-9]{13})|(3(?:0[0-5]|[68] [0-9])[0-9]{11})|((?:2131|1800|35[0-9]{3})[0-9]{11}))$")]
        public int CardNumber { get; set;}

        [Required]
        [RegularExpression("^\\d\\d\\d$")]
        public int CVC { get; set; }

        [Required]
        [RegularExpression("^\\d\\d[/,\\]\\d\\d$")]
        public string ExpDate { get; set; }

        [Required]
        public string CardHolder { get; set; }

        public Movie Movie { get; set; }
    }
}