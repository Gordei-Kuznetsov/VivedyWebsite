using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace VivedyWebApp.Areas.Admin.Models.ViewModels
{
    public class RoomsCreateViewModel
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
        public List<SelectListItem> SeatsLayouts;

        /// <summary>
        /// ID of the cinema where the room is located
        /// </summary>
        [Required]
        [MaxLength(36)]
        [RegularExpression(@"^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$")]
        [Display(Name = "Cinema")]
        public string CinemaId { get; set; }
        public List<SelectListItem> Cinemas;
    }

    public class RoomsViewModel : RoomsCreateViewModel
    {
        /// <summary>
        /// Room GUID
        /// </summary>
        [Required]
        [MaxLength(36)]
        [RegularExpression(@"^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$")]
        public string Id { get; set; }
    }
}