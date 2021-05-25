using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    public class MoviesBookingViewModel
    {
        public List<Rotation> Rotations { get; set; }
        public int Seat { get; set; }
        public Rotation PickedRotation { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }

}