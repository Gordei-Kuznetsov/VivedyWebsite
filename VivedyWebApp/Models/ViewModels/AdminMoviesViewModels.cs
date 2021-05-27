using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models.ViewModels
{
    public class AdminMoviesNewViewModels
    {
        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Rating")]
        [Required]
        public int Rating { get; set; }

        [Display(Name = "Category")]
        [Required]
        public string Category { get; set; }

        [Display(Name = "Description")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Duration")]
        [Required]
        public TimeSpan Duration { get; set; }

        [Display(Name = "Price")]
        [Required]
        public int Price { get; set; }

        [Required]
        [Url]
        public string TrailerUrl { get; set; }
    }
}