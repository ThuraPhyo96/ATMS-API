using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;

namespace ATMS.Web.Dto.Models
{
    public class Division
    {
        public int DivisionId { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }

        [StringLength(MaxLength.L_100)]
        public string Name { get; set; }

        [StringLength(MaxLength.L_500)]
        public string Description { get; set; }

        public int Sort { get; set; }

        public List<Township> Townships { get; set; } = new List<Township>();

        public Division()
        {
                
        }

        public Division(int regionId, string name, string description, int sort)
        {
            RegionId = regionId;
            Name = name;
            Description = description;
            Sort = sort;
        }
    }
}
