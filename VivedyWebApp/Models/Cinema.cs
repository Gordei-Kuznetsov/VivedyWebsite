using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    public class Cinema
    {
        [Display(Name = "Cinema Id")]
        [Key]
        [Required]
        public string CinemaId { get; set; }

        [Display(Name = "Cinema Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Cinema Address")]
        [Required]
        public string Address { get; set; }

        [Display(Name = "Seats Layout")]
        [Required]
        public string SeatsLayout { get; set; }
    }
}