using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VivedyWebApp.Models
{
    public class Rotation
    {
        public string RotationId { get; set; }
        public Rotation StartTime { get; set; }
        public string MovieId { get; set; }
    }
}