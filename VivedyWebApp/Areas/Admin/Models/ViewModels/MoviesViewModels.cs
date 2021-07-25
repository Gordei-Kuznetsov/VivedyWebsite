using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Areas.Admin.Models.ViewModels
{
    /// <summary>
    /// Model specificly used for creating new Movie on Admin/Movies/Create page
    /// </summary>
    public class MoviesCreateViewModel
    {
        /// <summary>
        /// Movie name
        /// </summary>
        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Movie rating
        /// </summary>
        [Display(Name = "Rating")]
        [Required]
        public int Rating { get; set; }

        /// <summary>
        /// Movie user rating
        /// </summary>
        [Display(Name = "User Rating")]
        [Range(1,5)]
        [Required]
        public int UserRating { get; set; }

        /// <summary>
        /// Movie category/genre
        /// </summary>
        [Display(Name = "Category")]
        [Required]
        public string Category { get; set; }

        /// <summary>
        /// Movie description
        /// </summary>
        [Display(Name = "Description")]
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Movie duration
        /// </summary>
        [Display(Name = "Duration")]
        [Required]
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Movie price
        /// </summary>
        [Display(Name = "Price")]
        [Required]
        public int Price { get; set; }

        /// <summary>
        /// Movie trailer url
        /// </summary>
        [Display(Name = "Trailer URL")]
        [Required]
        [Url]
        public string TrailerUrl { get; set; }

        /// <summary>
        /// Field used for uploading image for movie's horizontal poster to be used on Home and Movies pages
        /// <remarks>
        /// Only PNG images are accepted.
        /// </remarks>
        /// </summary>
        [Display(Name = "Horizontal Poster")]
        [DataType(DataType.Upload)]
        [Required]
        public HttpPostedFileBase HorizontalImage {get;set;}

        /// <summary>
        /// Field used for uploading image for movie's vertical poster to be used on Home and Booking pages
        /// <remarks>
        /// Only PNG images are accepted.
        /// </remarks>
        /// </summary>
        [Display(Name = "Vertical Poster")]
        [DataType(DataType.Upload)]
        [Required]
        public HttpPostedFileBase VerticalImage { get; set; }
    }

    /// <summary>
    /// Model which includes all Movie model's fields plus Vertical and Horizontal Image fileds
    /// </summary>
    public class MoviesViewModel
    {
        /// <summary>
        /// Movie GUID
        /// </summary>
        [Display(Name = "Movie Id")]
        [Required]
        public string MovieId { get; set; }

        /// <summary>
        /// Movie name
        /// </summary>
        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Movie rating
        /// </summary>
        [Display(Name = "Rating")]
        [Required]
        public int Rating { get; set; }

        /// <summary>
        /// Movie user rating
        /// </summary>
        [Display(Name = "User Rating")]
        [Required]
        public int UserRating { get; set; }

        /// <summary>
        /// Movie category
        /// </summary>
        [Display(Name = "Category")]
        [Required]
        public string Category { get; set; }

        /// <summary>
        /// Movie description
        /// </summary>
        [Display(Name = "Description")]
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Movie duration
        /// </summary>
        [Display(Name = "Duration")]
        [Required]
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Movie price
        /// </summary>
        [Display(Name = "Price")]
        [Required]
        public int Price { get; set; }

        /// <summary>
        /// Movie trailer url
        /// </summary>
        [Display(Name = "Trailer URL")]
        [Required]
        [Url]
        public string TrailerUrl { get; set; }

        /// <summary>
        /// Field used for uploading image for movie's horizontal poster
        /// </summary>
        [Display(Name = "Horizontal Poster")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase HorizontalImage { get; set; }

        /// <summary>
        /// Field used for uploading image for movie's vertical poster
        /// </summary>
        [Display(Name = "Vertical Poster")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase VerticalImage { get; set; }
    }
}