using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VivedyWebApp.Models.ViewModels
{
    public class MoviesHomeViewModel
    {
        /// <summary>
        /// List of movies that are still to come
        /// </summary>
        public List<Movie> ComingSoonMovies { get; set; }

        /// <summary>
        /// List of top movies by rating
        /// </summary>
        public List<Movie> TopMovies { get; set; }
    }

    public class MoviesViewModel
    {
        /// <summary>
        /// List of all not closed movies
        /// </summary>
        public List<Movie> Movies { get; set; }

        /// <summary>
        /// Category of the movies to filter by
        /// </summary>
        public string Category { get; set; }
        public List<SelectListItem> Categories = new List<SelectListItem>();

        /// <summary>
        /// Rating of the movies to filter by
        /// </summary>
        public string Rating { get; set; }
        public List<SelectListItem> Ratings = new List<SelectListItem>();
    }

    public class MoviesDetailsViewModel
    {
        /// <summary>
        /// Movies property
        /// </summary>
        public Movie Movie { get; set; }

        /// <summary>
        /// List of cinemas where the movie is shown
        /// </summary>
        public List<Cinema> Cinemas { get; set; }
    }
}