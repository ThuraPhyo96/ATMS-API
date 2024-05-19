using ATMS.Web.Dto.Models;
using System.ComponentModel.DataAnnotations;

namespace ATMS.Web.Dto.Dtos
{
    public class TownshipDto
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
    }

    public class CreateTownshipDto
    {
        public int RegionId { get; set; }

        public int DivisionId { get; set; }

        [StringLength(MaxLength.L_100)]
        public string Name { get; set; }

        [StringLength(MaxLength.L_500)]
        public string Description { get; set; }
    }

    public class TownshipResponseDto
    {
        public int TownshipId { get; set; }
        public string RegionName { get; set; }
        public string DivisionName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
