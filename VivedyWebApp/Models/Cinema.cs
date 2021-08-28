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
    public class Cinema : BaseModel
    {
        /// <summary>
        /// Cinema Name
        /// </summary>
        [Required]
        [MaxLength(20)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Cinema physical address
        /// </summary>
        [Required]
        [MaxLength(80)]
        [Display(Name = "Address")]
        public string Address { get; set; }
    }
}