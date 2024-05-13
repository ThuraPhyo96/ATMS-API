using ATMS.Web.Dto.Helpers;
using ATMS.Web.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ATMS.Web.Dto.Models;

namespace ATMS.Web.Dto.Dtos
{
    public class ATMLocationDto : ResponseDto
    {
        public int ATMLocationId { get; set; }

        public int BankNameId { get; set; }
        public BankName BankName { get; set; }

        public int? BankBranchNameId { get; set; }
        public BankBranchName BankBranchName { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }

        public int DivisionId { get; set; }
        public Division Division { get; set; }

        public int TownshipId { get; set; }
        public Township Township { get; set; }

        [StringLength(MaxLength.L_1000)]
        public string Address { get; set; }

        public int Status { get; set; }
    }

    public class ATMLocationListDto : ResponseDto
    {
        public List<ATMLocationDto> Data { get; set; } = new List<ATMLocationDto>();
    }

    public class ATMLocationListResponseDto : ResponseDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public bool IsEndOfPage => PageNumber == PageCount;
        public List<ATMLocationDto> Data { get; set; } = new List<ATMLocationDto>();
    }
}
