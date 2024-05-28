using ATMS.Web.Dto.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATMS.Web.Dto.Dtos
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserResponseDto : ResponseDto
    {
        public List<UserDto> Data { get; set; }
    }

    public class UserSessionDto
    {
        public Guid UserSessionId { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public DateTime SessionInterval { get; set; }
    }
}
