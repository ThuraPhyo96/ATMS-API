using System;
using System.Collections.Generic;
using System.Text;

namespace ATMS.Web.Dto.Dtos
{
    public class ATMLocationFilterDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public ATMLocationFilterDto()
        {

        }

        public ATMLocationFilterDto(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
