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
    public class Room : BaseModel
    {
        /// <summary>
        /// Room name
        /// </summary>
        [Required]
        [MaxLength(16)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Layout of seats in the room
        /// </summary>
        [Required]
        [MaxLength(16)]
        [Display(Name = "Seats Layout")]
        public string SeatsLayout { get; set; }

        /// <summary>
        /// ID of the cinema where the room is located
        /// </summary>
        [ForeignKey("Cinema")]
        [Required]
        [MaxLength(36)]
        [RegularExpression(@"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$")]
        [Display(Name = "Cinema")]
        public string CinemaId { get; set; }
        public Cinema Cinema { get; set; }
    }
}