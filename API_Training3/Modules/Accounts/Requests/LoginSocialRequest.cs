using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Training3.Modules.Accounts.Requests
{
    public class LoginSocialRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
