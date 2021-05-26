using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models.ViewModels
{
    public class AdminMoviesNewViewModels
    {
        public string Name { get; set; }
        public int Rating { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public int Price { get; set; }
        public string TrailerUrl { get; set; }
    }
}