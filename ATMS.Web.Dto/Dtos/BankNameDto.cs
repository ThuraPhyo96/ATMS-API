using ATMS.Web.Dto.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ATMS.Web.Dto.Dtos
{
    public class BankNameDto
    {
        public int BankNameId { get; set; }

        [StringLength(MaxLength.L_100)]
        public string Name { get; set; }

        [StringLength(MaxLength.L_12)]
        public string Code { get; set; }

        [StringLength(MaxLength.L_20)]
        public string TelephoneNumber { get; set; }

        [StringLength(MaxLength.L_100)]
        public string Email { get; set; }

        [StringLength(MaxLength.L_500)]
        public string Address { get; set; }

        public bool IsActive { get; set; }

        public List<BankBranchNameDto> BankBranchNames { get; set; } = new List<BankBranchNameDto>();   
    }
}
