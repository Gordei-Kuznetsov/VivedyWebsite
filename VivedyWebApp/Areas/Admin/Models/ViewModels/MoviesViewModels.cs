using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VivedyWebApp.Models;

namespace VivedyWebApp.Areas.Admin.Models.ViewModels
{
    /// <summary>
    /// Model specificly used for creating new Movie on Admin/Movies/Create page
    /// </summary>
    public class MoviesCreateViewModel : MoviesAdminBaseModel
    {
        /// <summary>
        /// Field used for uploading image for movie's horizontal poster to be used on Home and Movies pages
        /// <remarks>
        /// Only PNG images are accepted.
        /// </remarks>
        /// </summary>
        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Horizontal Poster")]
        public HttpPostedFileBase HorizontalImage {get;set;}

        /// <summary>
        /// Field used for uploading image for movie's vertical poster to be used on Home and Booking pages
        /// <remarks>
        /// Only PNG images are accepted.
        /// </remarks>
        /// </summary>
        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Vertical Poster")]
        public HttpPostedFileBase VerticalImage { get; set; }
    }

    /// <summary>
    /// Model which includes all Movie model's fields plus Vertical and Horizontal Image fileds
    /// </summary>
    public class MoviesAdminViewModel : MoviesAdminBaseModel
    {
        /// <summary>
        /// Movie GUID
        /// </summary>
        [Required]
        [MaxLength(36)]
        [RegularExpression(@"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$")]
        public string Id { get; set; }

        /// <summary>
        /// Field used for uploading image for movie's horizontal poster to be used on Home and Movies pages
        /// <remarks>
        /// Only PNG images are accepted.
        /// </remarks>
        /// </summary>
        [DataType(DataType.Upload)]
        [Display(Name = "Horizontal Poster")]
        public HttpPostedFileBase HorizontalImage { get; set; }

        /// <summary>
        /// Field used for uploading image for movie's vertical poster to be used on Home and Booking pages
        /// <remarks>
        /// Only PNG images are accepted.
        /// </remarks>
        /// </summary>
        [DataType(DataType.Upload)]
        [Display(Name = "Vertical Poster")]
        public HttpPostedFileBase VerticalImage { get; set; }
    }

    public class MoviesAdminBaseModel
    {
        /// <summary>
        /// Movie name
        /// </summary>
        [Required]
        [Display(Name = "Name")]
        [MaxLength(64)]
        public string Name { get; set; }

        /// <summary>
        /// Movie rating
        /// </summary>
        [Required]
        [Range(0, 21)]
        [Display(Name = "Rating")]
        public int Rating { get; set; }

        /// <summary>
        /// Movie user rating
        /// </summary>
        [Required]
        [Range(0, 5)]
        [Display(Name = "Viewer Rating")]
        public float ViewerRating { get; set; }

        /// <summary>
        /// Movie category/genre
        /// </summary>
        [Required]
        [MaxLength(16)]
        [Display(Name = "Category")]
        public string Category { get; set; }

        /// <summary>
        /// Movie description
        /// </summary>
        [Required]
        [MaxLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Movie duration
        /// </summary>
        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Duration")]
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Movie price
        /// </summary>
        [Required]
        [Range(0, 30)]
        [Display(Name = "Price")]
        public float Price { get; set; }

        /// <summary>
        /// Movie trailer url
        /// </summary>
        [Required]
        [Url]
        [MaxLength(41)]
        [RegularExpression(@"^https:\/\/(?:www\.)?youtube.com\/embed\/[A-z0-9]+")]
        [Display(Name = "Trailer URL")]
        public string TrailerUrl { get; set; }


        /// <summary>
        /// Movie release date
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// Movie closing date
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Closing Date")]
        public DateTime ClosingDate { get; set; }
    }
}