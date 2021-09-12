using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    /// <summary>
    /// Movie model
    /// </summary>
    public class Movie : BaseModel
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
        [Range(0,30)]
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
        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// Movie closing date
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Closing Date")]
        public DateTime ClosingDate { get; set; }
    }
}