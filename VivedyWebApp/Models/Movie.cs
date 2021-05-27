﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    public class Movie
    {
        [Key]
        public string MovieId { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public int Price { get; set; }
        public string TrailerUrl { get; set; }
    }
}