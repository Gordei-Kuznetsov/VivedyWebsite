using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using VivedyWebApp.Models;

namespace VivedyWebApp.Areas.Admin.Models.ViewModels
{
    /// <summary>
    /// Model specificly used for creating new Room on Admin/Rooms/Create page
    /// </summary>
    public class RoomsCreateViewModel
    {
        /// <summary>
        /// Room name
        /// </summary>
        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Layout of seats in the room
        /// </summary>
        [Display(Name = "Seats Layout")]
        [Required]
        public string SeatsLayout { get; set; }

        /// <summary>
        /// ID of the cinema where the room is located
        /// </summary>
        [Display(Name = "Cinema Id")]
        [ForeignKey("Cinema")]
        [Required]
        public string CinemaId { get; set; }
        public Cinema Cinema { get; set; }
    }
}