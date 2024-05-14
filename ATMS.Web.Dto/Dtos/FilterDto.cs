using System;
using System.Collections.Generic;
using System.Text;

namespace ATMS.Web.Dto.Dtos
{
    public class FilterDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public FilterDto()
        {

        }

        public FilterDto(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
