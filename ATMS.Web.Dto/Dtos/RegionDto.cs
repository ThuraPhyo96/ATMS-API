using ATMS.Web.Dto.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ATMS.Web.Dto.Dtos
{
    public class RegionDto
    {
        public int RegionId { get; set; }

        [StringLength(MaxLength.L_100)]
        public string Name { get; set; }

        [StringLength(MaxLength.L_500)]
        public string Description { get; set; }

        public int Sort { get; set; }

        public List<DivisionDto> Divisions { get; set; } = new List<DivisionDto>();
    }
}
