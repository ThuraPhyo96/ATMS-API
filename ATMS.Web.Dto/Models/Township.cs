using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ATMS.Web.Dto.Models
{
    public class Township
    {
        public int TownshipId { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }

        public int DivisionId { get; set; }
        public Division Division { get; set; }

        [StringLength(MaxLength.L_100)]
        public string Name { get; set; }

        [StringLength(MaxLength.L_500)]
        public string Description { get; set; }

        public int Sort { get; set; }

        public Township()
        {
                
        }

        public Township(int regionId, int divisionId, string name, string description, int sort)
        {
            RegionId = regionId;
            DivisionId = divisionId;
            Name = name;
            Description = description;
            Sort = sort;
        }
    }
}
