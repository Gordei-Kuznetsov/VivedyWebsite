using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    public class MoviesRotationsViewModel
    {
        public string RotationId { get; set; }
        public DateTime StartTime { get; set; }
    }
    public class MoviesSeatsViewModel
    {
        public List<int> Seats { get; set; }
    }
}