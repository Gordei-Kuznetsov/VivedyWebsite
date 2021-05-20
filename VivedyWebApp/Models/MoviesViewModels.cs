using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    //public class MoviesRotationsViewModel
    //{
    //    public string RotationId { get; set; }
    //    public DateTime StartTime { get; set; }
    //}
    //public class MoviesSeatsViewModel
    //{
    //    public List<int> Seats { get; set; }
    //}
    public class MoviesBookingViewModel
    {
        public List<Rotation> Rotations { get; set; }
        public int Seat { get; set; }
        public Rotation PickedRotation { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}