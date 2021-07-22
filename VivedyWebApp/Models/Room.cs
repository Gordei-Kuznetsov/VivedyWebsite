using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    /// <summary>
    /// Room model
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Room GUID
        /// </summary>
        [Display(Name = "Room Id")]
        [Key]
        [Required]
        public string RoomId { get; set; }

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