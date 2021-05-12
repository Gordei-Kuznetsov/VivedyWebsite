using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    public class Rotation
    {
        public string RotationId { get; set; }
        public DateTime StartTime { get; set; }

        public string MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}