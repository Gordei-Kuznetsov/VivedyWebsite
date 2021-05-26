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
        public Rotation SelectedRotation { get; set; }
        public List<int> Seats { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public Movie Movie { get; set; }
    }
}