using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models.ViewModels
{
    public class AdminMoviesCreateViewModel
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

        [Display(Name = "Trailer URL")]
        [Required]
        [Url]
        public string TrailerUrl { get; set; }

        [Display(Name = "Horizontal Poster")]
        [DataType(DataType.Upload)]
        [Required]
        public HttpPostedFileBase HorizontalImage {get;set;}

        [Display(Name = "Vertical Poster")]
        [DataType(DataType.Upload)]
        [Required]
        public HttpPostedFileBase VerticalImage { get; set; }
    }

    public class AdminMoviesViewModel
    {
        [Display(Name = "Movie Id")]
        [Required]
        public string MovieId { get; set; }

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

        [Display(Name = "Trailer URL")]
        [Required]
        [Url]
        public string TrailerUrl { get; set; }

        [Display(Name = "Horizontal Poster")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase HorizontalImage { get; set; }

        [Display(Name = "Vertical Poster")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase VerticalImage { get; set; }
    }
}