using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ATM.Web.ViewModels
{
    public class TownshipViewModel
    {
        public int TownshipId { get; set; }

        public string RegionName { get; set; }

        public string DivisionName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class CreateTownshipViewModel
    {
        [Required]
        [DisplayName("Region")]
        public int RegionId { get; set; }

        [Required]
        [DisplayName("Division")]
        public int DivisionId { get; set; }

        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Sort { get; set; }

        public List<SelectListItem> RegionNames { get; set; }
    }
}
