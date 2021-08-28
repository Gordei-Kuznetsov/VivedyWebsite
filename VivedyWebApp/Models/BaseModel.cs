using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    /// <summary>
    /// Base model class that is to be inherited by all models
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// Model GUID
        /// </summary>
        [Key]
        [Required]
        [MaxLength(36)]
        [RegularExpression(@"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$")]
        [Display(Name = "Id")]
        public string Id { get; set; }
    }
}