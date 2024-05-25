using System;
using System.Collections.Generic;
using System.Text;

namespace ATMS.Web.Dto.Models
{
    public class UserSession
    {
        public Guid UserSessionId { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public int SessionInterval { get; set; }
    }
}
