using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    public class Booking
    {
        public string BookingId { get; set; }
        public List<int> Seats { get; set; }
        public DateTime CreationDate { get; set; }
        public string MovieId { get; set; }
        public Movie Movie { get; set; }
        public string RotationId { get; set; }
        public Rotation Rotation { get; set; }
        public string UserId { get; set; }
        public  ApplicationUser User { get; set; }
    }
}