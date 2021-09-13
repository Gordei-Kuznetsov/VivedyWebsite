using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VivedyWebApp.Areas.Admin.Models.ViewModels
{
    public class HomeViewModel
    {
        public string ScreeningId { get; set; }
        public List<SelectListItem> Screenings;
    }
}