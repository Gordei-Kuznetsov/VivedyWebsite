using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    public class Movie
    {
        public string MovieId { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public int Duration { get; set; }
        public int Price { get; set; }
        public string TrailerUrl { get; set; }
    }
}