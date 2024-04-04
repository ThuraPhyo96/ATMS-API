using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ATMS.Web.Dto.Models
{
    public class Region
    {
        public int RegionId { get; set; }

        [StringLength(MaxLength.L_100)]
        public string Name { get; set; }

        [StringLength(MaxLength.L_500)]
        public string Description { get; set; }

        public int Sort { get; set; }

        public List<Division> Divisions { get; set; } = new List<Division>();

        public Region()
        {

        }

        public Region(string name, string description, int sort)
        {
            Name = name;
            Description = description;
            Sort = sort;
        }
    }
}
