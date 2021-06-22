using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    /// <summary>
    /// Cinema model
    /// </summary>
    public class Cinema
    {
        /// <summary>
        /// Cinema GUID
        /// </summary>
        [Display(Name = "Cinema Id")]
        [Key]
        [Required]
        public string CinemaId { get; set; }

        /// <summary>
        /// Cinema Name
        /// </summary>
        [Display(Name = "Cinema Name")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Cinema physical address
        /// </summary>
        [Display(Name = "Cinema Address")]
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// Layout of seats in the cinema
        /// </summary>
        [Display(Name = "Seats Layout")]
        [Required]
        public string SeatsLayout { get; set; }
    }
}