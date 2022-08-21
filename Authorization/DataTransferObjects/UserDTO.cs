using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.DataTransferObjects
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}
