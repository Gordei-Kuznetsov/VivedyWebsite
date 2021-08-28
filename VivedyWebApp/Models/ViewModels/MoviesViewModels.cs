using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models.ViewModels
{
    public class MoviesViewModel
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