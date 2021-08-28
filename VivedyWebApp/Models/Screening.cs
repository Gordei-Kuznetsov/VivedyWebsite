using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    /// <summary>
    /// Screening model
    /// </summary>
    public class Screening : BaseModel
    {
        /// <summary>
        /// Screening start date and time
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// ID of the movie for which the Screenings is created
        /// </summary>
        [ForeignKey ("Movie")]
        [Required]
        [MaxLength(36)]
        [RegularExpression(@"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$")]
        [Display(Name = "Movie Id")]
        public string MovieId { get; set; }
        public Movie Movie { get; set; }

        /// <summary>
        /// ID of the room where the screening is happeneing
        /// </summary>
        [ForeignKey("Room")]
        [Required]
        [MaxLength(36)]
        [RegularExpression(@"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$")]
        [Display(Name = "Room Id")]
        public string RoomId { get; set; }
        public Room Room { get; set; }
    }
}