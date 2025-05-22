using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModels
{
    public class UserInfo
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }
        public required string Fullname { get; set; }
    }
}
