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
    /// Model specificly used for creating new Cinemas on Admin/Cinemas/Create page
    /// </summary>
    public class CinemasCreateViewModel
    {
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
    }
}