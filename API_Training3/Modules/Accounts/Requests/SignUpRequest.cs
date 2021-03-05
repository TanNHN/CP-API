using API_Training3.Modules.Accounts.Validate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Training3.Modules.Accounts.Requests
{
    public class SignUpRequest
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        [SignUpEmailValidation]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
