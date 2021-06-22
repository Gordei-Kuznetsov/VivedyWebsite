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
    public class Movie
    {
        /// <summary>
        /// Movie GUID
        /// </summary>
        [Display(Name = "Movie Id")]
        [Key]
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

        /*[Display(Name = "Viewer Rating")]
        [Required]
        public string ViewerRating { get; set; }*/
    }
}