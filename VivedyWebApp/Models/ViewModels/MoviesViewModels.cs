using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    public class MoviesBookingViewModel
    {
        public List<Rotation> AvailableRotations { get; set; }

        [Required]
        public Rotation SelectedRotation { get; set; }

        [Required]
        public List<int> Seats { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Movie Movie { get; set; }
    }
}