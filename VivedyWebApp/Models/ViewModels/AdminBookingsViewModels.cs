using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models.ViewModels
{
    public class AdminBookingsNewViewModels
    {
        [Required]
        public string Seats { get; set; }
        [Required]
        public string RotationId { get; set; }
        [Required]
        public string UserEmail { get; set; }
    }
}