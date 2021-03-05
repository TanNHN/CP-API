using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Training3.Modules.Accounts.Response
{
    public class LoginResponse
    {
        public string Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string PhotoUrl { get; set; }
        public string Token { get; set; }
    }
}
