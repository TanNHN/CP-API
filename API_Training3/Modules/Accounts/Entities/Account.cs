using API_Training3.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Training3.Modules.Accounts.Entities
{
    public class Account : Document
    {
        public string DisplayName { get; set; }
        public string Phone { get; set; }
        public string PhotoUrl { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
